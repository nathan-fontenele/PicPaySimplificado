namespace PicPaySimplificado.Domain;

public class SellerUserEntity
{
    public Guid Guid { get; private set; }
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string Cnpj { get; private set; }
    public string Password { get; private set; }
    
    private  SellerUserEntity(){}

    public SellerUserEntity(string  fullName, string email, string cnpj, string password)
    {
        if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentNullException("Nome é obrigatório.");
        if(!IsValidCnpj(cnpj))throw new ArgumentException("CNPJ inválido.");
        if(!IsValidEmail(email))throw new ArgumentException("E-mail inválido.");
        Guid = Guid.NewGuid();
        FullName = fullName;
        Email = email;
        Cnpj = cnpj;
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

    private bool IsValidCnpj(string cnpj)
    {
        return cnpj.Length == 14 && cnpj.All(char.IsDigit);
    }
}