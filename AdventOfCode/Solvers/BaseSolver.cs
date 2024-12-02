using System.Diagnostics;
using AdventOfCode.Helpers;
using AdventOfCode.Interfaces;

namespace AdventOfCode.Solvers;

public abstract class BaseSolver(int day, int year) : ISolver
{
    private static (TimeSpan TotalTime, string Result) Measure(Func<string, string> action, string input)
    {
        Stopwatch watch = Stopwatch.StartNew();
        var result = action(input);
        watch.Stop();
        return (watch.Elapsed, result);
    }

    private void Display(TimeSpan totalTime, string result, int part)
    {
        Console.WriteLine($"AoC {year} day {day}, part {part}");
        Console.Write($"Result: ");
        using (new ColorOutputter(ConsoleColor.Green))
        {
            Console.WriteLine(result);
        }
        Console.WriteLine($"Total time elapsed: {totalTime}");
    }

    public void Solve()
    {
        var input = DataHelper.GetInput(year, day);

        var (totalTime, result) = Measure(Part1, input);
        Display(totalTime, result, 1);


        var (timeSpan, s) = Measure(Part2, input);
        Display(timeSpan, s, 2);
    }

    public abstract string Part1(string input);

    public abstract string Part2(string input);
}
