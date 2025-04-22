namespace Battle_Spells.Api.Entities
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int MMR { get; set; }

        public virtual ICollection<PlayerCard> PlayerCards { get; set; } = [];
    }
}
