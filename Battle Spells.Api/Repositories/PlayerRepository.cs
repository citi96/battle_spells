using Battle_Spells.Api.Data;
using Battle_Spells.Api.Entities;
using Battle_Spells.Api.Repositories.Interfaces;
using Battle_Spells.Models.Enums.Card;
using Microsoft.EntityFrameworkCore;

namespace Battle_Spells.Api.Repositories
{
    public class PlayerRepository(BattleSpellsDbContext dbContext) : QueryableRepository<Player>, IPlayerRepository
    {
        protected override DbSet<Player> Entities => dbContext.Players;
        protected override IQueryable<Player>? IncludableQueryable => null;

        public async Task<Player?> GetPlayerByIdAsync(Guid playerId)
        {
            return await dbContext.Players
                .FirstOrDefaultAsync(p => p.Id == playerId);
        }

        public async Task<List<Player>> GetPlayersByMatchIdAsync(Guid matchId)
        {
            return await dbContext.Players
                .Where(p => p.MatchId == matchId)
                .ToListAsync();
        }

        public async Task AddPlayerAsync(Player player)
        {
            await dbContext.Players.AddAsync(player);
        }

        public Task UpdatePlayerAsync(Player player)
        {
            dbContext.Players.Update(player);
            return Task.CompletedTask;
        }
    }
}
