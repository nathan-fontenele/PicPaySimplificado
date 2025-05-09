﻿using PicPaySimplificado.Domain;
using PicPaySimplificado.Domain.Interfaces;
using PicPaySimplificado.Infrastructure;
using PicPaySimplificado.ValueObject;

namespace PicPaySimplificado.Application;

public class Users
{
    private readonly UsersRepository _usersRepository;

    public Users(UsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public bool ValidateSenderForTransaction(Domain.Users sender, decimal amount)
    {
        if (sender.GetUserType() == UserType.Merchant)
        {
            throw new Exception("Merchant is not allowed");
        }

        if (sender.GetBalance().CompareTo(amount) < 0)
        {
            throw new Exception("Insufficient balance");
        }
        
        return true;
    }

    public async Task CreateUserAsync(string fullName, Document document, decimal balance, string email, string password)
    {
        if (await _usersRepository.DocumentExistAsync(document.DocumentNumber))
            throw new Exception("Document already exists");

        if (await _usersRepository.EmailExistAsync(email))
            throw new Exception("Email already exists");
        
        var user = new Domain.Users(fullName, document.DocumentNumber, balance, email,  password);
        await _usersRepository.AddAsync(user);
    }
    
    public async Task<List<Domain.Users>> GetUsers()
    {
        return await _usersRepository.GetAll();
    }

    public async Task<Domain.Users> FindUserByDocumentAsync(string document)
    {
        var user = await _usersRepository.FindUserByDocumentIdAsync(document);

        if (user == null)
            throw new Exception("User not found");

        return user;
    }
    
    public async Task<Domain.Users> FindUserByIdAsync(string userId)
    {
        var user = await  this._usersRepository.FindUserByIdAsync(userId);

        if (user == null)
            throw new Exception("User not found");

        return user;
    }

    public async Task<Domain.Users> UpdateUserAsync(Domain.Users user)
    {
        await _usersRepository.UpdateAsync(user);
        return user;
    }

}