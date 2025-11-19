using Battle_Spells.Models.Enums.Card;
using Godot;
using Godot.Collections;

namespace BattleSpells.Scripts.Resources.Effects
{
    public enum TargetingStrategy
    {
        All,
        Nearest,
        Random,
        Adjacent,
        FreeSlots,
        Self,
    }

    public enum TargetTeam
    {
        Ally,
        Enemy,
        Everyone,
        SelfOnly,
    }

    public enum BoardRow
    {
        Any,
        Front,
        Back,
    }

    [GlobalClass]
    public partial class EffectTargetQuery : Resource
    {
        [Export] public TargetTeam Team { get; set; } = TargetTeam.Enemy;
        [Export] public TargetingStrategy Strategy { get; set; } = TargetingStrategy.All;
        [Export] public BoardRow Row { get; set; } = BoardRow.Any;
        [Export] public int MaxTargets { get; set; } = 0;
        [Export(PropertyHint.Enum, "Any,Hero,Minion,Structure")] public ECardType AllowedType { get; set; } = ECardType.Unknown;
        [Export] public bool IncludeBoardHoles { get; set; } = false;
        [Export] public bool RequireFreeTile { get; set; } = false;

        public EffectTargetQuery CloneWithStrategy(TargetingStrategy strategy)
        {
            return new EffectTargetQuery
            {
                Team = Team,
                Strategy = strategy,
                Row = Row,
                MaxTargets = MaxTargets,
                AllowedType = AllowedType,
                IncludeBoardHoles = IncludeBoardHoles,
                RequireFreeTile = RequireFreeTile
            };
        }
    }
}
