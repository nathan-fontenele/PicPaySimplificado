using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PicPaySimplificado.Domain;

[Table("Transactions")]
public class TransactionEntity
{
    private Guid Id { get; set; } 
    private decimal Amount { get; set; }
    private Users Sender { get; set; }
    private Users Receiver { get; set; }
    private DateTime Created { get; set; } =  DateTime.Now;

    private TransactionEntity()
    {
        
    }

    public TransactionEntity( decimal  amount, Users sender, Users receiver)
    {
        if (amount <= 0) throw new ArgumentException("Transaction value is invalid");
        
        Amount = amount;
        Sender = sender;
        Receiver = receiver;
        Created = DateTime.Now;
    }
}