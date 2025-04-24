using Battle_Spells.Api.Hubs;
using Battle_Spells.Api.Services.Interfaces;
using Battle_Spells.Api.Singletons.Interfaces;
using Battle_Spells.model.Enums.Hub;
using Microsoft.AspNetCore.SignalR;

namespace Battle_Spells.Api.Services
{
    public class NotificationService(IHubContext<MatchHub> hubContext, IPlayerConnectionTracker tracker, ILogger<NotificationService> logger) : INotificationService
    {
        public async Task NotifyMatchStartedAsync(Guid matchId, IReadOnlyCollection<Guid> playerIds)
        {
            var payload = new 
            { 
                Type =EHubMessageType.MatchStarted, 
                MatchId = matchId 
            };

            foreach (var playerId in playerIds)
            {
                if (tracker.TryGetConnectionId(playerId, out var cid))
                    await hubContext.Groups.AddToGroupAsync(cid!, matchId.ToString());
            }

            await hubContext.Clients.Group(matchId.ToString())
                .SendAsync(EHubEvent.ReceiveMatchEvent.ToString(), payload);
        }
    }
}