using Battle_Spells.Api.Data;
using Battle_Spells.Api.Entities;
using Battle_Spells.Api.Repositories.Interfaces;
using Battle_Spells.Models.Enums.Card;
using Microsoft.EntityFrameworkCore;

namespace Battle_Spells.Api.Repositories
{
    public class EffectDefinitionRepository(BattleSpellsDbContext dbContext) : BaseQueryableRepository<EffectDefinition>(dbContext), IEffectDefinitionRepository
    {
        protected override DbSet<EffectDefinition> Entities => dbContext.EffectDefinitions;

        protected override IQueryable<EffectDefinition>? IncludableQueryable =>
            dbContext.EffectDefinitions
                .Include(e => e.SubEffects)
                .Include(e => e.ConditionalEffect);

        public async Task<EffectDefinition?> FindReusableEffectByTypeAsync(ECardEffectType effectType)
        {
            return await dbContext.EffectDefinitions
                .Include(e => e.SubEffects)
                .Include(e => e.ConditionalEffect)
                .FirstOrDefaultAsync(e => e.EffectType == effectType && e.CardId == null);
        }

        public async Task<IEnumerable<EffectDefinition>> GetEffectsByCardIdAsync(Guid cardId)
        {
            return await dbContext.EffectDefinitions
                .Include(e => e.SubEffects)
                .Include(e => e.ConditionalEffect)
                .Where(e => e.CardId == cardId)
                .ToListAsync();
        }
    }
}
