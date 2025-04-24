namespace PicPaySimplificado.Domain.Interfaces;

public interface ITransactionRepository<TEntity, TKey>
    where TEntity:class
{
    IQueryable<TEntity> GetAll();
    Task<Transaction> GetById(TKey id);
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}