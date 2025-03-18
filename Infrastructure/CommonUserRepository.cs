using Microsoft.EntityFrameworkCore;
using PicPaySimplificado.Domain;
using PicPaySimplificado.Domain.Repositories;

namespace PicPaySimplificado.Infrastructure
{
    public class CommonUserRepository : ICommonUserRepository
    {
        private readonly AppDbContext _context;


        public CommonUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CpfExisteAsync(string cpf)
        {
            return await _context.Users.AnyAsync(u => u.Cpf == cpf);
        }

        public async Task<bool> EmailExisteAsync(string email)
        {
            return await _context.Users.AnyAsync(u =>  u.Email == email);
        }

        public async Task AddAsync(CommonUsersEntity user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
