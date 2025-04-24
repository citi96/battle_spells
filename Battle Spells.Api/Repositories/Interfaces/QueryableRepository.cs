using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Battle_Spells.Api.Repositories.Interfaces
{
    public abstract class QueryableRepository<TEntity> : IQueryableRepository<TEntity> where TEntity : class
    {
        protected abstract DbSet<TEntity> Entities { get; }
        protected abstract IQueryable<TEntity>? IncludableQueryable { get; }

        public async Task<IEnumerable<TEntity>> GetByQueryAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (IncludableQueryable != null)
                return await IncludableQueryable
                    .Where(predicate)
                    .ToListAsync();

            return await Entities
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
            => await Entities.AnyAsync(predicate);
    }
}
