using System.Linq.Expressions;

namespace Battle_Spells.Api.Repositories.Interfaces
{
    public interface IQueryableRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetByQueryAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
