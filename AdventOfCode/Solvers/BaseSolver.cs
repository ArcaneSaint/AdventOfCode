using System.Diagnostics;
using AdventOfCode.Helpers;
using AdventOfCode.Interfaces;

namespace AdventOfCode.Solvers;

public abstract class BaseSolver(int day, int year, long part1Test = 0, long part2Test = 0) : ISolver
{
    public virtual string? AdditionalInfo { get; protected set; }

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

        var (totalTimeTest, testResult) = Measure(action, testInput);
        Display(totalTimeTest, testResult, "Test", testResult == expectedTest ? ConsoleColor.Green : ConsoleColor.DarkRed);

        var (totalTime, result) = Measure(action, input);
        Display(totalTime, result, "Release", ConsoleColor.Blue);
    }

    private void Display(TimeSpan totalTime, long result, string label, ConsoleColor resultColor)
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
