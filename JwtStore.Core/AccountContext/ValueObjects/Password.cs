using System.Security.Cryptography;
using JwtStore.Core.SharedContext.ValueObjects;

namespace JwtStore.Core.AccountContext.ValueObjects;

public class Password : ValueObject
{
    protected Password(){}
    public Password(string? text = null)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            text = Generate();

        Hash = Hashing(text);
    }
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

    // saltSize: Tamanho do salt em bytes (16 bytes = 128 bits)
    // keySize: Tamanho da chave em bytes (32 bytes = 256 bits)
    // iterations: Número de iterações para o algoritmo PBKDF2 (10000 é um valor comum)
    // splitChar: Caractere usado para separar os componentes do hash (padrão é '.')
    private static string Hashing(
        string password,
        short saltSize = 16,
        short keySize = 32,
        int iterations = 10000,
        char splitChar = '.')
    {
        if (string.IsNullOrEmpty(password))
            throw new Exception("A senha não pode ser vazia.");
        
        password += Configuration.Secrets.PasswordSaltKey;

        // Rfc2898DeriveBytes: Implementa o algoritmo PBKDF2 para derivar uma chave a partir de uma senha.
        // O salt é gerado aleatoriamente e armazenado junto com o hash.
        using var algorithm = new Rfc2898DeriveBytes(
            password,
            saltSize,
            iterations,
            HashAlgorithmName.SHA256);


        var key = Convert.ToBase64String(algorithm.GetBytes(keySize));
        var salt = Convert.ToBase64String(algorithm.Salt);

        return $"{iterations}{splitChar}{salt}{splitChar}{key}";
    }

    // Verify: Verifica se a senha fornecida corresponde ao hash armazenado.
    // O hash é dividido em partes: número de iterações, salt e chave.
    // A senha é derivada novamente usando o mesmo algoritmo e parâmetros, e a chave resultante é comparada com a chave armazenada.
    // Se todas as partes corresponderem, a senha é considerada válida.
    private static bool Verify(
        string password,
        string hash,
        short keySize = 32,
        int iterations = 10000,
        char splitChar = '.')
    {
       password += Configuration.Secrets.PasswordSaltKey;

        var parts = hash.Split(splitChar, 3);
        if (parts.Length != 3)
            return false;

        var  hashIterations = Convert.ToInt32(parts[0]);
        var salt = Convert.FromBase64String(parts[1]);
        var key = Convert.FromBase64String(parts[2]);

        if (hashIterations != iterations)
            return false;
        
        using var algorithm = new Rfc2898DeriveBytes(
            password,
            salt,
            hashIterations,
            HashAlgorithmName.SHA256);

        var ketToCheck = algorithm.GetBytes(keySize);

        return ketToCheck.SequenceEqual(key);
    }

}