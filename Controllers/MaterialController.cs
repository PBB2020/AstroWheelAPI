using AstroWheelAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AstroWheelAPI.Context;
using AstroWheelAPI.DTOs;

namespace AstroWheelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MaterialController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetMaterialWithQuantity()
        {
            var materials = await _context.Materials
                .Select(m => new
                {
                    m.MaterialId,
                    m.WitchName,
                    m.EnglishName,
                    m.LatinName,
                    Quantity = _context.InventoryMaterials
                        .Where(im => im.MaterialId == m.MaterialId)
                        .Sum(im => im.Quantity)
                })
                .ToListAsync();

            return materials;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetMaterialWithQuantity(int id)
        {
            var material = await _context.Materials.FindAsync(id);

            if (material == null)
            {
                return NotFound();
            }

            var materialWithQuantity = new
            {
                material.MaterialId,
                material.WitchName,
                material.EnglishName,
                material.LatinName,
                Quantity = _context.InventoryMaterials
                    .Where(im => im.MaterialId == material.MaterialId)
                    .Sum(im => im.Quantity)
            };

            return materialWithQuantity;
        }

        [HttpPost]
        public async Task<ActionResult<Material>> PostMaterial(Material material)
        {
            _context.Materials.Add(material);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMaterial", new { id = material.MaterialId }, material);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult>PutMaterial(int id, Material material)
        {
            if (id != material.MaterialId)
            {
                return BadRequest();
            }

            _context.Entry(material).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialExists(id))
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
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }

            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MaterialExists( int id)
        {
            return _context.Materials.Any(e => e.MaterialId == id);
        }
    }
}
