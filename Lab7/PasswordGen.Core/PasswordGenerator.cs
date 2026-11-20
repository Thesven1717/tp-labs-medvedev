using System.Security.Cryptography;

namespace PasswordGen.Core;

public class PasswordGenerator
{
    public const string Lower = "abcdefghijklmnopqrstuvwxyz";
    public const string Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public const string Digits = "0123456789";
    public const string Symbols = "!@#$%^&*()-_=+";

    private readonly IPasswordStorage _storage;

    public PasswordGenerator(IPasswordStorage storage) => _storage = storage;

    public string Generate(int length, bool useLower, bool useUpper, bool useDigits, bool useSymbols)
    {
        if (length < 1 || length > 128)
            throw new ArgumentOutOfRangeException(nameof(length), "Длина должна быть от 1 до 128");

        string alphabet = "";
        if (useLower) alphabet += Lower;
        if (useUpper) alphabet += Upper;
        if (useDigits) alphabet += Digits;
        if (useSymbols) alphabet += Symbols;

        if (alphabet == "")
            throw new ArgumentException("Не выбран ни один набор символов");

        char[] chars = new char[length];
        for (int i = 0; i < length; i++)
            chars[i] = alphabet[RandomNumberGenerator.GetInt32(alphabet.Length)];

        string password = new string(chars);
        _storage.Save(password);
        return password;
    }
}
