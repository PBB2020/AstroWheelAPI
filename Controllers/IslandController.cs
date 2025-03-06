using AstroWheelAPI.Context;
using AstroWheelAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AstroWheelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IslandController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public IslandController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Island>>> GetIsland()
        {
            return await _context.Islands.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Island>> GetIsland(int id)
        {
            var island = await _context.Islands.FindAsync(id);


            if (island == null)
            {
                return NotFound();
            }
            return island;
        }

        [HttpPost]
        public async Task<ActionResult<Island>> CreateIsland(Island island)
        {
            _context.Islands.Add(island);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIsland), new { id = island.IslandId }, island);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIsland(int id, Island island)
        {
            if (id != island.IslandId)
            {
                return BadRequest();
            }

            _context.Entry(island).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IslandExists(id))
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
        public async Task<IActionResult> DeleteIsland(int id)
        {
            var island = await _context.Islands.FindAsync(id);
            if (island == null)
            {
                return NotFound();
            }

            _context.Islands.Remove(island);
            await _context.SaveChangesAsync();

            return NoContent();

        }
        private bool IslandExists(int id)
        {
            return _context.Islands.Any(e => e.IslandId == id);
        }
    }
}
