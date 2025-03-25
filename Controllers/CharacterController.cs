using AstroWheelAPI.Context;
using AstroWheelAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AstroWheelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CharacterController> _logger;

        public  CharacterController(ApplicationDbContext context, ILogger<CharacterController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Character>>> GetCharacters()
        {
            try
            {
                var characters = await _context.Characters.ToListAsync();
                return Ok(characters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving characters.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Character>> GetCharacter(int id)
        {
            try
            {
                var character = await _context.Characters.FindAsync(id);


                if (character == null)
                {
                    return NotFound();
                }
                return character;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving character with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]

        public async Task<ActionResult<Character>> PostCharacter(Character character)
        {
            try
            {
                _context.Characters.Add(character);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCharacter", new { id = character.CharacterId }, character);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating character");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCharacter(int id, Character character)
        {
            if (id != character.CharacterId)
            {
                return BadRequest();
            }

            _context.Entry(character).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CharacterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating character with ID {id}");
                return StatusCode(500, "Internal server error");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacter(int id)
        {
            try
            {
                var character = await _context.Characters.FindAsync(id);
                if (character == null)
                {
                    return NotFound();
                }

                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting character with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        private bool CharacterExists(int id)
        {
            return _context.Characters.Any(e => e.CharacterId == id);
        }
    }
}
