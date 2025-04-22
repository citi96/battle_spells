using Battle_Spells.Models.Models;

namespace Battle_Spells.Api.Services.Interfaces
{
    public interface IMatchmakingService
    {
        Task<MatchmackingResponse> FindMatchAsync(MatchmakingRequest request);
        Task<bool> CancelMatchmakingAsync(Guid playerId);
    }
}
