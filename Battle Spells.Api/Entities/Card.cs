namespace Battle_Spells.Api.Entities
{
    public class Card
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Flavor { get; set; } = string.Empty;
        public int MaxHealth { get; set; }
        public int ManaCost { get; set; }
        public int Attack { get; set; }
        public virtual ICollection<EffectDefinition> Effects { get; set; } = [];
        public virtual ICollection<PlayerCard> PlayerCards { get; set; } = [];
    }
}
