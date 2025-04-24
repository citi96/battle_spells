namespace Battle_Spells.Api.Services.Interfaces
{
    public interface IHeroService
    {
        Task<bool> ValidateHeroOwnershipAsync(Guid playerId, Guid heroId);
    }
}
