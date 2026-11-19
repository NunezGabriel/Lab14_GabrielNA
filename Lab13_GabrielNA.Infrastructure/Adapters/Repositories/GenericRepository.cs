using System.Linq.Expressions;
using Lab13_GabrielNA.Application.Ports.Repositories;
using Lab13_GabrielNA.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Lab13_GabrielNA.Infrastructure.Adapters.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object?>>[] includes)
        => await AplicarIncludes(includes).AsNoTracking().ToListAsync();

    public async Task<TEntity?> GetByIdAsync(int id)
        => await _dbSet.FindAsync(id);

    public async Task<IEnumerable<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object?>>[] includes)
        => await AplicarIncludes(includes).Where(predicate).AsNoTracking().ToListAsync();

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        => await _dbSet.AnyAsync(predicate);

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
        => predicate is null
            ? await _dbSet.CountAsync()
            : await _dbSet.CountAsync(predicate);

    private IQueryable<TEntity> AplicarIncludes(Expression<Func<TEntity, object?>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        foreach (var include in includes)
            query = query.Include(include);

        return query;
    }
}
