using System.Text;

Console.OutputEncoding = Encoding.UTF8;

while (true)
{
    Console.Write("Введите путь к текстовому файлу: ");
    string inputPath = (Console.ReadLine() ?? "").Trim().Trim('"');
    if (inputPath == "")
        break;

    try
    {
        if (!Path.IsPathRooted(inputPath))
            inputPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop), inputPath);

        if (!File.Exists(inputPath))
        {
            Console.WriteLine("Файл не найден: " + inputPath);
            continue;
        }

        string defaultOut = Path.Combine(
            Path.GetDirectoryName(inputPath)!,
            Path.GetFileNameWithoutExtension(inputPath) + "_freq.txt");

        Console.Write("Путь к файлу результата [Enter = " + defaultOut + "]: ");
        string outputPath = (Console.ReadLine() ?? "").Trim().Trim('"');
        if (outputPath == "")
            outputPath = defaultOut;

        if (File.Exists(outputPath))
        {
            Console.Write("Файл результата уже существует. Перезаписать? (д/н): ");
            string answer = (Console.ReadLine() ?? "").Trim().ToLower();
            if (answer != "д")
            {
                Console.WriteLine("Отменено.");
                continue;
            }
        }

        Dictionary<string, int> freq = WordFrequency.BuildFrequency(inputPath);
        List<KeyValuePair<string, int>> top = WordFrequency.GetTop(freq, 20);
        WordFrequency.SaveResult(outputPath, top);

        Console.WriteLine("Уникальных слов: " + freq.Count);
        Console.WriteLine("Топ-" + top.Count + " сохранён в файл: " + outputPath);
        for (int i = 0; i < top.Count; i++)
            Console.WriteLine($"{i + 1,3}. {top[i].Key,-25} {top[i].Value}");
    }
    catch (FileNotFoundException)
    {
        Console.WriteLine("Ошибка: файл не найден.");
    }
    catch (DirectoryNotFoundException)
    {
        Console.WriteLine("Ошибка: указанная папка не существует.");
    }
    catch (UnauthorizedAccessException)
    {
        Console.WriteLine("Ошибка: нет прав доступа к файлу или папке.");
    }
    catch (ArgumentException)
    {
        Console.WriteLine("Ошибка: некорректный путь.");
    }

    Console.WriteLine();
}

static class WordFrequency
{
    // Читает файл построчно и считает частоту слов
    public static Dictionary<string, int> BuildFrequency(string path)
    {
        var freq = new Dictionary<string, int>();

        using var reader = new StreamReader(path, Encoding.UTF8);
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            string word = "";
            foreach (char c in line.ToLower() + " ")
            {
                if (char.IsLetterOrDigit(c))
                {
                    word += c;
                }
                else if ((c == '-' || c == '\'') && word != "")
                {
                    word += c;
                }
                else
                {
                    word = word.Trim('-', '\'');
                    if (word != "")
                    {
                        if (freq.ContainsKey(word))
                            freq[word]++;
                        else
                            freq[word] = 1;
                    }
                    word = "";
                }
            }
        }

        return freq;
    }

    // Топ-N слов
    public static List<KeyValuePair<string, int>> GetTop(Dictionary<string, int> freq, int count)
    {
        return freq.OrderByDescending(p => p.Value)
                   .ThenBy(p => p.Key)
                   .Take(count)
                   .ToList();
    }

    public static void SaveResult(string path, List<KeyValuePair<string, int>> top)
    {
        using var writer = new StreamWriter(path, false, Encoding.UTF8);
        writer.WriteLine("Топ-" + top.Count + " слов:");
        for (int i = 0; i < top.Count; i++)
            writer.WriteLine($"{i + 1,3}. {top[i].Key,-25} {top[i].Value}");
    }
}