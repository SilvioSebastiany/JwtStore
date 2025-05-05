namespace JwtStore.Core.AccountContext.ValueObjects;
using JwtStore.Core.SharedContext.ValueObjects;
using JwtStore.Core.SharedContext.Extensions;

public class Email : ValueObject
{
    private const string Pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    public string Address { get;}
    public string Hash  => Address.ToBase64();
}
