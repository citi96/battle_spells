namespace Battle_Spells.Api.Entities
{
    public class MatchAction
    {
        public Guid Id { get; set; }
        public Guid MatchId { get; set; }
        public Guid PlayerId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string ActionType { get; set; } = string.Empty;
        public Guid? SourceCardId { get; set; }
        public Guid? TargetCardId { get; set; }
        public bool IsProcessed { get; set; } = false;

        public virtual Match Match { get; set; } = null!;
        public virtual Player Player { get; set; } = null!;
        public virtual Card? SourceCard { get; set; }
        public virtual Card? TargetCard { get; set; }
        public virtual ICollection<MatchStateChange> ProcessedChanges { get; set; } = [];

    }
}
