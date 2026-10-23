using System;
using Lab4.Core;
using Xunit;

namespace Lab4.Tests;

public class RoomCostTests
{
    //StandardRoom

    [Theory]
    [InlineData(1, 3000)]
    [InlineData(6, 18000)]
    [InlineData(7, 18900)]
    [InlineData(10, 27000)]
    public void Standard_CalculateCost_AppliesDiscountFromSevenNights(int nights, int expectedCost)
    {
        var room = new StandardRoom(101, 3000m);
        decimal expected = expectedCost;

        decimal actual = room.CalculateCost(nights);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Standard_SevenNights_CheaperThanSixNightsPlusOneNight()
    {
        var room = new StandardRoom(101, 3000m);

        decimal seven = room.CalculateCost(7);
        decimal sixPlusOne = room.CalculateCost(6) + room.CalculateCost(1);

        Assert.True(seven < sixPlusOne);
    }

    //LuxuryRoom

    [Theory]
    [InlineData(1, 9800)]
    [InlineData(3, 29400)]
    [InlineData(7, 68600)]
    public void Luxury_CalculateCost_AddsBreakfastToEveryNight(int nights, int expectedCost)
    {
        var room = new LuxuryRoom(201, 9000m);
        decimal expected = expectedCost;

        decimal actual = room.CalculateCost(nights);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Luxury_ToString_MentionsBreakfast()
    {
        var room = new LuxuryRoom(201, 9000m);

        string text = room.ToString();

        Assert.Equal("Люкс №201, 9000 руб./ночь, свободен, завтрак включён", text);
    }

    //HostelRoom

    [Theory]
    [InlineData(1, 700)]
    [InlineData(3, 2100)]
    [InlineData(10, 7000)]
    public void Hostel_CalculateCost_IsPricePerBedTimesNights(int nights, int expectedCost)
    {
        // Arrange
        var room = new HostelRoom(1, 700m, 2);
        decimal expected = expectedCost;

        // Act
        decimal actual = room.CalculateCost(nights);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Hostel_CalculateCost_DoesNotDependOnBedCount()
    {
        // Arrange
        var small = new HostelRoom(1, 700m, 2);
        var large = new HostelRoom(2, 700m, 10);

        // Act
        decimal costSmall = small.CalculateCost(4);
        decimal costLarge = large.CalculateCost(4);

        // Assert
        Assert.Equal(costSmall, costLarge);
    }

}
