namespace Lab2;

// Хостел цена за одну койку
public class HostelRoom : Room
{
    private readonly int _beds;   // всего коек
    private int _occupied;        // сколько занято

    public HostelRoom(int number, decimal pricePerBed, int beds)
        : base(number, pricePerBed)
    {
        if (beds <= 0)
            throw new ArgumentOutOfRangeException(nameof(beds));
        _beds = beds;
    }

    public override string Type => "Хостел";

    public override decimal CalculateCost(int nights)
    {
        if (nights <= 0)
            throw new ArgumentOutOfRangeException(nameof(nights));

        return PricePerNight * nights;   // за одну койку
    }

    // Комната свободна, пока остались свободные койки
    public override bool IsAvailable => _occupied < _beds;

    public override void Book(string guestName)
    {
        CheckGuestName(guestName);
        if (!IsAvailable)
            throw new InvalidOperationException($"В {Type} №{Number} нет свободных коек.");

        _occupied++;
    }

    public override void Release()
    {
        if (_occupied == 0)
            throw new InvalidOperationException($"В {Type} №{Number} нет занятых коек.");

        _occupied--;
    }

    public override string ToString()
        => $"{Type} №{Number}, {PricePerNight:F0} руб./койка/ночь, занято коек: {_occupied} из {_beds}";
}
