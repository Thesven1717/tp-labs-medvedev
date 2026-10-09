namespace Lab2;

// Стандартный номер скидка 10% за7 ночей
public class StandardRoom : Room
{
    public StandardRoom(int number, decimal pricePerNight)
        : base(number, pricePerNight) { }

    public override string Type => "Стандарт";

    public override decimal CalculateCost(int nights)
    {
        if (nights <= 0)
            throw new ArgumentOutOfRangeException(nameof(nights));

        decimal total = PricePerNight * nights;
        if (nights >= 7)
            total = total * 0.9m;
        return total;
    }
}
