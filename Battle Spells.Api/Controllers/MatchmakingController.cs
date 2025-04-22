using Battle_Spells.Api.Services.Interfaces;
using Battle_Spells.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace Battle_Spells.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchmakingController(IMatchmakingService matchmakingService, ILogger<MatchmakingController> logger) : ControllerBase
    {
        [HttpPost()]
        public async Task<IActionResult> StartMatchmaking([FromBody] MatchmakingRequest request)
        {
            logger.LogInformation($"Inizio matchmaking per giocatore: {request.PlayerId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await matchmakingService.FindMatchAsync(request);
            return Ok(result);
        }
    }
}
