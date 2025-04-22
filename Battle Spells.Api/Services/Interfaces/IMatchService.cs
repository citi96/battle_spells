using Battle_Spells.Api.Entities;
using Battle_Spells.Models.Dtos;

namespace Battle_Spells.Api.Services.Interfaces
{
    public interface IMatchService
    {
        Task<Match> CreateMatchAsync(Guid playerId, Guid heroId, List<Guid> deckCardIds);
        Task<Match> CreateMatchAsync(Player player, Hero hero, List<Guid> deckCardIds);
        Task<Match> JoinMatchAsync(Guid matchId, Guid playerId, Guid heroId, List<Guid> deckCardIds);
        Task<Match> JoinMatchAsync(Match match, Player player, Hero hero, List<Guid> deckCardIds);
    }
}
