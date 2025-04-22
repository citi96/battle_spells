using Battle_Spells.Api.Effects.Context;
using Battle_Spells.Api.Effects.Interfaces;
using Battle_Spells.Models.Enums.Card;
using Battle_Spells.Models.Enums.Match;

namespace Battle_Spells.Api.Effects
{
    public class DamageEffect(int damageAmount) : CardEffect
    {
        public override ECardEffectType EffectType => ECardEffectType.Damage;
   
        public override async Task<EffectResult> ExecuteAsync(EffectContext context)
        {
            if (context.TargetCard is null)
                return new EffectResult { Success = false };

            var targetCard = context.TargetCard;
            var newHealth = Math.Max(0, targetCard.CurrentHealt - damageAmount);

            // Registra il cambiamento
            AddStateChange(
                context,
                EMatchStateChangeType.UpdateHealth,
                targetCard.Id,
                new { Health = newHealth }
            );

            // Aggiorna l'entità
            targetCard.CurrentHealt = newHealth;

            var result = new EffectResult { Success = true };

            // Controllo morte carta
            if (newHealth <= 0)
            {
                //result.TriggeredEffects.Add(new DeathEffect(targetCard));
            }

            return result;
        }
    }
}
