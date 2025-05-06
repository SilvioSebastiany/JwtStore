using System.Reflection.Metadata.Ecma335;
using JwtStore.Core.AccountContext.ValueObjects;
using JwtStore.Core.SharedContext.Entities;

namespace JwtStore.Core.AccountContext.Entities;

public class User : Entity
{
    protected User() { }
    public User( Email email, Password? password = null)
    {
        Email = email;
        Password = new Password(password?.Hash ?? string.Empty);
    }

    public string Name {get; private set; } = string.Empty;
    public Email Email { get; private set; }
    public Password Password { get; private set; }
    public string Image { get; private set; } = string.Empty;

    public void UpdatePassword(string plainTextPassword, string code)
    {
        if (!string.Equals(code.Trim(), Password.ResetCode.Trim(), StringComparison.CurrentCultureIgnoreCase))
            throw new Exception("Código de restauração inválido.");

        var newPassword = new Password(plainTextPassword);
        Password = newPassword;   
    }

    public void UpdateEmail(Email email)
    {
        Email = email;
    }

    public void ChangePassword(string plainTextPassword)
    {
        var newPassword = new Password(plainTextPassword);
        Password = newPassword;   
    }

}
