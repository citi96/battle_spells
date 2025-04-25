using Battle_Spells.Models.Enums.Card;

namespace Battle_Spells.Api.Entities
{
    public class MatchPlayerCard : BaseEntity
    {
        public Guid CardId { get; set; }
        public required Card Card { get; set; }
        public int CurrentHealt { get; set; }
        public ECardLocation Location { get; set; } = ECardLocation.Unknown;

        public Guid? PlayerMatchStateHandId { get; set; }
        public Guid? PlayerMatchStateGraveyardId { get; set; }
        public Guid? PlayerMatchStateDeckId { get; set; }
        public Guid? PlayerMatchStateShopId { get; set; }
    }
}
