using System;
using System.Numerics;

namespace Lab4.Core;

/// <summary>Алгоритмы из лабораторной работы 1 (вариант 11), вынесенные из консольного меню.</summary>
public static class MathAlgorithms
{
    /// <summary>Максимальный n, для которого F(n) помещается в long.</summary>
    public const int MaxFibonachiIndex = 92;

    public static BigInteger Factorial(int n)
    {
        if (n < 0) throw new ArgumentOutOfRangeException(nameof(n), "n должно быть >= 0");
        BigInteger result = BigInteger.One;
        for (int i = 2; i <= n; i++)
            result = result * i;
        return result;
    }

    /// <summary>Числа Фибоначчи от F(0) до F(n) включительно (n + 1 элемент).</summary>
    public static long[] Fibonachi(int n)
    {
        if (n < 0) throw new ArgumentOutOfRangeException(nameof(n), "n должно быть >= 0");
        if (n > MaxFibonachiIndex) throw new ArgumentOutOfRangeException(nameof(n), "переполнение long");
        long[] arr = new long[n + 1];
        if (n >= 0) arr[0] = 0;
        if (n >= 1) arr[1] = 1;
        for (int i = 2; i <= n; i++)
            arr[i] = arr[i - 1] + arr[i - 2];
        return arr;
    }

    /// <summary>A = ln(x^2 + 1) * sin^2(x / 3) + sqrt(|x - 7|).</summary>
    public static double Functions(double x)
    {
        double Lnx = Math.Log(x * x + 1);
        double Sinx = Math.Sin(x / 3);
        Sinx *= Sinx;
        double Sqrtx = Math.Sqrt(Math.Abs(x - 7));
        return Lnx * Sinx + Sqrtx;
    }

    /// <summary>Сумма ряда Тейлора для sin(x) с точностью eps и количество просуммированных членов.</summary>
    public static (double sum, int count) Taylorsin(double x, double eps)
    {
        if (!double.IsFinite(x)) throw new ArgumentOutOfRangeException(nameof(x), "x должен быть конечным числом");
        if (!(eps > 0)) throw new ArgumentOutOfRangeException(nameof(eps), "eps должен быть положительным");

        double sum = 0;
        int count = 0;
        double firstchel = x;
        int n = 1; // степень первого члена (1-3-5..)

        while (Math.Abs(firstchel) > eps)
        {
            sum += firstchel;
            count++;
            n += 2;
            firstchel = -firstchel * x * x / ((n - 1) * n);
        }
        return (sum, count);
    }
}
