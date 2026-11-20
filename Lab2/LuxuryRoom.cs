namespace Lab2;

// Люкс в цену каждой ночи добавлен завтрак
public class LuxuryRoom : Room
{
    private const decimal BreakfastPrice = 800m;

    public LuxuryRoom(int number, decimal pricePerNight)
        : base(number, pricePerNight) { }

    public override string Type => "Люкс";

    public override decimal CalculateCost(int nights)
    {
        if (nights <= 0)
            throw new ArgumentOutOfRangeException(nameof(nights));

        return (PricePerNight + BreakfastPrice) * nights;
    }

    public override string ToString() => base.ToString() + ", завтрак включён";
}
