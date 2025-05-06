using JwtStore.Core.SharedContext.ValueObjects;

namespace JwtStore.Core.AccountContext.ValueObjects;

public class Password : ValueObject
{
    private const string Valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+[]{}|;:,.<>?";
    private const string Special = "!@#$%^&*()_+[]{}|;:,.<>?";

    public string Hash { get; } = string.Empty;
    public string ResetCode { get; } = Guid.NewGuid().ToString("N").ToUpper()[0..8];


    private static string Generate(
        short length = 16,
        bool includeSpecialChars = true,
        bool uppercase = false)
    {
        var chars = includeSpecialChars ? (Valid + Special) : Valid;
        var startRandom = uppercase ? 26 : 0;
        var index = 0;
        var res = new char[length];
        var rnd = new Random();

        while (index < length)
            res[index++] = chars[rnd.Next(startRandom, chars.Length)];

        return new string(res);
    }

}