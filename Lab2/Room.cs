namespace Lab2;

// Базовый абстрактный класс
public abstract class Room
{
    private bool _isBooked;
    private string _guestName = "";

    public int Number { get; }
    public decimal PricePerNight { get; }

    protected Room(int number, decimal pricePerNight)
    {
        if (number <= 0)
            throw new ArgumentOutOfRangeException(nameof(number));
        if (pricePerNight <= 0)
            throw new ArgumentOutOfRangeException(nameof(pricePerNight));

        Number = number;
        PricePerNight = pricePerNight;
    }

    // Название типа номера
    public abstract string Type { get; }

    public abstract decimal CalculateCost(int nights);

    // Свободен ли номер
    public virtual bool IsAvailable => !_isBooked;

    public virtual void Book(string guestName)
    {
        CheckGuestName(guestName);
        if (_isBooked)
            throw new InvalidOperationException($"{Type} №{Number} уже занят.");

        _isBooked = true;
        _guestName = guestName;
    }

    public virtual void Release()
    {
        if (!_isBooked)
            throw new InvalidOperationException($"{Type} №{Number} и так свободен.");

        _isBooked = false;
        _guestName = "";
    }

    protected static void CheckGuestName(string guestName)
    {
        if (string.IsNullOrWhiteSpace(guestName))
            throw new ArgumentException("Имя гостя не может быть пустым.", nameof(guestName));
    }

    public override string ToString()
    {
        string status = _isBooked ? $"занят ({_guestName})" : "свободен";
        return $"{Type} №{Number}, {PricePerNight:F0} руб./ночь, {status}";
    }
}
