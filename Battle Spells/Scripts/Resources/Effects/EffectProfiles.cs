using Battle_Spells.Models.Enums.Card;
using Godot;
using Godot.Collections;

namespace BattleSpells.Scripts.Resources.Effects
{
    [GlobalClass]
    public partial class DamageProfile : Resource
    {
        [Export] public int Amount { get; set; } = 1;
        [Export] public bool Splash { get; set; } = false;
        [Export] public int SplashAmount { get; set; } = 0;
        [Export] public bool PierceShield { get; set; } = false;
    }

    [GlobalClass]
    public partial class HealProfile : Resource
    {
        [Export] public int Amount { get; set; } = 1;
        [Export] public bool OverhealToShield { get; set; } = false;
    }

    [GlobalClass]
    public partial class ShieldProfile : Resource
    {
        [Export] public int DamageReduction { get; set; } = 0;
        [Export] public int DurationTurns { get; set; } = 1;
        [Export] public bool ConvertUnusedReductionToHeal { get; set; } = false;
    }

    [GlobalClass]
    public partial class SummonProfile : Resource
    {
        [Export] public string TokenName { get; set; } = string.Empty;
        [Export] public PackedScene? Prefab { get; set; }
            = null;
        [Export] public int Attack { get; set; } = 0;
        [Export] public int Health { get; set; } = 0;
        [Export] public int Copies { get; set; } = 1;
        [Export] public ECardType SpawnType { get; set; } = ECardType.Minion;
        [Export] public EffectTargetQuery Placement { get; set; } = new()
        {
            Team = TargetTeam.Ally,
            Strategy = TargetingStrategy.FreeSlots,
            Row = BoardRow.Front,
            RequireFreeTile = true
        };

        [Export] public Array<CardEffect> OngoingEffects { get; set; } = [];
        [Export] public Array<CardEffect> DeathEffects { get; set; } = [];
    }
}
