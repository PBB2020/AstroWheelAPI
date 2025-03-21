using AstroWheelAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AstroWheelAPI.Context;
using AstroWheelAPI.DTOs;
using System.Collections.Concurrent;

namespace AstroWheelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InventoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTOs.InventoryWithPlayerDTO>>> GetInventories()
        {
            var inventories = await _context.Inventories
                .Include(i => i.InventoryMaterials)
                .ThenInclude(im => im.Material)
                .ToListAsync();

            var inventoryDTOs = inventories.Join(_context.Players,
                inventory => inventory.InventoryId,
                player => player.InventoryId,
                (inventory, player) => new InventoryWithPlayerDTO
                {
                    InventoryId = inventory.InventoryId,
                    TotalScore = inventory.TotalScore,
                    PlayerId = player.PlayerId,
                    PlayerName = player.PlayerName
                }).ToList();

            return Ok(inventoryDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryWithPlayerDTO>> GetInventory(int id)
        {
            var inventory = await _context.Inventories
                .Include(i => i.InventoryMaterials)
                .ThenInclude(im => im.Material)
                .FirstOrDefaultAsync(i => i.InventoryId == id);

            if (inventory == null)
            {
                return NotFound();
            }

            var player = await _context.Players.FirstOrDefaultAsync(p => p.InventoryId == id);

            if (player ==  null)
            {
                return NotFound(); // Ha nincs játékos az adott InventoryId-val
            }

            var inventoryDTO = new InventoryWithPlayerDTO
            {
                InventoryId = inventory.InventoryId,
                TotalScore = inventory.TotalScore,
                PlayerId = player.PlayerId,
                PlayerName = player.PlayerName
            };

            return Ok(inventoryDTO);
        }

        [HttpPost]

        public async Task<ActionResult<InventoryWithPlayerDTO>> PostInventory(Inventory inventory)
        {
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();

            var player = await _context.Players.FirstOrDefaultAsync(p => p.InventoryId == inventory.InventoryId);

            if (player == null)
            {
                return NotFound("Player not found for the given InventoryId.");
            }

            var inventoryDTO = new InventoryWithPlayerDTO
            {
                InventoryId = inventory.InventoryId,
                TotalScore = inventory.TotalScore,
                PlayerId = player.PlayerId,
                PlayerName = player.PlayerName
            };

            return CreatedAtAction("GetInventory", new { id = inventory.InventoryId }, inventoryDTO);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> PutInventory(int id, InventoryWithPlayerDTO inventoryDTO)
        {
            if (id != inventoryDTO.InventoryId)
            {
                return BadRequest();
            }

            var inventory = await _context.Inventories.FindAsync(id);

            if (inventory == null)
            {
                return NotFound();
            }

            inventory.TotalScore = inventoryDTO.TotalScore; //Csak a TotalScore-t frissítjük

            _context.Entry(inventory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryExists(id))
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
        public async Task<ActionResult> DeleteInventory(int id)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }

            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InventoryExists(int id)
        {
            return _context.Inventories.Any(e => e.InventoryId == id);
        }
    }
}
