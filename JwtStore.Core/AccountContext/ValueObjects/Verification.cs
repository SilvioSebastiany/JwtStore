using JwtStore.Core.SharedContext.ValueObjects;

namespace JwtStore.Core.AccountContext.ValueObjects;

public class Verification : ValueObject
{
    public Verification() { }
    // ToString("N"): Converte o Guid para string sem hifens.
    // [0..6]: Pega os primeiros 6 caracteres da string.
    public string Code { get; } = Guid.NewGuid().ToString("N")[0..6].ToUpper();

    // UtcNow: Pega a data e hora atual em UTC.
    public DateTime? ExpirationAt { get; private set; } = DateTime.UtcNow.AddMinutes(5);

    public DateTime? VerifiedAt { get; private set; } = null;

    public bool IsActive => VerifiedAt != null && ExpirationAt.HasValue && ExpirationAt > DateTime.UtcNow;

    public void Verify(string code)
    {
        if (IsActive)
            throw new Exception("Este item já foi ativado.");

        if (ExpirationAt < DateTime.UtcNow)
            throw new Exception("Este código já expirou.");

        if (!string.Equals(code.Trim(), Code.Trim(), StringComparison.CurrentCultureIgnoreCase))
            throw new Exception("Código de verificação inválido.");

        ExpirationAt = null;
        VerifiedAt = DateTime.UtcNow;
            
    }
}