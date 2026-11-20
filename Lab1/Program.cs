using System;
using System.Globalization;
using System.Numerics;


while (true)
{
    Console.WriteLine("Лабораторная работа 1, вариант 11");
    Console.WriteLine("1. Факториал числа n BigInteger");
    Console.WriteLine("2. Число фибоначи от 0 до n");
    Console.WriteLine("3. Функция А=ln(x^2+1)*sin^2(x/3)+sqrt(abs(x-7))");
    Console.WriteLine("4. Ряд тейлора для sin(x)");
    Console.WriteLine("0. Выход");
    Console.Write("Ваш выбор: ");
    if (!int.TryParse(Console.ReadLine(), out int choice ))
    {
        Console.WriteLine("Введите число!");
        continue;
    }

    switch (choice)
    {
        case 1: Task1();
            break;
        case 2: Task2();
            break;
        case 3: Task3();
            break;
        case 4: Task4();
            break;
        case 0: return;
    }
}

static BigInteger Factorial(int n)
{
    BigInteger result = BigInteger.One;
    for (int i = 2; i <= n; i++)
        result = result * i;
    return result;
}

static long[] Fibonachi(int n)
{
    long[] arr = new long[n + 1];
    if (n >= 0) arr[0] = 0;
    if (n>=1) arr[1] = 1;
    for(int i = 2; i <= n; i++)
        arr[i] = arr[i - 1] + arr[i-2];
    return arr;
}

static double Functions(double x)
{
    double Lnx = Math.Log(x * x + 1);
    double Sinx = Math.Sin(x / 3);
    Sinx *= Sinx;
    double Sqrtx = Math.Sqrt(Math.Abs(x - 7));
    return Lnx * Sinx + Sqrtx;
}

static (double sum, int count) Taylorsin(double x, double eps)
{
    double sum = 0;
    int count = 0;
    double firstchel = x;
    int n = 1; //степень первого члена (1-3-5..)

    while (Math.Abs(firstchel) > eps)
    {
        sum += firstchel;
        count++;
        n += 2;
        firstchel = -firstchel * x * x / ((n - 1) * n);
    }
    return(sum, count);
}

static void Task1()
{
    Console.WriteLine("Введите число n>=0");
    if (!int.TryParse(Console.ReadLine(), out int n) || n<0)
    {
        Console.WriteLine("Нужно целое число >=0");
        return;
    }
    BigInteger result = Factorial(n);
    Console.WriteLine(n+"! =" + result);
}

static void Task2()
{
    Console.WriteLine("Введите число n>=0");
    if (!int.TryParse(Console.ReadLine(), out int n) || n < 0)
    {
        Console.WriteLine("Нужно целое число >=0");
        return;
    }
    Console.WriteLine(string.Join(", ", Fibonachi(n)));
}

static void Task3()
{
    Console.WriteLine("Введите число x");
    if (!double.TryParse(Console.ReadLine(), out double x))
    {
        Console.WriteLine("Разделитель запятая");
        return;
    }
    double a = Functions(x);
    Console.WriteLine($"a = {a:F6}");
}

static void Task4()
{
    Console.WriteLine("Введите число x");
    if (!double.TryParse(Console.ReadLine(), out double x))
    {
        Console.WriteLine("Разделитель запятая");
        return;
    }
    const double eps = 1e-6;
    (double sum, int count) = Taylorsin(x, eps);
    double b = Math.Sin(x);

    Console.WriteLine($"Сумма ряда равна {sum:F10}");
    Console.WriteLine($"Количество просуммированых членов равно {count}");
    Console.WriteLine($"Синус равен {b:F10}");
    Console.WriteLine($"Разница между Math и суммированием членов {Math.Abs(sum-b):F3}");
}