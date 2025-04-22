using System.Linq.Expressions;
using Battle_Spells.Api.Data;
using Battle_Spells.Api.Entities;
using Battle_Spells.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Battle_Spells.Api.Repositories
{
    public class PlayerCardRepository(BattleSpellsDbContext dbContext) : IPlayerCardRepository
    {
        public async Task<IEnumerable<PlayerCard>> GetByQueryAsync(Expression<Func<PlayerCard, bool>> predicate)
        {
            return await dbContext.PlayerCards
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Card>> GetCardsByPlayerIdAsync(Guid playerId)
        {
            return await dbContext.PlayerCards
                .Where(pc => pc.PlayerId == playerId)
                .Select(pc => pc.Card)
                .ToListAsync();
        }
    }
}
