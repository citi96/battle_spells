namespace Battle_Spells.model.Models
{
    public record StartMatchRequest(Guid PlayerId, Guid HeroId, List<Guid> DeckCardIds);
    public record StartMatchResponse(Guid PlayerId, Guid HeroId, List<Guid> DeckCardIds);

}
