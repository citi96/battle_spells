using Battle_Spells.Api.Data;
using Battle_Spells.Api.Entities;
using Battle_Spells.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Battle_Spells.Api.Repositories
{
    public class PlayerCardRepository(BattleSpellsDbContext dbContext) : BaseQueryableRepository<PlayerCard>(dbContext), IPlayerCardRepository
    {
        protected override DbSet<PlayerCard> Entities => dbContext.PlayerCards;
        protected override IQueryable<PlayerCard>? IncludableQueryable => null;

        public async Task<IEnumerable<Card>> GetCardsByPlayerIdAsync(Guid playerId)
        {
            return await dbContext.PlayerCards
                .Where(pc => pc.PlayerId == playerId)
                .Select(pc => pc.Card)
                .ToListAsync();
        }
    }
}
