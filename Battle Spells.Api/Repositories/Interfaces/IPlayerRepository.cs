using Battle_Spells.Api.Entities;

namespace Battle_Spells.Api.Repositories.Interfaces
{
    public interface IPlayerRepository : IQueryableRepository<Player>
    {
        Task<Player?> GetPlayerByIdAsync(Guid playerId);
        Task<List<Player>> GetPlayersByMatchIdAsync(Guid matchId);
        Task AddPlayerAsync(Player player);
        Task UpdatePlayerAsync(Player player);
    }
}
