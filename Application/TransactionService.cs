using PicPaySimplificado.Infrastructure;

namespace PicPaySimplificado.Application;

public class TransactionService
{
    private UsersService _usersService;
    private TransactionRepository _transectionRepository;
    
    public void CreateTransaction(TransactionDTO transaction)
    {
        User sender = this._usersService.FindUserByIdAsync(transaction.User.Id);
    }
    
}