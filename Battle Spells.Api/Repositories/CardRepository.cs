using System.Linq.Expressions;
using Battle_Spells.Api.Data;
using Battle_Spells.Api.Entities;
using Battle_Spells.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Battle_Spells.Api.Repositories
{
    public class CardRepository(BattleSpellsDbContext dbContext) : ICardRepository
    {
        public async Task<Card?> GetCardByIdAsync(Guid cardId)
        {
            return await dbContext.Cards
                .Include(c => c.Effects)
                .FirstOrDefaultAsync(c => c.Id == cardId);
        }

        public async Task<IEnumerable<Card>> GetByQueryAsync(Expression<Func<Card, bool>> predicate)
        {
            return await dbContext.Cards
                .Include(c => c.Effects)
                .Where(predicate)
                .ToListAsync();
        }
    }
}
