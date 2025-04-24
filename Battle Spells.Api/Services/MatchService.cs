using Battle_Spells.Api.Entities;
using Battle_Spells.Api.Helpers;
using Battle_Spells.Api.Repositories.Interfaces;
using Battle_Spells.Api.Services.Interfaces;
using Battle_Spells.Models.Enums.Card;
using Battle_Spells.Models.Enums.Match;

namespace Battle_Spells.Api.Services
{
    public class MatchService(IDeckService deckService, IMatchRepository matchRepository, IPlayerRepository playerRepository,
        IHeroRepository heroRepository, ICardRepository cardRepository) : IMatchService
    {
        public async Task<Match> CreateMatchAsync(Guid playerId, Guid heroId, List<Guid> deckCardIds)
        {
            var player = await playerRepository.GetPlayerByIdAsync(playerId) ??
               throw new APIException($"Player not found with id {playerId}.", System.Net.HttpStatusCode.BadRequest);

            var hero = await heroRepository.GetHeroByIdAsync(heroId) ??
                throw new APIException($"Hero not found with id {heroId}.", System.Net.HttpStatusCode.BadRequest);

            return await CreateMatchAsync(player, hero, deckCardIds);
        }

        public async Task<Match> CreateMatchAsync(Player player, Hero hero, List<Guid> deckCardIds)
        {
            if (!await deckService.ValidateUpgradesOwnershipAsync(player.Id, hero.Id, deckCardIds))
                throw new APIException("Invalid deck.", System.Net.HttpStatusCode.BadRequest);

            // Ottieni le carte dal repository invece di usare matchPlayerCardRepository
            var cards = await cardRepository.GetByQueryAsync(c => deckCardIds.Contains(c.Id));

            // Crea gli oggetti MatchPlayerCard per ogni carta
            var deckCards = cards.Select(card => new MatchPlayerCard
            {
                Card = card,
                CardId = card.Id,
                CurrentHealt = card.MaxHealth,
                Location = ECardLocation.Deck
            }).ToList();

            var match = new Match
            {
                Id = Guid.NewGuid(),
                State = EMatchState.Created,
                Name = $"Auto Match #{Guid.NewGuid().ToString()[..8]}",
                Player1 = player,
                Player1Id = player.Id,
                Player1MatchState = new MatchPlayerState
                {
                    Id = Guid.NewGuid(),
                    Hero = hero,
                    HeroId = hero.Id,
                    Player = player,
                    PlayerId = player.Id,
                    Deck = deckCards
                }
            };

            await matchRepository.AddMatchAsync(match);
            return match;
        }

        public async Task<Match> JoinMatchAsync(Guid matchId, Guid playerId, Guid heroId, List<Guid> deckCardIds)
        {
            var match = await matchRepository.GetMatchByIdAsync(matchId) ??
                throw new APIException($"Match with id {matchId} not found.", System.Net.HttpStatusCode.NotFound);

            if (match.Player2 != null)
                throw new APIException($"Match full.", System.Net.HttpStatusCode.BadRequest);

            var player = await playerRepository.GetPlayerByIdAsync(playerId) ??
              throw new APIException($"Player not found with id {playerId}.", System.Net.HttpStatusCode.BadRequest);

            var hero = await heroRepository.GetHeroByIdAsync(heroId) ??
                throw new APIException($"Hero not found with id {heroId}.", System.Net.HttpStatusCode.BadRequest);

            return await JoinMatchAsync(match, player, hero, deckCardIds);
        }

        public async Task<Match> JoinMatchAsync(Match match, Player player, Hero hero, List<Guid> deckCardIds)
        {
            if (!await deckService.ValidateUpgradesOwnershipAsync(player.Id, hero.Id, deckCardIds))
                throw new APIException("Invalid deck.", System.Net.HttpStatusCode.BadRequest);

            if (!await deckService.ValidateUpgradesOwnershipAsync(player.Id, hero.Id, deckCardIds))
                throw new APIException("Invalid deck.", System.Net.HttpStatusCode.BadRequest);

            var cards = await cardRepository.GetByQueryAsync(c => deckCardIds.Contains(c.Id));
            var deckCards = cards.Select(card => new MatchPlayerCard
            {
                Card = card,
                CardId = card.Id,
                CurrentHealt = card.MaxHealth,
                Location = ECardLocation.Deck
            }).ToList();

            match.Player2 = player;
            match.Player2Id = player.Id;
            match.Player2MatchState = new MatchPlayerState
            {
                Id = Guid.NewGuid(),
                HeroId = hero.Id,
                Hero = hero,
                PlayerId = player.Id,
                Player = player,
                Deck = deckCards
            };

            await matchRepository.UpdateMatchAsync(match);
            return match;
        }
    }
}