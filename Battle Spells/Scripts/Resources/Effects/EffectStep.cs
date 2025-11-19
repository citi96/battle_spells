using Godot;
using Godot.Collections;

namespace BattleSpells.Scripts.Resources.Effects
{
    [GlobalClass]
    public abstract partial class EffectStep : Resource
    {
        [Export(PropertyHint.MultilineText)] public string Notes { get; set; } = string.Empty;

        public abstract EffectResolutionResult Execute(EffectContext context);
    }

    [GlobalClass]
    public partial class DamageEffectStep : EffectStep
    {
        [Export] public DamageProfile Damage { get; set; } = new();
        [Export] public EffectTargetQuery TargetQuery { get; set; } = new();

        public override EffectResolutionResult Execute(EffectContext context)
        {
            var targets = context.Resolver.SelectTargets(TargetQuery, context);
            var result = context.Resolver.ApplyDamage(targets, Damage, context);
            context.LastResult = result;
            context.CurrentTargets = result.Targets;
            return result;
        }
    }

    [GlobalClass]
    public partial class HealEffectStep : EffectStep
    {
        [Export] public HealProfile HealProfile { get; set; } = new();
        [Export] public EffectTargetQuery TargetQuery { get; set; } = new() { Team = TargetTeam.Ally };

        public override EffectResolutionResult Execute(EffectContext context)
        {
            var targets = context.Resolver.SelectTargets(TargetQuery, context);
            var result = context.Resolver.ApplyHeal(targets, HealProfile, context);
            context.LastResult = result;
            context.CurrentTargets = result.Targets;
            return result;
        }
    }

    [GlobalClass]
    public partial class ShieldEffectStep : EffectStep
    {
        [Export] public ShieldProfile Shield { get; set; } = new();
        [Export] public EffectTargetQuery TargetQuery { get; set; } = new() { Strategy = TargetingStrategy.Self, Team = TargetTeam.SelfOnly };

        public override EffectResolutionResult Execute(EffectContext context)
        {
            var targets = context.Resolver.SelectTargets(TargetQuery, context);
            var result = context.Resolver.ApplyShield(targets, Shield, context);
            context.LastResult = result;
            context.CurrentTargets = result.Targets;
            return result;
        }
    }

    [GlobalClass]
    public partial class SummonEffectStep : EffectStep
    {
        [Export] public SummonProfile Profile { get; set; } = new();

        public override EffectResolutionResult Execute(EffectContext context)
        {
            var result = context.Resolver.Summon(Profile, context);
            context.LastResult = result;
            context.CurrentTargets = result.Targets;
            return result;
        }
    }

    [GlobalClass]
    public partial class ScaledHealEffectStep : EffectStep
    {
        [Export] public int HealPerDefeatedUnit { get; set; } = 1;
        [Export] public EffectTargetQuery TargetQuery { get; set; } = new() { Strategy = TargetingStrategy.Self, Team = TargetTeam.SelfOnly };

        public override EffectResolutionResult Execute(EffectContext context)
        {
            var defeated = context.LastResult.UnitsDefeated;
            if (defeated <= 0)
                return EffectResolutionResult.Empty;

            var healProfile = new HealProfile { Amount = defeated * HealPerDefeatedUnit };
            var targets = context.Resolver.SelectTargets(TargetQuery, context);
            var result = context.Resolver.ApplyHeal(targets, healProfile, context);
            context.LastResult = result;
            context.CurrentTargets = result.Targets;
            return result;
        }
    }

    [GlobalClass]
    public partial class StackingEffectStep : EffectStep
    {
        [Export] public string CounterKey { get; set; } = "stack";
        [Export] public int MaxStacks { get; set; } = 1;
        [Export] public int Threshold { get; set; } = 1;
        [Export] public bool ResetOnTrigger { get; set; } = true;
        [Export] public Array<EffectStep> OnThresholdReached { get; set; } = [];

        public override EffectResolutionResult Execute(EffectContext context)
        {
            var current = context.RuntimeValues.IncrementCounter(CounterKey, 1, MaxStacks);

            if (current < Threshold)
                return EffectResolutionResult.Empty;

            EffectResolutionResult? lastResult = null;
            foreach (var step in OnThresholdReached)
            {
                lastResult = step.Execute(context);
            }

            if (ResetOnTrigger)
                context.RuntimeValues.SetCounter(CounterKey, 0);

            return lastResult ?? EffectResolutionResult.Empty;
        }
    }
}
