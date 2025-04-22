using Battle_Spells.Api.Effects.Context;
using Battle_Spells.Api.Effects.Interfaces;
using Battle_Spells.Models.Enums.Card;

namespace Battle_Spells.Api.Effects
{
    public class LeechEffect(int amount) : CardEffect
    {
        public override ECardEffectType EffectType => ECardEffectType.Leech;

        private readonly DamageEffect _damageEffect = new (amount);
        private readonly HealEffect _healEffect = new (amount);

        public override async Task<EffectResult> ExecuteAsync(EffectContext context)
        {
            if (context.TargetCard is null || context.SourceCard is null)
                return new EffectResult { Success = false };

            // Riusa l'effetto danno  
            var damageResult = await _damageEffect.ExecuteAsync(context);

            if (!damageResult.Success)
                return damageResult;

            // Modifica il contesto per curare la carta sorgente
            var healContext = new EffectContext
            {
                Action = context.Action,
                SourceCard = context.SourceCard,
                TargetCard = context.SourceCard,  // La carta cura se stessa
                Match = context.Match,
                SequenceCounter = context.SequenceCounter
            };

            // Riusa l'effetto cura
            var healResult = await _healEffect.ExecuteAsync(healContext);

            // Sincronizza contatore sequenza
            context.SequenceCounter = healContext.SequenceCounter;

            // Unisci gli effetti attivati
            var result = new EffectResult
            {
                Success = healResult.Success,
                TriggeredEffects = damageResult.TriggeredEffects
            };

            return result;
        }
    }
}
