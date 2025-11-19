using Godot;
using BattleSpells.Scripts.Resources.Effects;

namespace BattleSpells.Scripts.Resources.Cards
{
    [GlobalClass]
    public partial class SpellCardDefinition : CardDefinitionBase
    {
        public override bool CanUseCard()
        {
            return Effects.Count > 0 && EffectResolver != null;
        }

        public override bool UseCard()
        {
            if (EffectResolver == null)
            {
                GD.PushWarning($"No resolver configured for {Name}.");
                return false;
            }

            return ResolveEffects(EffectResolver);
        }
    }
}
