using System;
using System.Collections.Generic;
using Lab4.Core;
using Xunit;

namespace Lab4.Tests;

public class RoomTests
{
    public static IEnumerable<object[]> RoomsAndInvalidNights()
    {
        foreach (int nights in new[] { 0, -1 })
        {
            yield return new object[] { new StandardRoom(101, 3000m), nights };
            yield return new object[] { new LuxuryRoom(201, 9000m), nights };
            yield return new object[] { new HostelRoom(1, 700m, 2), nights };
        }
    }


    [Fact]
    public void Constructor_ValidArguments_StoresNumberPriceAndType()
    {
        // Arrange
        const int number = 101;
        const decimal price = 3000m;

        var room = new StandardRoom(number, price);

        Assert.Equal(number, room.Number);
        Assert.Equal(price, room.PricePerNight);
        Assert.Equal("Стандарт", room.Type);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_NonPositiveNumber_Throws(int number)
    {
        // Arrange
        Action act = () => new StandardRoom(number, 3000m);

        Assert.Throws<ArgumentOutOfRangeException>(act);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Constructor_NonPositivePrice_Throws(int price)
    {
        // Arrange
        Action act = () => new LuxuryRoom(201, price);

        Assert.Throws<ArgumentOutOfRangeException>(act);
    }

    [Fact]
    public void Book_FreeRoom_MakesRoomUnavailable()
    {
        var room = new StandardRoom(101, 3000m);

        room.Book("Иванов");

        Assert.False(room.IsAvailable);
    }

    [Fact]
    public void Book_AlreadyBookedRoom_ThrowsAndKeepsRoomBooked()
    {
        var room = new LuxuryRoom(201, 9000m);
        room.Book("Иванов");
        Action act = () => room.Book("Петров");

        Assert.Throws<InvalidOperationException>(act);
        Assert.False(room.IsAvailable);
    }

  

    [Fact]
    public void Release_BookedRoom_MakesRoomAvailableAgain()
    {
        var room = new StandardRoom(101, 3000m);
        room.Book("Иванов");

        room.Release();

        Assert.True(room.IsAvailable);
    }

    [Fact]
    public void Release_FreeRoom_Throws()
    {
        // Arrange
        var room = new StandardRoom(101, 3000m);
        Action act = () => room.Release();

        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void ToString_FreeStandardRoom_ShowsTypeNumberPriceAndStatus()
    {
        var room = new StandardRoom(101, 3000m);

        string text = room.ToString();

        Assert.Equal("Стандарт №101, 3000 руб./ночь, свободен", text);
    }

    [Fact]
    public void ToString_BookedStandardRoom_ShowsGuestName()
    {
        var room = new StandardRoom(101, 3000m);
        room.Book("Иванов");

        string text = room.ToString();

        Assert.Equal("Стандарт №101, 3000 руб./ночь, занят (Иванов)", text);
    }
}
