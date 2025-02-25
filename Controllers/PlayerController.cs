using AstroWheelAPI.Context;
using AstroWheelAPI.DTOs;
using AstroWheelAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AstroWheelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlayerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTOs.PlayerDTO>>> GetPlayers()
        {
            var players = await _context.Players
                .Include(p => p.Character)
                .Include(p => p.Island)
                .Include(p => p.Inventory)// Inventory betöltése
                .Select(p => new DTOs.PlayerDTO
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
                    CharacterName = p.Character != null ? p.Character.AstroSign : null,// A karakter neve az Astro_sign mezőből jön
                    IslandName = p.Island != null ? p.Island.IslandName : null// A sziget neve az Island táblából jön
                })
                .ToListAsync();

            return Ok(players);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DTOs.PlayerDTO>> GetPlayer(int id)
        {
            var player = await _context.Players
                .Include(p => p.Character)
                .Include(p => p.Island)
                .Include(p => p.Inventory)
                .Where(p => p.PlayerId == id)
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
                    IslandName = p.Island != null ? p.Island.IslandName : null
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