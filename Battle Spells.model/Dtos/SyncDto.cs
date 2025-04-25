namespace Battle_Spells.Models.DTOs
{
    public record SyncResourcesRequest(List<HeroRequest> Heroes, List<CardDto> Cards);
}
