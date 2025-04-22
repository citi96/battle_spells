using Battle_Spells.Api.Entities;

namespace Battle_Spells.Api.Effects.Context
{
    public class EffectContext
    {
        public required MatchAction Action { get; init; }
        public required MatchPlayerCard SourceCard { get; init; }
        public MatchPlayerCard? TargetCard { get; init; }
        public Player? TargetPlayer { get; init; }
        public Match Match { get; init; } = null!;
        public List<MatchStateChange> StateChanges { get; } = [];
        public int SequenceCounter { get; set; } = 0;
    }
}
