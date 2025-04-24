using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using PicPaySimplificado.ValueObject;

namespace PicPaySimplificado.Domain;

public class Users
{

    public Guid Guid { get;  set; }
    public string Fullname { get;  set; }

    public string Email {  get;  set; }

    public Document Document { get;  set; }

    public string Password { get;  set; }
    
    public  decimal Balance { get;  set; }
    
    public UserType UserType { get;  set; }
    
    public ICollection<Transaction> SentTransactions { get;  set; } = new List<Transaction>();
    public ICollection<Transaction> ReceivedTransactions { get;  set; } = new List<Transaction>();
    
    private  Users(){}

    public Users(string  fullname, string document, decimal balance, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(fullname)) throw new ArgumentNullException("Fullname is required.");
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("Password is required.");
        if(!IsValidEmail(email))throw new ArgumentException("E-mail is invalid.");
        Guid = Guid.NewGuid();
        Fullname = fullname;
        Document = new Document(document);
        Balance = balance;
        Email = email;
        Password = password;
    }
    
    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch (Exception)
        {

            return false;
        }
    }

    public string GetEmail()
    {
        return Email;
    }

    public string GetDocument()
    {
        return Document.ToString();
    }

    public UserType GetUserType()
    {
        return UserType;
    }

    public decimal GetBalance()
    {
        return Balance;
    }

    public void setBalance(decimal getBalance)
    {
        Balance = getBalance;
    }
}