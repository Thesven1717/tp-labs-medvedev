using PasswordGen.Core;

namespace PasswordGen.Tests;

public class PasswordStrengthTests
{
    [Fact]
    public void Entropy_FourDigits_IsFourLog2Ten()
    {
        double bits = PasswordStrength.Entropy("1234");

        Assert.Equal(4 * Math.Log2(10), bits, 6);
    }

    [Fact]
    public void Entropy_LongerPassword_HasMoreEntropy()
    {
        Assert.True(PasswordStrength.Entropy("abcdefghij") > PasswordStrength.Entropy("abcdefgh"));
    }

    [Fact]
    public void GetLevel_ShortDigits_IsWeak()
    {
        Assert.Equal(StrengthLevel.Weak, PasswordStrength.GetLevel("1234"));
    }

    [Fact]
    public void GetLevel_LettersAndDigits_IsMedium()
    {
        Assert.Equal(StrengthLevel.Medium, PasswordStrength.GetLevel("abcd1234"));
    }

    [Fact]
    public void GetLevel_LongMixedPassword_IsStrong()
    {
        Assert.Equal(StrengthLevel.Strong, PasswordStrength.GetLevel("Abc123!@#xyz"));
    }
}
