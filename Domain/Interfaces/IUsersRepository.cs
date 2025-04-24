namespace PicPaySimplificado.Domain.Interfaces;

public interface IUsersRepository
{
    Task<bool> DocumentExistAsync(string cnpj);
    Task<bool> EmailExistAsync(string email);
    Task AddAsync(Users user);
    Task<Users> FindUserByDocumentIdAsync(string documentId);
    Task UpdateAsync(Users user);

}