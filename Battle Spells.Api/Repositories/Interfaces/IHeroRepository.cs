using Battle_Spells.Api.Entities;

namespace Battle_Spells.Api.Repositories.Interfaces
{
    public interface IHeroRepository : IQueryableRepository<Hero>
    {
        Task<Hero?> GetHeroByIdAsync(Guid playerId);
    }
}
