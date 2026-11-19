using System.Collections;
using Lab13_GabrielNA.Application.Ports.Repositories;
using Lab13_GabrielNA.Infrastructure.Context;

namespace Lab13_GabrielNA.Infrastructure.Adapters.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly Hashtable _repositories;
    private readonly ApplicationDbContext _context;

    private IOrderRepository? _orders;
    private IProductRepository? _products;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        _repositories = new Hashtable();
    }

    public IOrderRepository Orders => _orders ??= new OrderRepository(_context);

    public IProductRepository Products => _products ??= new ProductRepository(_context);

    public Task<int> Complete()
    {
        return _context.SaveChangesAsync();
    }

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity).Name;

        if (_repositories.ContainsKey(type))
        {
            return (IGenericRepository<TEntity>)_repositories[type]!;
        }

        var repositoryType = typeof(GenericRepository<>);
        var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);

        if (repositoryInstance != null)
        {
            _repositories.Add(type, repositoryInstance);
            return (IGenericRepository<TEntity>)repositoryInstance;
        }

        throw new Exception($"Could not create repository instance for type {type}");
    }
}
