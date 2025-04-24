using Battle_Spells.Api.Data;
using Battle_Spells.Api.Entities;
using Battle_Spells.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Battle_Spells.Api.Repositories
{
    public class PlayerHeroRepository(BattleSpellsDbContext dbContext) : QueryableRepository<PlayerHero>, IPlayerHeroRepository
    {
        protected override DbSet<PlayerHero> Entities => dbContext.PlayerHero;
        protected override IQueryable<PlayerHero>? IncludableQueryable => null;
    }
}
