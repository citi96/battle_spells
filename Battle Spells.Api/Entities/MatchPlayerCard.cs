using Battle_Spells.Models.Enums.Card;

namespace Battle_Spells.Api.Entities
{
    public class MatchPlayerCard
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CardId { get; set; }
        public required Card Card { get; set; }
        public int CurrentHealt { get; set; }
        public ECardLocation Location { get; set; } = ECardLocation.Unknown;
    }
}
