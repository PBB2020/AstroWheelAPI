using AstroWheelAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AstroWheelAPI.Context;
using AstroWheelAPI.DTOs;

namespace AstroWheelAPI.Controllers
{
    [Route("api/inventoryMaterials")]
    [ApiController]
    public class InventoryMaterialController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InventoryMaterialController> _logger; 

        public InventoryMaterialController(ApplicationDbContext context, ILogger<InventoryMaterialController> logger)
        {
            _context = context;
            _logger = logger; 
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryMaterial>>> GetInventoryMaterials()
        {
            var inventoryMaterials = await _context.InventoryMaterials.ToListAsync();

            // Explicit betöltés minden InventoryMaterial objektumhoz
            foreach (var im in inventoryMaterials)
            {
                await _context.Entry(im).Reference(im => im.Inventory).LoadAsync();
                await _context.Entry(im).Reference(im => im.Material).LoadAsync();
            }
            return inventoryMaterials;
        }

        [HttpGet("{inventoryId}/{materialId}")]
        public async Task<ActionResult<InventoryMaterial>> GetInventoryMaterial(int inventoryId, int materialId)
        {
            var inventoryMaterial = await _context.InventoryMaterials.FindAsync(inventoryId, materialId);

            if (inventoryMaterial == null)
            {
                return NotFound();
            }

            // Explicit betöltés
            await _context.Entry(inventoryMaterial).Reference(im => im.Inventory).LoadAsync();
            await _context.Entry(inventoryMaterial).Reference(im => im.Material).LoadAsync();

            return inventoryMaterial;
        }

        [HttpPost]
        public async Task<ActionResult<InventoryMaterial>> PostInventoryMaterial(InventoryMaterialDTO inventoryMaterialDTO)
        {
            //Ellenőrizzük, hogy az Inventory létezik-e
            if (!await _context.Inventories.AnyAsync(i => i.InventoryId == inventoryMaterialDTO.InventoryId))
            {
                return BadRequest("Invalid InventoryId");
            }

            //Ellenőrizzük, hgoy a Material létezik-e
            if (!await _context.Materials.AnyAsync(m => m.MaterialId == inventoryMaterialDTO.MaterialId))
            {
                return BadRequest("Invalid MaterialId");
            }

            // Ellenőrizzük, hogy a rekord már létezik-e
            var existingInventoryMaterial = await _context.InventoryMaterials.FindAsync(inventoryMaterialDTO.InventoryId, inventoryMaterialDTO.MaterialId);

            if (existingInventoryMaterial != null)
            {
                // A rekord már létezik, frissítjük a Quantity értékét
                existingInventoryMaterial.Quantity += inventoryMaterialDTO.Quantity;
                _context.Entry(existingInventoryMaterial).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                    return Ok(existingInventoryMaterial);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError(ex, "Concurrency error during InventoryMaterial update.");
                    return Conflict("Concurrency error occurred.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating InventoryMaterial.");
                    return StatusCode(500, "Internal server error.");
                }
            }
            else
            {
                // A rekord nem létezik, hozzáadjuk az újat
                var inventoryMaterial = new InventoryMaterial
                {
                    InventoryId = inventoryMaterialDTO.InventoryId,
                    MaterialId = inventoryMaterialDTO.MaterialId,
                    Quantity = inventoryMaterialDTO.Quantity
                };

                _context.InventoryMaterials.Add(inventoryMaterial);
                try
                {
                    await _context.SaveChangesAsync();

                    // Explicit betöltés hozzáadása
                    await _context.Entry(inventoryMaterial).Reference(im => im.Inventory).LoadAsync();
                    await _context.Entry(inventoryMaterial).Reference(im => im.Material).LoadAsync();

                    return CreatedAtAction(nameof(GetInventoryMaterial), new { inventoryId = inventoryMaterial.InventoryId, materialId = inventoryMaterial.MaterialId }, inventoryMaterial);
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogError(ex, "Invalid operation error during InventoryMaterial creation. Message: {Message}, StackTrace: {StackTrace}, InnerException: {InnerException}", ex.Message, ex.StackTrace, ex.InnerException);
                    return StatusCode(500, "Invalid operation error during InventoryMaterial creation.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "General error during InventoryMaterial creation. Message: {Message}, StackTrace: {StackTrace}, InnerException: {InnerException}", ex.Message, ex.StackTrace, ex.InnerException);
                    return StatusCode(500, "Internal server error during InventoryMaterial creation.");
                }
            }
        }

        [HttpPut("{inventoryId}/{materialId}")]
        public async Task<IActionResult> UpdateInventoryMaterial(int inventoryId, int materialId, InventoryMaterial inventoryMaterial)
        {
            if (inventoryId != inventoryMaterial.InventoryId || materialId != inventoryMaterial.MaterialId)
            {
                return BadRequest();
            }

            _context.Entry(inventoryMaterial).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryMaterialExists(inventoryId, materialId))
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

        [HttpDelete("{inventoryId}/{materialId}")]
        public async Task<ActionResult> DeleteInventoryMaterial(int inventoryId, int materialId)
        {
            var inventoryMaterial = await _context.InventoryMaterials.FindAsync(inventoryId, materialId);
            if (inventoryMaterial == null)
            {
                return NotFound();
            }

            _context.InventoryMaterials.Remove(inventoryMaterial);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InventoryMaterialExists(int inventoryId, int materialId)
        {
            return _context.InventoryMaterials.Any(e => e.InventoryId == inventoryId && e.MaterialId == materialId);
        }
    }
}