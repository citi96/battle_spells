using Battle_Spells.Api.Singletons.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Battle_Spells.Api.Hubs
{
    [Authorize]
    public class MatchHub(IPlayerConnectionTracker tracker, ILogger<MatchHub> log) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var pid = Guid.Parse(Context.User!.FindFirst("pid")!.Value);

            tracker.Register(pid, Context.ConnectionId);

            await Groups.AddToGroupAsync(Context.ConnectionId, pid.ToString());

            log.LogInformation($"Player {pid} connesso WS ({Context.ConnectionId})");
        }

        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            tracker.Unregister(Context.ConnectionId);
            await base.OnDisconnectedAsync(ex);
        }
    }
}
