﻿namespace Battle_Spells.Api.Entities
{
    public class Player : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int MMR { get; set; }
        public Guid? MatchId { get; set; }
        public virtual ICollection<PlayerCard> PlayerCards { get; set; } = [];
        public virtual ICollection<PlayerHero> PlayerHeros { get; set; } = [];
    }
}
