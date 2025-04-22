namespace Battle_Spells.Api.Services.Interfaces
{
    public interface IDeckService
    {
        Task<bool> Validate(Guid playerId, IEnumerable<Guid> cardIds);
    }
}
