using Battle_Spells.Api.Data;
using Battle_Spells.Api.Entities;
using Battle_Spells.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Battle_Spells.Api.Repositories
{
    public class MatchPlayerCardRepository(BattleSpellsDbContext dbContext) : QueryableRepository<MatchPlayerCard>, IMatchPlayerCardRepository
    {
        protected override DbSet<MatchPlayerCard> Entities => dbContext.MatchPlayerCards;
        protected override IIncludableQueryable<MatchPlayerCard, MatchPlayerCard?>? IncludableQueryable => null;
    }
}
