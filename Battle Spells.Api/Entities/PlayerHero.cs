namespace Battle_Spells.Api.Entities
{
    public class PlayerHero
    {
        public Guid PlayerId { get; set; }
        public Guid HeroId { get; set; }

        public virtual Player Player { get; set; } = null!;
        public virtual Hero Hero { get; set; } = null!;
    }
}
