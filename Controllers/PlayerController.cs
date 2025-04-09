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
                .ToListAsync();

            var materialsDictionary = await _context.Materials
                .ToDictionaryAsync(m => m.MaterialId, m => m);

            var playerDTOs = new List<PlayerDTO>();
            foreach (var player in players)
            {
                var playerMaterials = await _context.InventoryMaterials
                    .Where(im => im.InventoryId == player.InventoryId)
                    .ToListAsync();

                var playerMaterialDTOs = playerMaterials
                    .Select(im => new PlayerMaterialDTO
                    {
                        MaterialId = im.MaterialId,
                        Quantity = im.Quantity,
                        WitchName = materialsDictionary.TryGetValue(im.MaterialId, out var material) && material != null ?
                                    material.WitchName : string.Empty,
                        EnglishName = materialsDictionary.TryGetValue(im.MaterialId, out material) && material != null ?
                                      material.EnglishName : string.Empty, 
                        LatinName = materialsDictionary.TryGetValue(im.MaterialId, out material) && material != null ?
                                    material.LatinName : string.Empty
                    })
                    .ToList();

                playerDTOs.Add(new PlayerDTO
                {
                    PlayerId = player.PlayerId,
                    PlayerName = player.PlayerName,
                    UserId = player.UserId,
                    CharacterId = player.CharacterId,
                    IslandId = player.IslandId,
                    InventoryId = player.InventoryId,
                    TotalScore = player.Inventory != null ? player.Inventory.TotalScore : 0,
                    LastLogin = player.LastLogin,
                    CreatedAt = player.CreatedAt,
                    CharacterIndex = player.Character?.CharacterIndex,
                    IslandName = player.Island?.IslandName,
                    RecipeBookId = player.RecipeBookId,
                    Materials = playerMaterialDTOs
                });
            }
            
            return Ok(playerDTOs);
        }
       
        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDTO>> GetPlayer(int id)
        {
            var player = await _context.Players
                .Include(p => p.Inventory)
                .Include(p => p.Character)
                .Include(p => p.Island)
                .FirstOrDefaultAsync(p => p.PlayerId == id);

            if (player == null)
            {
                return NotFound();
            }

            var materialsDictionary = await _context.Materials
                .ToDictionaryAsync(m => m.MaterialId, m => m);

            var playerMaterials = await _context.InventoryMaterials
                .Where(im => im.InventoryId == player.InventoryId)
                .ToListAsync();

            var playerMaterialDTOs = playerMaterials
                .Select(im => new PlayerMaterialDTO
                {
                    MaterialId = im.MaterialId,
                    Quantity = im.Quantity,
                    WitchName = materialsDictionary.TryGetValue(im.MaterialId, out var material) && material != null ?
                                    material.WitchName : string.Empty,
                    EnglishName = materialsDictionary.TryGetValue(im.MaterialId, out material) && material != null ?
                                      material.EnglishName : string.Empty, 
                    LatinName = materialsDictionary.TryGetValue(im.MaterialId, out material) && material != null ?
                                    material.LatinName : string.Empty
                })
                .ToList();

           var playerDTO = new PlayerDTO
           {
               PlayerId = player.PlayerId,
               PlayerName = player.PlayerName,
               UserId = player.UserId,
               CharacterId = player.CharacterId,
               IslandId = player.IslandId,
               InventoryId = player.InventoryId,
               TotalScore = player.Inventory != null ? player.Inventory.TotalScore : 0,
               LastLogin = player.LastLogin,
               CreatedAt = player.CreatedAt,
               CharacterIndex = player.Character?.CharacterIndex,
               IslandName = player.Island?.IslandName,
               RecipeBookId = player.RecipeBookId,
               Materials = playerMaterialDTOs
           };
 
            return Ok(playerDTO);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<PlayerDTO>> GetMyPlayer()
        {
            // Hitelesítés ellenőrzése manuálisan
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();// Visszatérés, ha nincs userId
            }

            // Csak hitelesített felhasználóknak: adatbázis-lekérdezés
            var player = await _context.Players
                .Include(p => p.Inventory)
                .Include(p => p.Character)
                .Include(p => p.Island)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (player == null)
            {
                return NotFound();
            }

            //LastLogin frissítése
            player.LastLogin = DateTime.UtcNow;
            _context.Entry(player).State = EntityState.Modified; 
            await _context.SaveChangesAsync();

            var playerMaterials = await _context.InventoryMaterials
                .Where(im => im.InventoryId == player.InventoryId)
                .ToListAsync();

            var materialsDictionary = await _context.Materials
                .ToDictionaryAsync(m => m.MaterialId, m => m);

            var playerMaterialDTOs = playerMaterials
                .Select(im => new PlayerMaterialDTO
                {
                    MaterialId = im.MaterialId,
                    Quantity = im.Quantity,
                    WitchName = materialsDictionary.TryGetValue(im.MaterialId, out var material) ?
                        material?.WitchName ?? string.Empty :
                        string.Empty,
                    EnglishName = materialsDictionary.TryGetValue(im.MaterialId, out material) ?
                          material?.EnglishName ?? string.Empty :
                          string.Empty,
                    LatinName = materialsDictionary.TryGetValue(im.MaterialId, out material) ?
                         material?.LatinName ?? string.Empty :
                         string.Empty
                })
                .ToList();

            return Ok(new PlayerDTO
            {
                PlayerId = player.PlayerId,
                PlayerName = player.PlayerName,
                UserId = player.UserId,
                CharacterId = player.CharacterId,
                IslandId = player.IslandId,
                InventoryId = player.InventoryId,
                TotalScore = player.Inventory != null ? player.Inventory.TotalScore : 0,
                LastLogin = player.LastLogin,
                CreatedAt = player.CreatedAt,
                CharacterIndex = player.Character?.CharacterIndex,
                IslandName = player.Island?.IslandName,
                RecipeBookId = player.RecipeBookId,
                Materials = playerMaterialDTOs
            });
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

            if (player.InventoryId == 0)
            {
                return NotFound("Inventory not found.");
            }

            var inventoryMaterials = await _context.InventoryMaterials
                .Where(im => im.InventoryId == player.InventoryId)
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

        /*[HttpPost] -  regisztráció során létrejön a Player */
       
        [HttpPut("{id}")]

        public async Task<IActionResult> UpdatePlayer(int id, PlayerPutDTO playerDTO)

        {
            if (id != playerDTO.PlayerId)
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

            //Csak azokat a mezőket frissítjük, amelyek változtak

            existingPlayer.PlayerName = !string.IsNullOrEmpty(playerDTO.PlayerName) ? playerDTO.PlayerName : existingPlayer.PlayerName;
            existingPlayer.IslandId = playerDTO.IslandId.HasValue ? playerDTO.IslandId : existingPlayer.IslandId;
            existingPlayer.LastLogin = DateTime.UtcNow;

            _context.Entry(existingPlayer).State = EntityState.Modified;

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