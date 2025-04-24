using Battle_Spells.Api.Data;
using Battle_Spells.Api.Entities;
using Battle_Spells.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Battle_Spells.Api.Repositories
{
    public class HeroRepository(BattleSpellsDbContext dbContext) : QueryableRepository<Hero>, IHeroRepository
    {
        protected override DbSet<Hero> Entities => dbContext.Heroes;
        protected override IIncludableQueryable<Hero, Hero?>? IncludableQueryable => null;

        public async Task<Hero?> GetHeroByIdAsync(Guid playerId)
        {
            return await dbContext.Heroes
                .FirstOrDefaultAsync(h => h.Id == playerId);
        }
    }
}
