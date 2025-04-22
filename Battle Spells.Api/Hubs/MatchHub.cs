using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Battle_Spells.Api.Hubs
{
    [Authorize]
    public class MatchHub : Hub
    {
        public async Task JoinMatchGroup(string matchId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, matchId);
            await Clients.Group(matchId).SendAsync("JoinedGroup", matchId);
        }

        public async Task LeaveMatch(string matchId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, matchId);
            await Clients.Group(matchId).SendAsync("PlayerDisconnected", GetCurrentPlayerId());
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // Optional: Handle player disconnection (e.g., notify other players, pause game)
            await base.OnDisconnectedAsync(exception);
        }

        private Guid GetCurrentPlayerId()
        {
            var userIdClaim = Context.User.FindFirst("sub")?.Value;
            return Guid.Parse(userIdClaim ?? throw new InvalidOperationException("User ID not found in token"));
        }
    }
}
