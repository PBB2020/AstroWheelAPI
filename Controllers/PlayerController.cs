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
                .Include(p => p.Inventory)
                .Include(p => p.Character)
                .Include(p => p.Island)
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
        public async Task<ActionResult<PlayerDTO>> GetPlayer(int id)
        {
            var player = await _context.Players
                .Include(p => p.Inventory)
                .Include(p => p.Character)
                .Include(p => p.Island)
                .Select(p => new PlayerDTO
                {
                    PlayerId = p.PlayerId,
                    PlayerName = p.PlayerName, //PlayerName hozzáadása
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
                .FirstOrDefaultAsync(p => p.PlayerId == id);
                
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
                .Include(p => p.Inventory)
                .Include(p => p.Character)
                .Include(p => p.Island)
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

        [HttpGet("{playerId}/materials")]
        public async Task<ActionResult<IEnumerable<PlayerMaterialDTO>>> GetPlayerMaterials(int? playerId)
        {
            if(!playerId.HasValue)
            {
                return BadRequest("PlayerId is required.");
            }

            var player = await _context.Players
                .Include(p => p.Inventory)
                .FirstOrDefaultAsync(p => p.PlayerId == playerId.Value);

            if (player == null)
            {
                return NotFound();
            }

            if (!player.InventoryId.HasValue)
            {
                return NotFound("Inventory not found.");
            }

            var inventoryMaterials = await _context.InventoryMaterials
                .Where(im => im.InventoryId == player.InventoryId.Value)
                .ToListAsync();

            var playerMaterials = inventoryMaterials.Select(im => new PlayerMaterialDTO
            {
                MaterialId = im.MaterialId,
                Quantity = im.Quantity,
                // Lekérdezzük az anyag adatait az InventoryMaterial alapján
                WitchName = _context.Materials.FirstOrDefault(m => m.MaterialId == im.MaterialId)?.WitchName ?? string.Empty,
                EnglishName = _context.Materials.FirstOrDefault(m => m.MaterialId == im.MaterialId)?.EnglishName ?? string.Empty,
                LatinName = _context.Materials.FirstOrDefault(m => m.MaterialId == im.MaterialId)?.LatinName ?? string.Empty
            }).ToList();

            return playerMaterials;
        }

/*[HttpPost]
[Authorize(Roles = "Admin")]// Csak az adminok hozhatnak létre játékost, egyébként regisztráció során létrejön a Player
public async Task<ActionResult<Player>> CreatePlayer(Player player)
{
    // Ellenőrizzük, hogy a megadott UserId létezik-e
    var user = await _context.Users.FindAsync(player.UserId);
    if (user == null)
    {
        return BadRequest("Invalid UserId");
    }

    _context.Players.Add(player);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetPlayer), new { id = player.PlayerId }, player);
}*/

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
            existingPlayer.IslandId = player.IslandId.HasValue ? player.IslandId : existingPlayer.IslandId;
            existingPlayer.CharacterId = player.CharacterId.HasValue ? player.CharacterId : existingPlayer.CharacterId;
            existingPlayer.InventoryId = player.InventoryId.HasValue ? player.InventoryId : existingPlayer.InventoryId;
            existingPlayer.RecipeBookId = player.RecipeBookId.HasValue ? player.RecipeBookId : existingPlayer.RecipeBookId;
            existingPlayer.LastLogin = DateTime.UtcNow;

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