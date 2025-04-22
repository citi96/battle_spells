using Battle_Spells.Api.Effects.Interfaces;
using Battle_Spells.Api.Entities;
using Battle_Spells.Models.Enums.Card;

namespace Battle_Spells.Api.Effects.Factory
{
    public class EffectFactory
    {
        public static ICardEffect? CreateEffect(EffectDefinition definition)
        {
            return definition.EffectType switch
            {
                ECardEffectType.Damage => new DamageEffect(definition.Amount),
                ECardEffectType.Heal => new HealEffect(definition.Amount),
                ECardEffectType.Leech => new LeechEffect(definition.Amount),
                ECardEffectType.Conditional => CreateConditionalEffect(definition),
                _ => null
            };
        }

        private static ICardEffect? CreateConditionalEffect(EffectDefinition definition)
        {
            if (definition.ConditionalEffect == null || definition.Condition !=  ECardEffectCondition.Unknown)
                return null;

            // Leech on death è un caso comune, lo supportiamo direttamente
            if (definition.Condition == ECardEffectCondition.TargetDead &&
                definition.ConditionalEffect.EffectType == ECardEffectType.Leech)
            {
                return new LeechOnDeathEffect(definition.ConditionalEffect.Amount);
            }

            // Altri effetti condizionali...

            return null;
        }
    }
}
