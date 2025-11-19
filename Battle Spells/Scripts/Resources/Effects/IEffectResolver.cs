using Godot;
using Godot.Collections;

namespace BattleSpells.Scripts.Resources.Effects
{
    public interface IEffectResolver
    {
        Array<EffectTarget> SelectTargets(EffectTargetQuery query, EffectContext context);
        EffectResolutionResult ApplyDamage(Array<EffectTarget> targets, DamageProfile profile, EffectContext context);
        EffectResolutionResult ApplyHeal(Array<EffectTarget> targets, HealProfile profile, EffectContext context);
        EffectResolutionResult ApplyShield(Array<EffectTarget> targets, ShieldProfile profile, EffectContext context);
        EffectResolutionResult Summon(SummonProfile profile, EffectContext context);
        EffectResolutionResult ApplyAreaDamage(EffectTargetQuery areaQuery, DamageProfile profile, EffectContext context);
        Array<EffectTarget> SelectAdjacentTargets(Array<EffectTarget> anchors, EffectTargetQuery query, EffectContext context);
    }

    public abstract partial class EffectTarget : GodotObject
    {
        public abstract string Name { get; }
    }
}
