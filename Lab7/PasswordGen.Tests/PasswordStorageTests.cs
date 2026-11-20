using PasswordGen.Core;

namespace PasswordGen.Tests;

public class PasswordStorageTests
{
    [Fact]
    public void MemoryStorage_ReturnsSavedPasswords()
    {
        var storage = new MemoryPasswordStorage();

        storage.Save("first");
        storage.Save("second");

        Assert.Equal(new List<string> { "first", "second" }, storage.GetAll());
    }

    [Fact]
    public void FileStorage_ReturnsSavedPasswords()
    {
        string path = Path.GetTempFileName();
        var storage = new FilePasswordStorage(path);

        storage.Save("first");
        storage.Save("second");
        var all = storage.GetAll();
        File.Delete(path);

        Assert.Equal(new List<string> { "first", "second" }, all);
    }

    [Fact]
    public void FileStorage_NoFile_ReturnsEmptyList()
    {
        var storage = new FilePasswordStorage("no_such_file_12345.txt");

        Assert.Empty(storage.GetAll());
    }
}
