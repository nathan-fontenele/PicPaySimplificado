namespace PicPaySimplificado.Domain.Repositories
{
    public interface ICommonUserRepository
    {
        Task<bool> CpfExisteAsync(string cpf);
        Task<bool> EmailExisteAsync(string email);
        Task AddAsync(CommonUsersEntity user);
    }
}
