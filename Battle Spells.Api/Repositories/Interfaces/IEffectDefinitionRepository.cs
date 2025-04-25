using Battle_Spells.Api.Entities;
using Battle_Spells.Models.Enums.Card;

namespace Battle_Spells.Api.Repositories.Interfaces
{
    public interface IEffectDefinitionRepository : IQueryableRepository<EffectDefinition>
    {
        Task<EffectDefinition?> FindReusableEffectByTypeAsync(ECardEffectType effectType);
        Task<IEnumerable<EffectDefinition>> GetEffectsByCardIdAsync(Guid cardId);
    }
}
