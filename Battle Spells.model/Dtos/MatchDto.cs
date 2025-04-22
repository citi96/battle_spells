namespace Battle_Spells.Models.DTOs
{
    public record JoinMatchRequest(Guid MatchId, Guid PlayerId, Guid HeroId, List<Guid> DeckCardIds);
    public record MatchActionRequest(Guid MatchId, Guid PlayerId, string Action);
}
