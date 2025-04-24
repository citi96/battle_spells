namespace Battle_Spells.Api.Services.Interfaces
{
    public interface INotificationService
    {
        Task NotifyMatchStartedAsync(Guid matchId, IReadOnlyCollection<Guid> players);
    }
}
