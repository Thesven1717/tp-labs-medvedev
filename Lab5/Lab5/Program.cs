using System.Diagnostics;

const int N = 100000000;
const double A = 0;
const double B = Math.PI;
const double Exact = 2.0;

Console.Write($"Количество потоков (ядер в системе: {Environment.ProcessorCount}): ");
int threads = int.TryParse(Console.ReadLine(), out int parsed) && parsed > 0 ? parsed: Environment.ProcessorCount;

var sw = new Stopwatch();

//последовательно
sw.Restart();
double seq = Sequential();
sw.Stop();
long seqTime = sw.ElapsedMilliseconds;

//вручную по потокам
sw.Restart();
double byThreads = ByThreads(threads);
sw.Stop();
long threadsTime = sw.ElapsedMilliseconds;

//Parallel.For
sw.Restart();
double byParallelFor = ByParallelFor(threads);
sw.Stop();
long parallelTime = sw.ElapsedMilliseconds;

Console.WriteLine();
Print("Последовательно", seq, seqTime);
Print($"Потоки вручную ({threads})", byThreads, threadsTime);
Console.WriteLine($"  ускорение: {(double)seqTime / threadsTime:F2}");
Print($"Parallel.For ({threads})", byParallelFor, parallelTime);
Console.WriteLine($"  ускорение: {(double)seqTime / parallelTime:F2}");

bool same = Math.Abs(byThreads - seq) < 1e-6 && Math.Abs(byParallelFor - seq) < 1e-6;
Console.WriteLine();
Console.WriteLine(same ? "Результаты всех версий совпадают." : "Результаты не совпадают.");

// таблица
Console.WriteLine();
Console.WriteLine($"Последовательная версия: {seqTime} мс");
Console.WriteLine("| Потоков | Thread, мс | Ускорение | Parallel.For, мс | Ускорение |");
Console.WriteLine("|--------:|-----------:|----------:|-----------------:|----------:|");
foreach (int count in new[] { 1, 2, 4, 8 })
{
    sw.Restart();
    ByThreads(count);
    sw.Stop();
    long a = sw.ElapsedMilliseconds;

    sw.Restart();
    ByParallelFor(count);
    sw.Stop();
    long b = sw.ElapsedMilliseconds;

    Console.WriteLine($"| {count,7} | {a,10} | {(double)seqTime / a,9:F2} | {b,16} | {(double)seqTime / b,9:F2} |");
}

// подынтегральная функция
static double F(double x)
{
    return Math.Sin(x);
}

static void Print(string title, double result, long ms)
{
    Console.WriteLine($"{title}: {result:F12}, погрешность {Math.Abs(result - Exact):E2}, время {ms} мс");
}

// метод трапеций
static double Sequential()
{
    double h = (B - A) / N;
    double sum = (F(A) + F(B)) / 2;

    for (int i = 1; i < N; i++)
        sum += F(A + i * h);

    return sum * h;
}

// точки 1 .. N-1
static double ByThreads(int threadCount)
{
    double h = (B - A) / N;
    double[] sums = new double[threadCount];
    Thread[] workers = new Thread[threadCount];
    int partSize = (N - 1) / threadCount;

    for (int p = 0; p < threadCount; p++)
    {
        int part = p;
        int start = 1 + part * partSize;
        int end = (part == threadCount - 1) ? N : start + partSize;

        workers[part] = new Thread(() =>
        {
            double partSum = 0;
            for (int i = start; i < end; i++)
                partSum += F(A + i * h);

            sums[part] = partSum;
        });
        workers[part].Start();
    }

    foreach (Thread w in workers)
        w.Join();

    double sum = (F(A) + F(B)) / 2;
    foreach (double s in sums)
        sum += s;

    return sum * h;
}

// Parallel.For
static double ByParallelFor(int threadCount)
{
    double h = (B - A) / N;
    double total = (F(A) + F(B)) / 2;
    object lockObj = new();

    var options = new ParallelOptions { MaxDegreeOfParallelism = threadCount };

    Parallel.For(1, N, options, () => 0.0, (i, state, local) => local + F(A + i * h), local =>
        {
            lock (lockObj)
            {
                total += local;
            }
        });

    return total * h;
}
