using System.Linq.Expressions;
using Battle_Spells.Api.Data;
using Battle_Spells.Api.Entities;
using Battle_Spells.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Battle_Spells.Api.Repositories
{
    public class MatchPlayerCardRepository(BattleSpellsDbContext dbContext) : IMatchPlayerCardRepository
    {
        public async Task<IEnumerable<MatchPlayerCard>> GetByQueryAsync(Expression<Func<MatchPlayerCard, bool>> predicate)
        {
            return await dbContext.MatchPlayerCards
                .Where(predicate)
                .ToListAsync();
        }
    }
}
