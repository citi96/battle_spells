using Battle_Spells.Api.Repositories.Interfaces;

namespace Battle_Spells.Api.Services.Interfaces
{
    public class HeroService(IHeroRepository heroRepository, IPlayerHeroRepository playerHeroRepository) : IHeroService
    {
        public async Task<bool> ValidateHeroOwnershipAsync(Guid playerId, Guid heroId)
            => await playerHeroRepository.ExistsAsync(ph => ph.HeroId == heroId && ph.PlayerId == playerId);
    }
}
