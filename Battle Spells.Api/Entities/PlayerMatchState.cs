namespace Battle_Spells.Api.Entities
{
    public class PlayerMatchState
    {
        public Guid Id { get; set; }

        public Guid PlayerId { get; set; }
        public Guid HeroId { get; set; }

        public required Player Player { get; set; }
        public required Hero Hero { get; set; }

        public virtual IEnumerable<MatchPlayerCard> Deck { get; set; } = [];
        public virtual IEnumerable<MatchPlayerCard> Shop { get; set; } = [];
        public virtual ICollection<MatchPlayerCard> Hand { get; set; } = [];
        public virtual ICollection<MatchPlayerCard> Graveyard { get; set; } = [];
    }
}
