using Microsoft.EntityFrameworkCore;
using PicPaySimplificado.Domain;
using PicPaySimplificado.Domain.Repositories;

namespace PicPaySimplificado.Infrastructure;

public class SellerUserRepository : ISellerUserRepository
{
    private readonly AppDbContext _context;

    public SellerUserRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> CnpjExisteAsync(string cnpj)
    {
        return await _context.Seller.AnyAsync(u => u.Cnpj == cnpj);
    }

    public async Task<bool> EmailExisteAsync(string email)
    {
        return await _context.Seller.AnyAsync(u => u.Email == email);
    }

    public async Task AddAsync(SellerUserEntity user)
    {
        _context.Seller.Add(user);
        await _context.SaveChangesAsync();
    }
}