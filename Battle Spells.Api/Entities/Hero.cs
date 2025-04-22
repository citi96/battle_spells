using System.ComponentModel.DataAnnotations;

namespace Battle_Spells.Api.Entities
{
    public class Hero
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int BaseHP { get; set; } = 30;
        public int BaseOrbs { get; set; } = 3;

        public virtual ICollection<Card> Spells { get; set; } = [];

    }
}
