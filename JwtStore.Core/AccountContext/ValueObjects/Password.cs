using JwtStore.Core.SharedContext.ValueObjects;

namespace JwtStore.Core.AccountContext.ValueObjects;

public class Password : ValueObject
{
    private const string Valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+[]{}|;:,.<>?";
    private const string Special = "!@#$%^&*()_+[]{}|;:,.<>?";

    public string Hash { get; } = string.Empty;
    public string ResetCode { get; } = Guid.NewGuid().ToString("N").ToUpper()[0..8];
}