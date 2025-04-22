using Battle_Spells.Models.Enums.Match;

namespace Battle_Spells.Models.Dtos
{
    public record CardUseRequest(Guid MatchId, Guid SourceCardId, Guid TargetCardId, Guid PlayerId);
    public record MatchStateDto(Guid MatchId, string Name, Guid CurrentPlayerId, int TurnNumber);
    public record MatchStateChangeDto(EMatchStateChangeType Type, Guid TargetId, object Data);
    public record PlayCardRequest(Guid GameId, Guid ActionId, Guid SourceCardId, Guid? TargetCardId);
    public record PlayCardResponse(bool Success, string? ErrorMessage, IEnumerable<MatchStateChangeDto> Changes);
    public record EndTurnResponse(bool Success, string? ErrorMessage, IEnumerable<MatchStateChangeDto> Changes);
}
