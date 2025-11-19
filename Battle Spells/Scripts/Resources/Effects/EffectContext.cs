using BattleSpells.Scripts.Resources.Cards;
using Godot;
using Godot.Collections;

namespace BattleSpells.Scripts.Resources.Effects
{
    public sealed class EffectContext
    {
        public EffectContext(CardDefinitionBase sourceCard, IEffectResolver resolver)
        {
            SourceCard = sourceCard;
            Resolver = resolver;
        }

        public CardDefinitionBase SourceCard { get; }
        public IEffectResolver Resolver { get; }
        public EffectTarget? SourceEntity { get; init; }
        public EffectResolutionResult LastResult { get; set; } = EffectResolutionResult.Empty;
        public Array<EffectTarget> CurrentTargets { get; set; } = [];

        public RuntimeValues RuntimeValues => SourceCard.RuntimeValues;

        public EffectContext WithTargets(Array<EffectTarget> targets)
        {
            CurrentTargets = targets;
            return this;
        }
    }
}
