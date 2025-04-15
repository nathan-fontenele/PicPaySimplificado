using Microsoft.EntityFrameworkCore;
using PicPaySimplificado.Domain;
using PicPaySimplificado.Domain.Repositories;
using PicPaySimplificado.ValueObject;

namespace PicPaySimplificado.Infrastructure;

public class UsersRepository : IUsersRepository
{
    private readonly AppDbContext _context;

    public UsersRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> DocumentExistAsync(string documentNumber)
    {
        return await _context.Users
            .AnyAsync(u => EF.Property<string>(EF.Property<object>(u, "_document"), "DocumentNumber") == documentNumber);

    }


    public async Task<bool> EmailExistAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => EF.Property<string>(u, "_email") == email);
    }
    

    public async Task<Users?> FindUserByDocumentIdAsync(string documentId)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => EF.Property<string>(EF.Property<object>(u, "_document"), "DocumentNumber") == documentId);
    }

    public Users FindUserByIdAsync(string userId)
    {
        if (!Guid.TryParse(userId, out Guid guid))
        {
            throw new ArgumentException("Invalid GUID format.");
        }

        return  _context.Users
            .FirstOrDefault(u => EF.Property<Guid>(u, "_guid") == guid); 
    }
    
    public async Task AddAsync(Users user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }


}