using System.Linq.Expressions;
using Battle_Spells.Api.Data;
using Battle_Spells.Api.Entities;
using Battle_Spells.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Battle_Spells.Api.Repositories
{
    public class CardRepository(BattleSpellsDbContext dbContext) : BaseQueryableRepository<Card>(dbContext), ICardRepository
    {
        protected override DbSet<Card> Entities => dbContext.Cards;

        protected override IQueryable<Card>? IncludableQueryable => 
            dbContext.Cards
                .Include(c => c.Effects)
                    .ThenInclude(e => e.SubEffects)
                .Include(c => c.Effects)
                    .ThenInclude(e => e.ConditionalEffect);

        public async Task<Card?> GetCardByIdAsync(Guid cardId)
        {
            return await dbContext.Cards
                .Include(c => c.Effects)
                    .ThenInclude(e => e.SubEffects)
                .Include(c => c.Effects)
                    .ThenInclude(e => e.ConditionalEffect)
                .FirstOrDefaultAsync(c => c.Id == cardId);
        }
    }
}
