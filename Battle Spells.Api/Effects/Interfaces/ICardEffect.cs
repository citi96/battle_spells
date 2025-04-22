using Battle_Spells.Api.Effects.Context;
using Battle_Spells.Models.Enums.Card;

namespace Battle_Spells.Api.Effects.Interfaces
{
    public interface ICardEffect
    {
        ECardEffectType EffectType { get; }
        Task<EffectResult> ExecuteAsync(EffectContext context);
    }
}
