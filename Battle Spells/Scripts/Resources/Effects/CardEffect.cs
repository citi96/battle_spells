using Battle_Spells.Models.Enums.Card;
using Godot;
using Godot.Collections;

namespace BattleSpells.Scripts.Resources.Effects
{
    [GlobalClass]
    public partial class CardEffect : Resource
    {
        [Export] public string EffectName { get; set; } = string.Empty;
        [Export] public string Description { get; set; } = string.Empty;
        [Export] public ECardEffectActivation Activation { get; set; } = ECardEffectActivation.Lazy;
        [Export] public Array<EffectStep> Steps { get; set; } = [];

        public EffectResolutionResult Resolve(EffectContext context)
        {
            EffectResolutionResult lastResult = EffectResolutionResult.Empty;

            foreach (var step in Steps)
            {
                lastResult = step.Execute(context);
                context.LastResult = lastResult;
            }

            return lastResult;
        }
    }
}
