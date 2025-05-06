namespace JwtStore.Core.AccountContext.ValueObjects;
using JwtStore.Core.SharedContext.ValueObjects;
using JwtStore.Core.SharedContext.Extensions;
using System.Text.RegularExpressions;

public partial class Email : ValueObject
{
    private const string Pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    public Email(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new Exception("E-mail inválio.");
        
        //ToLowerInvariant: converte a string para minúsculas, independentemente da cultura
        Address = address.Trim().ToLowerInvariant();

        if (Address.Length > 5)
            throw new Exception("E-mail inválio.");

        // IsMatch: verifica se a string corresponde ao padrão regex
        if (!EmailRegex().IsMatch(address))
            throw new Exception("E-mail inválio.");

        Address = address;
    }

    public string Address { get;}
    public string Hash => Address.ToBase64();
    public Verification Verification { get; private set; } = new();

    public void ResendVerification()
    {
        Verification = new Verification();
    }

    public static implicit operator string(Email email)  => email.ToString(); 

    public static implicit operator Email(string email) => new(email);

    public override string ToString() => Address.Trim().ToLower();

    [GeneratedRegex(Pattern)]
    private static partial Regex EmailRegex();
}
