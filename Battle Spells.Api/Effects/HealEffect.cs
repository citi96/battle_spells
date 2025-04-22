using Battle_Spells.Api.Effects.Context;
using Battle_Spells.Api.Effects.Interfaces;
using Battle_Spells.Models.Enums.Card;
using Battle_Spells.Models.Enums.Match;

namespace Battle_Spells.Api.Effects
{
    public class HealEffect(int healAmount) : CardEffect
    {
        public override ECardEffectType EffectType => ECardEffectType.Heal;

        public override async Task<EffectResult> ExecuteAsync(EffectContext context)
        {
            if (context.TargetCard is null)
                return new EffectResult { Success = false };

            var targetCard = context.TargetCard;
            var newHealth = Math.Min(targetCard.Card.MaxHealth, targetCard.CurrentHealt + healAmount);

            // Registra il cambiamento
            AddStateChange(
                context,
                EMatchStateChangeType.UpdateHealth,
                targetCard.Id,
                new { Health = newHealth }
            );

            // Aggiorna l'entità
            targetCard.CurrentHealt = newHealth;

            return new EffectResult { Success = true };
        }
    }
}
