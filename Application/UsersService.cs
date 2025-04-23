using PicPaySimplificado.Domain;
using PicPaySimplificado.Domain.Repositories;
using PicPaySimplificado.Infrastructure;
using PicPaySimplificado.ValueObject;

namespace PicPaySimplificado.Application;

public class UsersService
{
    private readonly UsersRepository _usersRepository;

    public UsersService(UsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public void ValidateSenderForTransaction(Users sender, decimal amount)
    {
        if (sender.GetUserType() == UserType.Merchant)
        {
            throw new Exception("Merchant is not allowed");
        }

        if (sender.GetBalance().CompareTo(amount) < 0)
        {
            throw new Exception("Insufficient balance");
        }
    }

    public async Task<List<Users>> GetUsers()
    {
        var users = _usersRepository.GetAll();
        return await users;
    }

    public async Task<Users> FindUserByDocumentAsync(string document)
    {
        var user = await _usersRepository.FindUserByDocumentIdAsync(document);

        if (user == null)
            throw new Exception("User not found");

        return user;
    }
    
    public async Task CreateUserAsync(string fullName, Document document, string email, string password)
    {
        if (await _usersRepository.DocumentExistAsync(document.DocumentNumber))
            throw new Exception("Document already exists");

        if (await _usersRepository.EmailExistAsync(email))
            throw new Exception("Email already exists");
        
        var user = new Users(fullName, email, document.DocumentNumber, password);
        await _usersRepository.AddAsync(user);
    }

    public async Task<Users> FindUserByIdAsync(string userId)
    {
        var user = await  this._usersRepository.FindUserByIdAsync(userId);

        if (user == null)
            throw new Exception("User not found");

        return user;
    }

    public async Task<Users> UpdateUserAsync(Users user)
    {
        await _usersRepository.UpdateAsync(user);
        return user;
    }

}