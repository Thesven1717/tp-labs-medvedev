namespace PasswordGen.Core;

public interface IPasswordStorage
{
    void Save(string password);
    List<string> GetAll();
}

// хранение в файле
public class FilePasswordStorage : IPasswordStorage
{
    private readonly string _path;

    public FilePasswordStorage(string path) => _path = path;

    public void Save(string password) => File.AppendAllText(_path, password + Environment.NewLine);

    public List<string> GetAll()
    {
        if (!File.Exists(_path))
            return new List<string>();
        return File.ReadAllLines(_path).ToList();
    }
}

// хранение в памяти 
public class MemoryPasswordStorage : IPasswordStorage
{
    private readonly List<string> _items = new();

    public void Save(string password) => _items.Add(password);

    public List<string> GetAll() => _items;
}
