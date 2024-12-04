using System.Diagnostics;
using AdventOfCode.Helpers;
using AdventOfCode.Interfaces;

namespace AdventOfCode.Solvers;

public abstract class BaseSolver(int day, int year, long part1Test = 0, long part2Test = 0) : ISolver
{

    public void Solve()
    {
        Console.WriteLine($"AoC {year} day {day}");

        var testInput = DataHelper.GetInput(year, day, true);
        var input = DataHelper.GetInput(year, day);

        TestPart(testInput, input, 1, part1Test, Part1);
        TestPart(testInput, input, 2, part2Test, Part2);

        Console.WriteLine();
    }

    private void TestPart(string[] testInput, string[] input, int part, long expectedTest, Func<string[], long> action)
    {
        Console.WriteLine($"Part {part}");
        var testResult = action(testInput);

        Console.Write($"  Test: ");
        using (new ColorOutputter(testResult == expectedTest ? ConsoleColor.Green : ConsoleColor.DarkRed))
        {
            Console.WriteLine(testResult);
        }
        var (totalTime, result) = Measure(action, input);
        Display(totalTime, result);
    }

    private void Display(TimeSpan totalTime, long result)
    {
        Console.Write($"  Result: ");
        using (new ColorOutputter(ConsoleColor.Blue))
        {
            Console.WriteLine(result);
        }
        Console.WriteLine($" Total time elapsed: {totalTime}");
    }
    private static (TimeSpan TotalTime, long Result) Measure(Func<string[], long> action, string[] input)
    {
        Stopwatch watch = Stopwatch.StartNew();
        var result = action(input);
        watch.Stop();
        return (watch.Elapsed, result);
    }
    public abstract long Part1(string[] input);

    public abstract long Part2(string[] input);
}
