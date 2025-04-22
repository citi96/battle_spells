using Battle_Spells.Api.Entities;

namespace Battle_Spells.Api.Repositories.Interfaces
{
    public interface IPlayerCardRepository : IQueryableRepository<PlayerCard>
    {
        Task<IEnumerable<Card>> GetCardsByPlayerIdAsync(Guid playerId);
    }
}
