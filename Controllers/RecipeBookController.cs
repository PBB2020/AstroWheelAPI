using AstroWheelAPI.Context;
using AstroWheelAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AstroWheelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeBookController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RecipeBookController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeBook>>> GetRecipeBooks()
        {
            var recipeBooks = await _context.RecipeBooks
                .ToListAsync();
            return Ok(recipeBooks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeBook>> GetRecipeBook(int id)
        {
            var recipeBook = await _context.RecipeBooks
                .FirstOrDefaultAsync(rb => rb.RecipeBookId == id);

            if (recipeBook == null)
            {
                return NotFound();
            }

            return recipeBook;

        }

        [HttpPost]
        public async Task<ActionResult<RecipeBook>> PostRecipeBook(RecipeBook recipeBook)
        {
            _context.RecipeBooks.Add(recipeBook);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipeBook", new { id = recipeBook.RecipeBookId }, recipeBook);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipeBook(int id, RecipeBook recipeBook)
        {
            if (id != recipeBook.RecipeBookId)
            {
                return BadRequest();
            }

            _context.Entry(recipeBook).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeBookExists(id))
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
        public async Task<IActionResult> DeleteRecipeBook(int id)
        {
            var recipeBook = await _context.RecipeBooks.FindAsync(id);
            if (recipeBook == null)
            {
                return NotFound();
            }

            _context.RecipeBooks.Remove(recipeBook);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecipeBookExists(int id)
        {
            return _context.RecipeBooks.Any(e => e.RecipeBookId == id);
        }
    }
}
