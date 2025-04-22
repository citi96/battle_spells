using System.Linq.Expressions;
using Battle_Spells.Api.Data;
using Battle_Spells.Api.Entities;
using Battle_Spells.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Battle_Spells.Api.Repositories
{
    public class HeroRepository(BattleSpellsDbContext dbContext) : IHeroRepository
    {
        public async Task<IEnumerable<Hero>> GetByQueryAsync(Expression<Func<Hero, bool>> predicate)
        {
            return await dbContext.Heroes
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<Hero?> GetHeroByIdAsync(Guid playerId)
        {
            return await dbContext.Heroes
                .FirstOrDefaultAsync(h => h.Id == playerId);
        }
    }
}
