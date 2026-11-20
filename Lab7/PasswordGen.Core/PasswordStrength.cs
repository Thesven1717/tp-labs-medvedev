namespace PasswordGen.Core;

public enum StrengthLevel
{
    Weak,
    Medium,
    Strong
}

public static class PasswordStrength
{
    public static double Entropy(string password)  // Энтропия
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Пароль пустой", nameof(password));

        int alphabetSize = 0;
        if (password.Any(c => c >= 'a' && c <= 'z')) alphabetSize += 26;
        if (password.Any(c => c >= 'A' && c <= 'Z')) alphabetSize += 26;
        if (password.Any(c => c >= '0' && c <= '9')) alphabetSize += 10;
        if (password.Any(c => !(c >= 'a' && c <= 'z') && !(c >= 'A' && c <= 'Z') && !(c >= '0' && c <= '9')))
            alphabetSize += 32;

        return password.Length * Math.Log2(alphabetSize);
    }

    public static StrengthLevel GetLevel(string password)
    {
        double bits = Entropy(password);
        if (bits < 40) return StrengthLevel.Weak;
        if (bits < 70) return StrengthLevel.Medium;
        return StrengthLevel.Strong;
    }
}
