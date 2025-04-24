namespace Battle_Spells.Api.Singletons.Interfaces
{
    public interface IPlayerConnectionTracker
    {
        bool TryGetConnectionId(Guid playerId, out string? cid);
        void Register(Guid playerId, string connectionId);
        void Unregister(string connectionId);
    }
}
