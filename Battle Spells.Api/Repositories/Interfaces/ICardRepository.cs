using Battle_Spells.Api.Entities;

namespace Battle_Spells.Api.Repositories.Interfaces
{
    public interface ICardRepository : IQueryableRepository<Card>
    {
        Task<Card?> GetCardByIdAsync(Guid cardId);
    }
}
