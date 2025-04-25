using System.Linq.Expressions;
using Battle_Spells.Api.Entities;

namespace Battle_Spells.Api.Repositories.Interfaces
{
    public interface IQueryableRepository<TEntity>
    {
        Task<TEntity?> GetByIdAsync(Guid id);
        Task AddAsync(TEntity effect);
        Task UpdateAsync(TEntity effect);
        Task RemoveAsync(TEntity effect);
        Task<IEnumerable<TEntity>> GetByQueryAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
