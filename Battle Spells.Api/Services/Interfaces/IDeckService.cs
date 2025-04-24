namespace Battle_Spells.Api.Services.Interfaces
{
    public interface IDeckService
    {
        Task<bool> ValidateUpgradesOwnershipAsync(Guid playerId, Guid heroId, IEnumerable<Guid> cardIds);
        Task<bool> ValidateShopOwnershipAsync(Guid playerId, IEnumerable<Guid> cardIds);
    }
}
