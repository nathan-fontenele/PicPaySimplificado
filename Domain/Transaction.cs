using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PicPaySimplificado.Domain;

[Table("Transactions")]

public class Transaction
{
    public Guid Id { get; set; } 
    public decimal Amount { get; set; }
    
    public Guid SenderId { get;  set; }
    public Users Sender { get; set; }
    
    public Guid ReceiverId { get;  set; }
    public Users Receiver { get; set; }
    public DateTime Created { get; set; } =  DateTime.Now;

    public Transaction()
    {
        
    }

    public Transaction( decimal  amount, Guid senderId, Users sender, Guid receiverId, Users receiver)
    {
        if (amount <= 0) throw new ArgumentException("Transaction value is invalid");
        
        Amount = amount;
        SenderId = senderId;
        Sender = sender;
        ReceiverId = receiverId;
        Receiver = receiver;
        Created = DateTime.Now;
    }


    public void setAmount(decimal transactionValue)
    {
        Amount = transactionValue;
    }

    public void setSender(Users sender)
    {
        Sender = sender;
    }

    public void setReceiver(Users receiver)
    {
        Receiver = receiver;
    }

    public void setTimeStampo(DateTime now)
    {
        Created = now;
    }
}