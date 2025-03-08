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
        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ApplicationDbContext context)//Módosítva
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context; //Hozzáadva
        }

        [HttpPost("register")]   
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    if (modelStateVal != null)//Null ellenőrzés hozzáadása
                    {
                        foreach (var error in modelStateVal.Errors)
                        {
                            var errorMessage = error.ErrorMessage;
                            //Logoljuk az errorMessage-t
                        }
                    }
                }
                return BadRequest(ModelState);
            }

            //Karakter keresése index alapján
            var character = await _context.Characters
                .FirstOrDefaultAsync(c => c.CharacterIndex == model.CharacterId);

            if (character == null)
            {
                return BadRequest("Invalid CharacterId");
            }

            //Inventory létrehozása
            var inventory = new Inventory
            {
                TotalScore = 0, //Alapértelmezett pontszám
            };
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PlayerName = model.PlayerName };//PlayerName hozzáadva
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                //Player entitás létrehozása
                var player = new Player
                {
                    UserId = user.Id,
                    PlayerName = model.PlayerName,
                    CharacterId = character.CharacterId, //Karakter hozzárendelése
                    InventoryId = inventory.InventoryId, //Inventory hozzárendelése
                    CreatedAt = DateTime.UtcNow // Létrehozás dátuma
                };

                _context.Players.Add(player);
                await _context.SaveChangesAsync();

                return Ok("User and Player are registered successfully!");
            }

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
