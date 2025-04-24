using Battle_Spells.Api.Hubs;
using Battle_Spells.Api.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Battle_Spells.Api.Services
{
    public class NotificationService(IHubContext<MatchHub> hubContext, ILogger<NotificationService> logger) : INotificationService
    {
        public async Task NotifyMatchStartedAsync(Guid matchId)
        {
            var payload = new
            {
                type = "MatchStarted",
                matchId = matchId.ToString()
            };

            await hubContext.Clients.Group(matchId.ToString()).SendAsync("ReceiveMatchEvent", payload);

            logger.LogInformation("Notifica MatchStarted inviata al match {MatchId}", matchId);
        }
    }
}