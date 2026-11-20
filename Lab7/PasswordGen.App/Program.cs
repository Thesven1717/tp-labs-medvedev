using ConsoleTables;
using PasswordGen.Core;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var storage = new FilePasswordStorage("passwords.txt");
var generator = new PasswordGenerator(storage);

while (true)
{
    Console.WriteLine("\n1-сгенерировать 2-оценить пароль 3-история 0-выход");
    switch (Console.ReadLine())
    {
        case "1":
            Console.Write("Длина (1-128): ");
            int length = int.Parse(Console.ReadLine()!);
            Console.Write("Строчные буквы (да/нет): ");
            bool lower = Console.ReadLine() == "да";
            Console.Write("Заглавные буквы (да/нет): ");
            bool upper = Console.ReadLine() == "да";
            Console.Write("Цифры (да/нет): ");
            bool digits = Console.ReadLine() == "да";
            Console.Write("Спецсимволы (да/нет): ");
            bool symbols = Console.ReadLine() == "да";

            try
            {
                string password = generator.Generate(length, lower, upper, digits, symbols);
                Console.WriteLine($"Пароль: {password}");
                Console.WriteLine($"Стойкость: {PasswordStrength.GetLevel(password)}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            break;

        case "2":
            Console.Write("Пароль: ");
            string text = Console.ReadLine()!;
            try
            {
                Console.WriteLine($"Энтропия: {PasswordStrength.Entropy(text):F1} бит");
                Console.WriteLine($"Стойкость: {PasswordStrength.GetLevel(text)}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            break;

        case "3":
            var table = new ConsoleTable("№", "Пароль", "Стойкость");
            var all = storage.GetAll();
            for (int i = 0; i < all.Count; i++)
                table.AddRow(i + 1, all[i], PasswordStrength.GetLevel(all[i]));
            table.Write();
            break;

        case "0":
            return;
    }
}
