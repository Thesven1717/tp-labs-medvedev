using System.Net;
using System.Net.Sockets;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

const int Port = 5555;

var clients = new Dictionary<string, ClientInfo>(StringComparer.OrdinalIgnoreCase);
var lockObj = new object();

var listener = new TcpListener(IPAddress.Any, Port);
listener.Start();
Console.WriteLine($"Сервер запущен на порту {Port}. Ctrl+C — остановка.");

while (true)
{
    TcpClient client = await listener.AcceptTcpClientAsync();
    _ = HandleClientAsync(client);
}

async Task HandleClientAsync(TcpClient client)
{
    var endpoint = client.Client.RemoteEndPoint;
    string? nick = null;
    ClientInfo? me = null;
    bool announced = false;

    try
    {
        var stream = client.GetStream();
        var reader = new StreamReader(stream);
        var writer = new StreamWriter(stream) { AutoFlush = true };
        me = new ClientInfo(writer);

        while (nick == null)
        {
            string? candidate = await reader.ReadLineAsync();
            if (candidate == null) return;
            candidate = candidate.Trim();

            if (candidate.Length == 0)
            {
                await me.SendAsync("ERR Ник не может быть пустым");
                continue;
            }

            bool added;
            lock (lockObj)
            {
                if (clients.ContainsKey(candidate))
                    added = false;
                else
                {
                    clients[candidate] = me;
                    added = true;
                }
            }

            if (!added)
            {
                await me.SendAsync($"ERR Ник «{candidate}» уже занят");
                continue;
            }

            nick = candidate;
        }

        await me.SendAsync("OK");
        me.Ready = true;
        announced = true;

        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {nick} подключился ({endpoint})");
        await BroadcastAsync($"{nick} вошёл в чат");

        string? line;
        while ((line = await reader.ReadLineAsync()) != null) //чтьение строки от клиента
        {
            if (line == "/exit") break;
            await BroadcastAsync($"[{DateTime.Now:HH:mm:ss}] {nick}: {line}");//рассылаем всем
        }
    }
    catch (IOException) { }
    catch (ObjectDisposedException) { }
    finally
    {
        if (nick != null)
        {
            lock (lockObj) clients.Remove(nick);
        }
        client.Close();

        if (announced)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {nick} отключился");
            await BroadcastAsync($"*** {nick} покинул чат ***");
        }
    }
}

async Task BroadcastAsync(string message) // сервер пишет каждому нику сообщение(одно и тоже)
{
    List<ClientInfo> snapshot;
    lock (lockObj) snapshot = clients.Values.Where(c => c.Ready).ToList();

    foreach (var c in snapshot)
    {
        try { await c.SendAsync(message); }
        catch (Exception ex) when (ex is IOException or ObjectDisposedException) { }
    }
}

class ClientInfo
{
    private readonly StreamWriter _writer;
    private readonly SemaphoreSlim _gate = new(1, 1);
    public volatile bool Ready;

    public ClientInfo(StreamWriter writer) => _writer = writer;

    public async Task SendAsync(string message)
    {
        await _gate.WaitAsync();
        try { await _writer.WriteLineAsync(message); }
        finally { _gate.Release(); }
    }
}