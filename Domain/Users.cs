using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using PicPaySimplificado.ValueObject;

namespace PicPaySimplificado.Domain;

public class Users
{

    private Guid _guid { get;  set; }
    private string _fullname { get;  set; }

    private string _email {  get;  set; }

    private Document _document { get;  set; }

    private string _password { get;  set; }
    
    private  decimal _balance { get;  set; }
    
    private UserType _userType { get;  set; }
    
    public ICollection<Transaction> SentTransactions { get;  set; } = new List<Transaction>();
    public ICollection<Transaction> ReceivedTransactions { get;  set; } = new List<Transaction>();
    
    private  Users(){}

    public Users(string  fullname, string email, string document, string password)
    {
        if (string.IsNullOrWhiteSpace(fullname)) throw new ArgumentNullException("Fullname is required.");
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("Password is required.");
        if(!IsValidEmail(email))throw new ArgumentException("E-mail is invalid.");
        _guid = Guid.NewGuid();
        _fullname = fullname;
        _email = email;
        _document = new Document(document);
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