using AstroWheelAPI.Context;
using AstroWheelAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AstroWheelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context; //Hozzáadva
        private readonly ILogger<AuthController> _logger; //ILogger Hozzáadva
        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ApplicationDbContext context, ILogger<AuthController> logger)//Módosítva
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context; //Hozzáadva
            _logger = logger;
        }

        [HttpPost("register")]   
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state during registration: {ModelState}", ModelState); // Loggolás
                return BadRequest(ModelState);
            }

            Character? character;

            if (model.CharacterId == 0)
            {
                character = await _context.Characters.FirstOrDefaultAsync();
                if (character == null)
                {
                    _logger.LogWarning("No default Character found during registration."); // Loggolás
                    return BadRequest("No default Character found");
                }
            }
            else
            {
                character = await _context.Characters.FirstOrDefaultAsync(c => c.CharacterIndex == model.CharacterId); //Karakter keresése index alapján
                if (character == null)
                {
                    _logger.LogWarning("No default Character found during registration."); // Loggolás
                    return BadRequest("No default Character found");
                }
            }

            //Inventory létrehozása és hibakezelés
            Inventory inventory;
            try
            {
                inventory = new Inventory { TotalScore = 0 };
                _context.Inventories.Add(inventory);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Inventory during registration."); // Loggolás
                return StatusCode(500, "Error creating Inventory: " + ex.Message);
            }

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PlayerName = model.PlayerName };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Player entitás létrehozása és hibakezelés
                try
                {
                    var player = new Player
                    {
                        UserId = user.Id,
                        PlayerName = model.PlayerName,
                        CharacterId = character.CharacterId,
                        InventoryId = inventory.InventoryId,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.Players.Add(player);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("User and Player registered successfully: {UserId}", user.Id); // Loggolás
                    return Ok("User and Player are registered successfully!");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating Player during registration."); // Loggolás
                    return StatusCode(500, "Error creating Player: " + ex.Message);
                }
            }

            _logger.LogError("User registration failed: {Errors}", result.Errors); // Loggolás
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded)
                return Unauthorized("Invalid credentials");

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            string? jwtSecret = _configuration["JwtSettings:Secret"];
            if (string.IsNullOrEmpty(jwtSecret))
            {
                throw new InvalidOperationException("JWT Secret is missing from configuration.");
            }

            var key = Encoding.UTF8.GetBytes(jwtSecret);

            string? expiryMinutesStr = _configuration["JwtSettings:ExpiryMinutes"];
            if (!int.TryParse(expiryMinutesStr, out int expiryMinutes))
            {
                expiryMinutes = 60; 
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
