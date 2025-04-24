using System.Net;
using Battle_Spells.Api.Entities;
using Battle_Spells.Api.Helpers;
using Battle_Spells.Api.Repositories.Interfaces;
using Battle_Spells.Api.Services.Interfaces;
using Battle_Spells.Models.Enums.Match;
using Battle_Spells.Models.Models;

namespace Battle_Spells.Api.Services
{
    public class MatchmakingService(IMatchRepository matchRepository, IMatchService matchService, IPlayerRepository playerRepository, INotificationService notificationService, IHeroRepository heroRepository, ILogger<MatchmakingService> logger) : IMatchmakingService
    {
        private const int MMR_RANGE_INITIAL = 100;  // Intervallo MMR iniziale per il matchmaking
        private const int MMR_RANGE_INCREMENT = 50; // Incremento dell'intervallo MMR per ogni iterazione
        private const int MAX_QUEUE_TIME_SECONDS = 120; // Tempo massimo di attesa in coda
        private const int MIN_PLAYERS_FOR_MATCH = 2; // Numero minimo di giocatori per un match

        public async Task<MatchmackingResponse> FindMatchAsync(MatchmakingRequest request)
        {
            var player = await playerRepository.GetPlayerByIdAsync(request.PlayerId) ??
                throw new APIException($"Player not found with ID {request.PlayerId}", HttpStatusCode.NotFound);

            var hero = await heroRepository.GetHeroByIdAsync(request.HeroId) ??
               throw new APIException($"Hero not found with id {request.HeroId}.", System.Net.HttpStatusCode.BadRequest);

            logger.LogInformation($"Cercando match per giocatore {player.Id} con MMR {player.MMR}");

            var match = await FindSuitableMatchAsync(player);
            if (match != null)
            {
                logger.LogInformation($"Match trovato: {match.Id} per giocatore {player.Id}");

                await matchService.JoinMatchAsync(match, player, hero, request.DeckCardIds);
                await notificationService.NotifyMatchStartedAsync(match.Id);
                return new MatchmackingResponse(match.Id, EMatchState.Started);
            }

            match = await matchService.CreateMatchAsync(player, hero, request.DeckCardIds);
            return new MatchmackingResponse(match.Id, EMatchState.Created);
        }

        public async Task<bool> CancelMatchmakingAsync(Guid playerId)
        {
            var matches = await matchRepository.GetByQueryAsync(m => m.Player1 != null && m.Player1.Id == playerId && m.State == EMatchState.Created);

            var match = matches?.FirstOrDefault();
            if (match == null)
                return false;

            match.State = EMatchState.Canceled;
            await matchRepository.UpdateMatchAsync(match);

            return true;
        }

        private async Task<Match?> FindSuitableMatchAsync(Player player)
        {
            var availableMatches = await matchRepository.GetByQueryAsync(m => m.State == EMatchState.Created);
            foreach (var match in availableMatches)
            {
                if (match.Player2 != null || match.Player1 == null)
                    continue;

                int mmrDifference = Math.Abs(match.Player1.MMR - player.MMR);
                // Usa un intervallo MMR che aumenta nel tempo
                //int timeInQueue = (int)(DateTime.UtcNow - entry.QueuedAt).TotalSeconds;
                //int allowedMmrDifference = MMR_RANGE_INITIAL + (timeInQueue / 10) * MMR_RANGE_INCREMENT;

                if (mmrDifference > MMR_RANGE_INITIAL)
                    continue;

                return match;
            }

            return null;
        }
}
}
