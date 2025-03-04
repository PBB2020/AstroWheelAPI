using AstroWheelAPI.Context;
using AstroWheelAPI.DTOs;
using AstroWheelAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AstroWheelAPI.Controllers
{
    [Route("api/players")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlayerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerDTO>>> GetPlayers()
        {
            var players = await _context.Players
                .Select(p => new PlayerDTO
                {
                    PlayerId = p.PlayerId,
                    PlayerName = p.PlayerName,
                    UserId = p.UserId,
                    CharacterId = p.CharacterId,
                    IslandId = p.IslandId,
                    InventoryId = p.InventoryId,
                    TotalScore = p.Inventory != null ? p.Inventory.TotalScore : 0, // Inventory TotalScore hozzáadása
                    LastLogin = p.LastLogin,
                    CreatedAt = p.CreatedAt,
                    CharacterName = p.Character != null ? p.Character.AstroSign : null, // A karakter neve az AstroSign mezőből jön
                    IslandName = p.Island != null ? p.Island.IslandName : null, // A sziget neve az Island táblából jön
                    RecipeBookId = p.RecipeBookId,
                })
                .ToListAsync();

            return Ok(players);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
                
            if (player == null)
            {
                return NotFound();
            }
            return Ok(player);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<PlayerDTO>> GetMyPlayer()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var player = await _context.Players
                .Select(p => new PlayerDTO
                {
                    PlayerId = p.PlayerId,
                    PlayerName = p.PlayerName,
                    UserId = p.UserId,
                    CharacterId = p.CharacterId,
                    IslandId = p.IslandId,
                    InventoryId = p.InventoryId,
                    TotalScore = p.Inventory != null ? p.Inventory.TotalScore : 0,
                    LastLogin = p.LastLogin,
                    CreatedAt = p.CreatedAt,
                    CharacterName = p.Character != null ? p.Character.AstroSign : null,
                    IslandName = p.Island != null ? p.Island.IslandName : null,
                    RecipeBookId = p.RecipeBookId
                })
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        [HttpGet("withMaterials/{id}")]
        public async Task<ActionResult<object>> GetPlayerWithMaterials(int id)
        {
            var player = await _context.Players
                .Where(p => p.PlayerId == id)
                .Select(p => new
                {
                    p.PlayerId,
                    p.PlayerName,
                    p.UserId,
                    p.CharacterId,
                    p.IslandId,
                    p.InventoryId,
                    p.RecipeBookId,
                    TotalScore = p.Inventory != null ? p.Inventory.TotalScore : 0,
                    p.LastLogin,
                    p.CreatedAt,
                    CharacterName = _context.Characters.FirstOrDefault(c => c.CharacterId == p.CharacterId) != null ? _context.Characters.FirstOrDefault(c => c.CharacterId == p.CharacterId)!.AstroSign : null,
                    IslandName = _context.Islands.FirstOrDefault(i => i.IslandId == p.IslandId) != null ? _context.Islands.FirstOrDefault(i => i.IslandId == p.IslandId)!.IslandName : null,
                    Materials = _context.InventoryMaterials
                        .Where(im => im.InventoryId == p.InventoryId)
                        .Select(im => new
                        {
                            im.Material.MaterialId,
                            im.Material.WitchName,
                            im.Material.EnglishName,
                            im.Material.LatinName,
                            im.Quantity
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        [HttpPost]
        public async Task<ActionResult<Player>> CreatePlayer(Player player)
        {
            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlayer), new { id = player.PlayerId }, player);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayer(int id, Player player)
        {
            if (id != player.PlayerId)
            {
                return BadRequest("ID mismatch");
            }

            var existingPlayer = await _context.Players
                .Include(p => p.Inventory)
                .FirstOrDefaultAsync(p => p.PlayerId == id);

            if (existingPlayer == null)
            {
                return NotFound();
            }

            // Csak azokat a mezőket frissítjük, amelyek változtak
            existingPlayer.PlayerName = !string.IsNullOrEmpty(player.PlayerName) ? player.PlayerName : existingPlayer.PlayerName;
            existingPlayer.IslandId = player.IslandId != 0 ? player.IslandId : existingPlayer.IslandId;
            existingPlayer.CharacterId = player.CharacterId != 0 ? player.CharacterId : existingPlayer.CharacterId;

            // TotalScore frissítése az Inventory-ban (mivel ott tárolódik)
            if (existingPlayer.Inventory != null)
            {
                existingPlayer.Inventory.TotalScore = player.Inventory?.TotalScore ?? existingPlayer.Inventory.TotalScore;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Players.Any(p => p.PlayerId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}