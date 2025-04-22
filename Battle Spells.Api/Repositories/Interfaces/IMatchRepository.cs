using System.Linq.Expressions;
using Battle_Spells.Api.Entities;
using Battle_Spells.Models.Enums;

namespace Battle_Spells.Api.Repositories.Interfaces
{
    public interface IMatchRepository : IQueryableRepository<Match>
    {
        Task<Match?> GetMatchByIdAsync(Guid matchId);
        Task<Match?> GetMatchWithPlayersAsync(Guid matchId);
        Task<List<Match>> GetActiveMatchesAsync();
        Task AddMatchAsync(Match match);
        Task UpdateMatchAsync(Match match);
    }
}
