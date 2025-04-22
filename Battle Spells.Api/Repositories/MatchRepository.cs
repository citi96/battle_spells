using System.Linq.Expressions;
using Battle_Spells.Api.Data;
using Battle_Spells.Api.Entities;
using Battle_Spells.Api.Repositories.Interfaces;
using Battle_Spells.Models.Enums.Match;
using Microsoft.EntityFrameworkCore;

namespace Battle_Spells.Api.Repositories
{
    public class MatchRepository(BattleSpellsDbContext dbContext) : IMatchRepository
    {
        public async Task<Match?> GetMatchByIdAsync(Guid matchId)
        {
            return await dbContext.Matches
                .Include(g => g.Player1)
                .Include(g => g.Player2)
                .Include(g => g.CurrentPlayer)
                .Include(g => g.Player1MatchState)
                    .ThenInclude(pms => pms.Hero)
                .Include(g => g.Player1MatchState)
                    .ThenInclude(pms => pms.Hand)
                    .ThenInclude(h => h.Card)
                .Include(g => g.Player2MatchState)
                    .ThenInclude(pms => pms.Hero)
                .Include(g => g.Player2MatchState)
                    .ThenInclude(pms => pms.Hand)
                    .ThenInclude(h => h.Card)
                .FirstOrDefaultAsync(g => g.Id == matchId);
        }

        public async Task<Match?> GetMatchWithPlayersAsync(Guid matchId)
        {
            return await dbContext.Matches
                .Include(g => g.Player1)
                .Include(g => g.Player2)
                .Include(g => g.CurrentPlayer)
                .Include(g => g.Player1MatchState)
                .Include(g => g.Player2MatchState)
                .FirstOrDefaultAsync(g => g.Id == matchId);
        }

        public async Task<List<Match>> GetActiveMatchesAsync()
        {
            return await dbContext.Matches
                .Where(g => g.State == EMatchState.Started)
                .OrderByDescending(g => g.LastActionTime)
                .ToListAsync();
        }

        public async Task AddMatchAsync(Match match)
        {
            await dbContext.Matches.AddAsync(match);
        }

        public Task UpdateMatchAsync(Match match)
        {
            dbContext.Matches.Update(match);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Match>> GetByQueryAsync(Expression<Func<Match, bool>> predicate)
        {
            return await dbContext.Matches
                .Include(g => g.Player1)
                .Include(g => g.Player2)
                .Include(g => g.CurrentPlayer)
                .Include(g => g.Player1MatchState)
                    .ThenInclude(pms => pms.Hero)
                .Include(g => g.Player1MatchState)
                    .ThenInclude(pms => pms.Hand)
                    .ThenInclude(h => h.Card)
                .Include(g => g.Player2MatchState)
                    .ThenInclude(pms => pms.Hero)
                .Include(g => g.Player2MatchState)
                    .ThenInclude(pms => pms.Hand)
                    .ThenInclude(h => h.Card)
                .Where(predicate)
                .ToListAsync();
        }
    }
}
