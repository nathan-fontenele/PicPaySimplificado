using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using PicPaySimplificado.ValueObject;

namespace PicPaySimplificado.Domain;

public class Users
{

    public Guid _guid { get;  set; }
    public string _fullname { get;  set; }

    public string _email {  get;  set; }

    public Document _document { get;  set; }

    public string _password { get;  set; }
    
    public  decimal _balance { get;  set; }
    
    public UserType _userType { get;  set; }
    
    public ICollection<Transaction> SentTransactions { get;  set; } = new List<Transaction>();
    public ICollection<Transaction> ReceivedTransactions { get;  set; } = new List<Transaction>();
    
    private  Users(){}

    public Users(string  fullname, string document, decimal balance, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(fullname)) throw new ArgumentNullException("Fullname is required.");
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("Password is required.");
        if(!IsValidEmail(email))throw new ArgumentException("E-mail is invalid.");
        _guid = Guid.NewGuid();
        _fullname = fullname;
        _document = new Document(document);
        _balance = balance;
        _email = email;
        _password = password;
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
        return _email;
    }

    public string GetDocument()
    {
        return _document.ToString();
    }

    public UserType GetUserType()
    {
        return _userType;
    }

    public decimal GetBalance()
    {
        return _balance;
    }

    public void setBalance(decimal getBalance)
    {
        _balance = getBalance;
    }
}