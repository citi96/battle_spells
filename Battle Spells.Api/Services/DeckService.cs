using Battle_Spells.Api.Repositories.Interfaces;
using Battle_Spells.Api.Services.Interfaces;

namespace Battle_Spells.Api.Services
{
    public class DeckService(IPlayerCardRepository playerCardRepository) : IDeckService
    {
        public async Task<bool> Validate(Guid playerId, IEnumerable<Guid> cardIds)
        {
            var playerCards = await playerCardRepository.GetCardsByPlayerIdAsync(playerId);
            var playerCardIds = playerCards.Select(c => c.Id).ToHashSet();

            if (cardIds.Any(id => !playerCardIds.Contains(id)))
                return false;

            return true;
        }
    }
}
