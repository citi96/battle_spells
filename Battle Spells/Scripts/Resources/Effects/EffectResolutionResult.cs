using Godot.Collections;

namespace BattleSpells.Scripts.Resources.Effects
{
    public sealed class EffectResolutionResult
    {
        public static readonly EffectResolutionResult Empty = new();

        public int TotalValue { get; init; }
        public int UnitsAffected { get; init; }
        public int UnitsDefeated { get; init; }
        public Array<EffectTarget> Targets { get; init; } = [];
    }
}
