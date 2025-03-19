namespace PicPaySimplificado.Domain.Repositories;

public interface ISellerUserRepository
{
    Task<bool> CnpjExisteAsync(string cnpj);
    Task<bool> EmailExisteAsync(string email);
    Task AddAsync(SellerUserEntity user);
}