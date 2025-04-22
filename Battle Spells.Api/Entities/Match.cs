using System.ComponentModel.DataAnnotations;
using Battle_Spells.Models.Enums.Match;

namespace Battle_Spells.Api.Entities
{
    public class Match
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid CurrentPlayerId { get; set; }
        public int TurnNumber { get; set; } = 1;
        public DateTime LastActionTime { get; set; } = DateTime.UtcNow;

        public virtual Player? CurrentPlayer { get; set; }

        public Guid? Player1Id { get; set; }
        public Guid? Player2Id { get; set; }
        public virtual Player? Player1 { get; set; }
        public virtual Player? Player2 { get; set; }

        public virtual PlayerMatchState? Player1MatchState { get; set; }
        public virtual PlayerMatchState? Player2MatchState { get; set; }


        public virtual ICollection<MatchAction> Actions { get; set; } = [];
        public EMatchState State { get; internal set; }
    }
}
