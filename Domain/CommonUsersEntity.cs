namespace PicPaySimplificado.Domain
{
    public class CommonUsersEntity
    {
        public Guid Guid { get; private set; }
        public string FullName { get; private set; }
        public string Cpf { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }

        private CommonUsersEntity() { }

        public CommonUsersEntity(string fullname, string cpf, string email, string password)
        {
            if(string.IsNullOrWhiteSpace(fullname))throw new ArgumentNullException("Nome completo é obrigatório");

            if(!IsValidCpf(cpf))throw new ArgumentException("CPF inválido");

            if(!IsValidEmail(email))throw new ArgumentException("E-mail inválido");

            if(string.IsNullOrWhiteSpace(password))throw new ArgumentNullException("Senha é obrigatória");

            Guid = new Guid();
            FullName = fullname.Trim();
            Cpf = cpf.Trim();
            Email = email.Trim();
            Password = password.Trim();
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

        private bool IsValidCpf(string cpf)
        {
            return cpf.Length == 11 && cpf.All(char.IsDigit);
        }
    }
}
