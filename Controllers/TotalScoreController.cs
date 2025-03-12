using AstroWheelAPI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AstroWheelAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TotalScoreController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TotalScoreController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{playerId}")]
        public async Task<ActionResult<int>> GetTotalScore(int playerId)
        {
            var player = await _context.Players
                .Include(p => p.Inventory)
                .FirstOrDefaultAsync(p => p.PlayerId == playerId);

            if (player == null || player.Inventory == null)
            {
                return NotFound();
            }

            return player.Inventory.TotalScore;
        }
    }
}
