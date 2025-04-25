using System.Linq.Expressions;
using Battle_Spells.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Battle_Spells.Api.Repositories.Interfaces
{
    public abstract class BaseQueryableRepository<TEntity>(DbContext dbContext) : IQueryableRepository<TEntity> where TEntity : BaseEntity
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

        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            if (IncludableQueryable != null)
                return await IncludableQueryable
                    .FirstOrDefaultAsync(e => e.Id == id);

            return await Entities
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddAsync(TEntity effect)
        {
            await Entities.AddAsync(effect);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity effect)
        {
            Entities.Update(effect);
            await dbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(TEntity effect)
        {
            Entities.Remove(effect);
            await dbContext.SaveChangesAsync();
        }
    }
}
