using Microsoft.EntityFrameworkCore;
using PicPaySimplificado.Domain;
using PicPaySimplificado.Domain.Interfaces;

namespace PicPaySimplificado.Infrastructure;

public class TransactionRepository: ITransactionRepository<Transaction, Guid>
{
    
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }
    public IQueryable<Transaction> GetAll()
    {
        return _context.Transactions
            .AsNoTracking();
    }

    public async Task<Transaction> GetById(Guid id)
    {
        return await _context.Transactions
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task AddAsync(Transaction entity)
    {
        await _context.Transactions.AddAsync(entity);
        await _context.SaveChangesAsync();  
    }

    public async Task UpdateAsync(Transaction entity)
    {
        _context.Transactions.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Transaction entity)
    {
        _context.Transactions.Remove(entity);
        await _context.SaveChangesAsync();
    }
}