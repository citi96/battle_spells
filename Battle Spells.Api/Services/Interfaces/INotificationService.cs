namespace Battle_Spells.Api.Services.Interfaces
{
    public interface INotificationService
    {
        Task NotifyMatchFoundAsync(Guid matchId, List<Guid> playerIds);
        Task NotifyPlayerJoinedAsync(Guid matchId, Guid playerId);
        Task NotifyMatchStartedAsync(Guid matchId);
        Task NotifyMatchEndedAsync(Guid matchId, Guid winnerId, Dictionary<Guid, int> mmrChanges);
    }
}
