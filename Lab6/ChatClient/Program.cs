using System.Net.Sockets;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

Console.Write("Адрес сервера (Enter = localhost): ");
string host = Console.ReadLine() is { Length: > 0 } h ? h : "localhost";

using var client = new TcpClient();
try
{
    await client.ConnectAsync(host, 5555);
}
catch (SocketException ex)
{
    Console.WriteLine($"Не удалось подключиться к {host}: {ex.Message}");
    return;
}

var stream = client.GetStream();
var reader = new StreamReader(stream);
var writer = new StreamWriter(stream) { AutoFlush = true };

while (true)
{
    Console.Write("Ваш ник: ");
    string? nick = Console.ReadLine();
    if (nick == null) return;

    await writer.WriteLineAsync(nick);

    string? answer = await reader.ReadLineAsync();
    if (answer == null)
    {
        Console.WriteLine("Сервер закрыл соединение.");
        return;
    }

    if (answer == "OK") break;
    string reason = answer.StartsWith("ERR ") ? answer[4..] : answer;
    Console.WriteLine($"{reason}. Введите другой ник.");
}

_ = Task.Run(async () =>
{
    try
    {
        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
            Console.WriteLine(line);
    }
    catch (IOException) { }
    Console.WriteLine("Соединение с сервером потеряно.");
});

Console.WriteLine("Подключено. Пишите сообщения, /exit — выход.");
while (true)
{
    string? msg = Console.ReadLine();
    if (msg == null) break;
    await writer.WriteLineAsync(msg);
    if (msg == "/exit") break;
}