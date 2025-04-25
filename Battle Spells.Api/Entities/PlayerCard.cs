namespace Battle_Spells.Api.Entities
{
    public class PlayerCard : BaseEntity
    {
        public Guid PlayerId { get; set; }
        public Guid CardId { get; set; }
        public int Quantity { get; set; }

        public virtual Player Player { get; set; } = null!;
        public virtual Card Card { get; set; } = null!;
    }
}
