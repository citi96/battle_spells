using Battle_Spells.Api.Services.Interfaces;
using Battle_Spells.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Battle_Spells.Api.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController(ISyncService syncService) : ControllerBase
    {
        // POST: api/admin/syncresources
        [HttpPost("syncresources")]
        public async Task<IActionResult> SyncResources([FromBody] SyncResourcesRequest request)
        {
            var result = await syncService.SyncResourcesAsync(request);

            return Ok(new { message = "Risorse sincronizzate con successo", heroes = request.Heroes.Count, cards = request.Cards.Count });
        }
    }
}
