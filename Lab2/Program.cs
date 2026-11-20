using Lab2;

Console.OutputEncoding = System.Text.Encoding.UTF8;

List<Room> rooms =
[
    new StandardRoom(101, 3000m),
    new StandardRoom(102, 3500m),
    new LuxuryRoom(201, 9000m),
    new LuxuryRoom(202, 12000m),
    new HostelRoom(1, 700m, 2),
    new HostelRoom(2, 900m, 4),
];

Console.WriteLine("Все номера");
foreach (Room room in rooms)
    Console.WriteLine(room);

Console.WriteLine("\nСтоимость за 3 ночи");
foreach (Room room in rooms)
    Console.WriteLine($"{room.Type} №{room.Number}: {room.CalculateCost(3):F0} руб.");

Console.WriteLine("\nСкидка в стандарте от 7 ночей");
Console.WriteLine($"3 ночи: {rooms[0].CalculateCost(3):F0} руб.");
Console.WriteLine($"7 ночей: {rooms[0].CalculateCost(7):F0} руб.");

Console.WriteLine("\nБронирование");
rooms[0].Book("Иванов");
rooms[2].Book("Петров");
rooms[4].Book("Сидоров");
rooms[4].Book("Кузнецов");
foreach (Room room in rooms)
    Console.WriteLine(room);

Console.WriteLine("\nОшибки");
TryRun("Занять занятый номер", () => rooms[0].Book("Другой"));
TryRun("0 ночей", () => rooms[0].CalculateCost(0));
TryRun("Отрицательная цена", () => new StandardRoom(300, -5m));

Console.WriteLine("\nОсвобождение");
rooms[0].Release();
rooms[4].Release();
foreach (Room room in rooms)
    Console.WriteLine(room);

Console.WriteLine("\nСвободные номера");
foreach (Room room in rooms)
{
    if (room.IsAvailable)
        Console.WriteLine($"{room.Type} №{room.Number}");
}

static void TryRun(string title, Action action)
{
    try
    {
        action();
        Console.WriteLine($"{title}: ошибки не было (!)");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"{title}: {ex.GetType().Name}");
    }
}
