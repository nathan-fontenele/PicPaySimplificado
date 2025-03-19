using PicPaySimplificado.Domain;
using PicPaySimplificado.Domain.Repositories;

namespace PicPaySimplificado.Application
{
    public class CommomUserService
    {
        private readonly ICommonUserRepository _repository;

        public CommomUserService(ICommonUserRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateUserAsync (string fullname, string cpf, string email, string password)
        {
            if (await _repository.CpfExisteAsync(cpf))
                throw new Exception("CPF já cadastrado");

            if (await _repository.EmailExisteAsync(email))
                throw new Exception("Email já cadastrado");

            var user = new CommonUsersEntity(fullname, cpf, email, password);
            await _repository.AddAsync(user);
        }
    }
}