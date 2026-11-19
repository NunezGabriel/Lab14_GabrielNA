namespace Lab13_GabrielNA.Application.Ports.Repositories;

public interface IUnitOfWork
{
    IOrderRepository Orders { get; }
    IProductRepository Products { get; }
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
    Task<int> Complete();
}
