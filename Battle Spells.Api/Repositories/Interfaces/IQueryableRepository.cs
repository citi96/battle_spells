using System.Linq.Expressions;

namespace Battle_Spells.Api.Repositories.Interfaces
{
    public interface IQueryableRepository<T>
    {
        Task<IEnumerable<T>> GetByQueryAsync(Expression<Func<T, bool>> predicate);
    }
}
