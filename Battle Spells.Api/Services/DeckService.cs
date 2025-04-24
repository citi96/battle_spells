using Battle_Spells.Api.Repositories.Interfaces;
using Battle_Spells.Api.Services.Interfaces;

namespace Battle_Spells.Api.Services
{
    public class DeckService(IPlayerCardRepository playerCardRepository) : IDeckService
    {
        private const int MAX_COPIES_SHOP = 5;
        private const int MAX_COPIES_UPGRADES = 3;  

        public async Task<bool> ValidateShopOwnershipAsync(Guid playerId, IEnumerable<Guid> cardIds)
        {
            var list = cardIds.ToList();

            // 1) limite copie shop (≤ 5 per carta)
            if (list.GroupBy(id => id).Any(g => g.Count() > MAX_COPIES_SHOP))
                return false;

            // 2) possesso sufficiente
            var cards = await playerCardRepository.GetByQueryAsync(pc =>
                pc.PlayerId == playerId && list.Contains(pc.CardId));

            var stock = cards.ToDictionary(pc => pc.CardId, pc => pc.Quantity);

            foreach (var grp in list.GroupBy(id => id))
            {
                if (!stock.TryGetValue(grp.Key, out var owned) || grp.Count() > owned)
                    return false;
            }
            return true;
        }

        public async Task<bool> ValidateUpgradesOwnershipAsync(Guid playerId, Guid heroId, IEnumerable<Guid> cardIds)
        {
            if (cardIds.GroupBy(id => id).Any(g => g.Count() > MAX_COPIES_UPGRADES))
                return false;

            var cards = await playerCardRepository.GetByQueryAsync(pc => pc.PlayerId == playerId && cardIds.Contains(pc.CardId));

            var stock = cards.ToDictionary(pc => pc.CardId, pc => new { pc.Quantity, pc.Card.HeroId });

            foreach (var group in cardIds.GroupBy(id => id))
            {
                if (!stock.TryGetValue(group.Key, out var info))
                    return false;                              

                if (group.Count() > info.Quantity)
                    return false;                              

                if (info.HeroId != heroId)
                    return false;                         
            }

            return true;
        }
    }

}
