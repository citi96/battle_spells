using Battle_Spells.Models.Enums.Match;

namespace Battle_Spells.Api.Entities
{
    public class MatchStateChange
    {
        public Guid Id { get; set; }
        public Guid MatchId { get; set; }
        public Guid ActionId { get; set; }
        public EMatchStateChangeType StateChangeType { get; set; }
        public Guid TargetId { get; set; }
        public string SerializedData { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public int Sequence { get; set; }

        public virtual Match Match { get; set; } = null!;
        public virtual MatchAction Action { get; set; } = null!;
    }
}
