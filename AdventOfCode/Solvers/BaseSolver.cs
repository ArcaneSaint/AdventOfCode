using System.Diagnostics;
using AdventOfCode.Helpers;
using AdventOfCode.Interfaces;

namespace AdventOfCode.Solvers;

public abstract class BaseSolver<T>(int day, int year, T part1Test, T part2Test) : ISolver<T>
{
    public virtual string? AdditionalInfo { get; protected set; }

    public void Solve()
    {
        Console.WriteLine($"AoC {year} day {day}");

        var testInput = DataHelper.GetInput(year, day, true);
        var input = DataHelper.GetInput(year, day);

        var testInput2 = DataHelper.GetInput(year, day, true, 2);
        var input2 = DataHelper.GetInput(year, day, false, 2);

        TestPart(testInput, input, 1, part1Test, Part1);
        TestPart(testInput2 ?? testInput, input2 ?? input, 2, part2Test, Part2);

        Console.WriteLine();
    }

    private void TestPart(string[] testInput, string[] input, int part, T expectedTest, Func<string[], T> action)
    {
        Console.WriteLine($"Part {part}");

        var (totalTimeTest, testResult) = Measure(action, testInput);
        Display(totalTimeTest, testResult, "Test", testResult.Equals(expectedTest) ? ConsoleColor.Green : ConsoleColor.DarkRed);

        var (totalTime, result) = Measure(action, input);
        Display(totalTime, result, "Result", ConsoleColor.Blue);
    }

    private void Display(TimeSpan totalTime, T result, string label, ConsoleColor resultColor)
    {
        Console.Write($"  {label}: ");
        using (new ColorOutputter(resultColor))
        {
            Console.WriteLine(result);
        }
        if (!String.IsNullOrWhiteSpace(AdditionalInfo))
        {
            using (new ColorOutputter(ConsoleColor.DarkGray))
            {
                Console.WriteLine($"   {AdditionalInfo}");
            }
        }
        Console.WriteLine($"   Total time elapsed: {totalTime}");
    }
    private static (TimeSpan TotalTime, T Result) Measure(Func<string[], T> action, string[] input)
    {
        Stopwatch watch = Stopwatch.StartNew();
        var result = action(input);
        watch.Stop();
        return (watch.Elapsed, result);
    }

    public abstract T Part1(string[] input);

    public abstract T Part2(string[] input);
}
