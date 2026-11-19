using System.Linq.Expressions;

namespace Lab13_GabrielNA.Application.Ports.Repositories;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object?>>[] includes);
    Task<TEntity?> GetByIdAsync(int id);
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object?>>[] includes);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);
}
