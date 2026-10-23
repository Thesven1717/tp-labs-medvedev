using System;
using System.Numerics;
using Lab4.Core;
using Xunit;

namespace Lab4.Tests;

public class MathAlgorithmsTests
{
    // Factorial

    [Fact]
    public void Factorial_OfZero_ReturnsOne()
    {
        const int n = 0;

        BigInteger result = MathAlgorithms.Factorial(n);

        Assert.Equal(BigInteger.One, result);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(int.MinValue)]
    public void Factorial_NegativeArgument_Throws(int n)
    {
        Action act = () => MathAlgorithms.Factorial(n);

        Assert.Throws<ArgumentOutOfRangeException>(act);
    }

    //Fibonachi

    [Fact]
    public void Fibonachi_UpToFive_ReturnsFirstSixNumbers()
    {
        long[] expected = { 0, 1, 1, 2, 3, 5 };

        long[] actual = MathAlgorithms.Fibonachi(5);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(MathAlgorithms.MaxFibonachiIndex + 1)]
    [InlineData(1000)]
    public void Fibonachi_IndexOutOfRange_Throws(int n)
    {
        Action act = () => MathAlgorithms.Fibonachi(n);

        Assert.Throws<ArgumentOutOfRangeException>(act);
    }


    //Taylorsin

    [Theory]
    [InlineData(0.5)]
    [InlineData(1.0)]
    [InlineData(Math.PI / 2)]
    [InlineData(-1.2)]
    [InlineData(Math.PI)]
    [InlineData(5.0)]
    [InlineData(-10.0)]
    public void Taylorsin_ReasonableArgument_MatchesMathSin(double x)
    {
        double expected = Math.Sin(x);

        var (sum, _) = MathAlgorithms.Taylorsin(x, 1e-6);

        Assert.Equal(expected, sum, 1e-5);
    }

    [Fact]
    public void Taylorsin_ZeroArgument_ReturnsZeroSumAndNoTerms()
    {
        const double x = 0.0;

        var (sum, count) = MathAlgorithms.Taylorsin(x, 1e-6);

        Assert.Equal(0.0, sum);
        Assert.Equal(0, count);
    }

    [Fact]
    public void Taylorsin_OneWithEpsMillionth_SumsFiveTerms()
    {
        const double x = 1.0;

        var (_, count) = MathAlgorithms.Taylorsin(x, 1e-6);

        Assert.Equal(5, count);
    }

    [Fact]
    public void Taylorsin_SmallerEps_UsesMoreTermsAndIsMoreAccurate()
    {
        const double x = 1.0;
        double expected = Math.Sin(x);

        var coarse = MathAlgorithms.Taylorsin(x, 1e-2);
        var fine = MathAlgorithms.Taylorsin(x, 1e-10);

        Assert.True(fine.count > coarse.count);
        Assert.True(Math.Abs(fine.sum - expected) < Math.Abs(coarse.sum - expected));
        Assert.Equal(expected, fine.sum, 1e-9);
    }

    [Fact]
    public void Taylorsin_NegatedArgument_ReturnsNegatedSumAndSameCount()
    {
        const double x = 0.7;

        var positive = MathAlgorithms.Taylorsin(x, 1e-6);
        var negative = MathAlgorithms.Taylorsin(-x, 1e-6);

        Assert.Equal(-positive.sum, negative.sum, 1e-12);
        Assert.Equal(positive.count, negative.count);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(-1e-3)]
    [InlineData(double.NaN)]
    public void Taylorsin_NonPositiveEps_Throws(double eps)
    {
        // Arrange
        Action act = () => MathAlgorithms.Taylorsin(1.0, eps);

        Assert.Throws<ArgumentOutOfRangeException>(act);
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void Taylorsin_NonFiniteArgument_Throws(double x)
    {
        // Arrange
        Action act = () => MathAlgorithms.Taylorsin(x, 1e-6);

        Assert.Throws<ArgumentOutOfRangeException>(act);
    }
}
