using PasswordGen.Core;

namespace PasswordGen.Tests;

public class PasswordGeneratorTests
{
    [Fact]
    public void Generate_OnlyLower_ContainsOnlyLowerLetters()
    {
        var generator = new PasswordGenerator(new MemoryPasswordStorage());

        string password = generator.Generate(30, true, false, false, false);

        Assert.True(password.All(c => PasswordGenerator.Lower.Contains(c)));
    }

    [Fact]
    public void Generate_SavesPasswordToStorage()
    {
        var storage = new MemoryPasswordStorage();
        var generator = new PasswordGenerator(storage);

        string password = generator.Generate(10, true, true, true, false);

        Assert.Single(storage.GetAll());
        Assert.Equal(password, storage.GetAll()[0]);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(129)]
    public void Generate_WrongLength_ThrowsException(int length)
    {
        var generator = new PasswordGenerator(new MemoryPasswordStorage());

        Assert.Throws<ArgumentOutOfRangeException>(() => generator.Generate(length, true, true, true, true));
    }
}
