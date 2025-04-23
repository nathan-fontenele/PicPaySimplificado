using PicPaySimplificado.Domain;
using PicPaySimplificado.DTOs;
using PicPaySimplificado.Infrastructure;
using System.Runtime.Serialization;
using System.Text.Json;

namespace PicPaySimplificado.Application;

public class TransactionService
{
    private UsersService _usersService;
    private TransactionRepository _transactionRepository;
    
    public async Task CreateTransaction(TransactionDto transaction)
    {
        Users sender = await this._usersService.FindUserByIdAsync(transaction.senderId);
        Users receiver = await this._usersService.FindUserByIdAsync(transaction.receiverId);

        ValidateTransaction(sender, transaction.value);

        bool isAuthorize = this.ValidateTransaction(sender, transaction.value);

        if (!isAuthorize) {
            throw new Exception("Unauthorized transaction");
        }

        Transaction newTransaction = new Transaction();
        newTransaction.setAmount(transaction.value);
        newTransaction.setSender(sender);
        newTransaction.setReceiver(receiver);
        newTransaction.setTimeStampo(DateTime.Now);

        sender.setBalance(sender.GetBalance() - newTransaction.Amount);
        receiver.setBalance(receiver.GetBalance() + newTransaction.Amount);

        _transactionRepository.AddAsync(newTransaction);
        _usersService.UpdateUserAsync(sender);
        _usersService.UpdateUserAsync(receiver);
    }

    public bool ValidateTransaction (Users sender, decimal value)
    {
        HttpClient httpClient = new HttpClient();
        var response = httpClient.GetAsync("https://util.devi.tools/api/v2/authorize");

        if (response.IsCompleted)
        {
            var result = JsonSerializer.Deserialize<ValidationTransactionDto>(response.Result.Content.ReadAsStream());
            return result?.Data?.Authorization ?? false;
        }

        return false;
    }
    
}