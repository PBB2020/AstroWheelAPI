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

        public InventoryMaterialController(ApplicationDbContext context)
        {
            _context = context;
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
            var inventoryMaterial = new InventoryMaterial
            {
                InventoryId = inventoryMaterialDTO.InventoryId,
                MaterialId = inventoryMaterialDTO.MaterialId,
                Quantity = inventoryMaterialDTO.Quantity
            };

            _context.InventoryMaterials.Add(inventoryMaterial);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInventoryMaterial), new { inventoryId = inventoryMaterial.InventoryId, materialId = inventoryMaterial.MaterialId }, inventoryMaterial);
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