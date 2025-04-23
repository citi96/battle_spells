namespace Battle_Spells.Api.Entities
{
    public class MatchPlayerState
    {
        public Guid Id { get; set; }

        public Guid PlayerId { get; set; }
        public Guid HeroId { get; set; }
        public Guid MatchId { get; set; }

        public virtual Player Player { get; set; } = null!;
        public virtual Hero Hero { get; set; } = null!;
        public virtual Match Match { get; set; } = null!;

        public virtual ICollection<MatchPlayerCard> Deck { get; set; } = [];
        public virtual ICollection<MatchPlayerCard> Shop { get; set; } = [];
        public virtual ICollection<MatchPlayerCard> Hand { get; set; } = [];
        public virtual ICollection<MatchPlayerCard> Graveyard { get; set; } = [];
    }
}
