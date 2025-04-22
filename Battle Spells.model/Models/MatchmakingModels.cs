using Battle_Spells.Models.Enums.Match;

namespace Battle_Spells.Models.Models
{
    public record MatchmakingRequest(Guid PlayerId, Guid HeroId, List<Guid> DeckCardIds);
    public record MatchmackingResponse(Guid id, EMatchState Status);
}
