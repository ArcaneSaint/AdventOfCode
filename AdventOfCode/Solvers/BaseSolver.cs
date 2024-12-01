using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.Helpers;
using AdventOfCode.Interfaces;

namespace AdventOfCode.Solvers;

public abstract class BaseSolver(int day, int year) : ISolver
{
    private (TimeSpan TotalTime, string Result) Measure(Func<string, string> action, string input)
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

        var part1 = Measure(Part1, input);
        Display(part1.TotalTime, part1.Result, 1);


        var part2 = Measure(Part2, input);
        Display(part2.TotalTime, part2.Result, 2);
    }

    public abstract string Part1(string input);

    public abstract string Part2(string input);
}
