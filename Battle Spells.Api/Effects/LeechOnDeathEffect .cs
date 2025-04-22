using Battle_Spells.Api.Effects.Context;
using Battle_Spells.Api.Effects.Interfaces;
using Battle_Spells.Models.Enums.Card;

namespace Battle_Spells.Api.Effects
{
    public class LeechOnDeathEffect(int amount) : CardEffect
    {
        public override ECardEffectType EffectType => ECardEffectType.Conditional;

        public override async Task<EffectResult> ExecuteAsync(EffectContext context)
        {
            if (context.TargetCard is null || context.SourceCard is null)
                return new EffectResult { Success = false };

            var isTargetDying = context.TargetCard.CurrentHealt <= 0;

            if (isTargetDying)
            {
                // Riusa LeechEffect quando la condizione è soddisfatta
                var leechEffect = new LeechEffect(amount);
                return await leechEffect.ExecuteAsync(context);
            }

            return new EffectResult { Success = true };
        }
    }
}
