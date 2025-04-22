using Battle_Spells.Api.Hubs;
using Battle_Spells.Api.Services.Interfaces;
using Battle_Spells.Models.Dtos;
using Battle_Spells.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Battle_Spells.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController(IMatchService matchService, IHubContext<MatchHub> hubContext, ILogger<MatchController> logger) : ControllerBase
    {      
        private Guid GetCurrentPlayerId()
        {
            // Ottieni l'ID del giocatore da JWT claim
            var userIdClaim = User.FindFirst("sub")?.Value;
            return Guid.Parse(userIdClaim ?? throw new InvalidOperationException("User ID not found in token"));
        }
    }

}
