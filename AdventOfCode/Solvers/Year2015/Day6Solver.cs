using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers.Year2015;

internal class Day6Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2015(6, part1Test, part2Test)
{
    public override long Part1(string[] input)
    {
        var grid = new bool[1000, 1000];

        foreach (var line in input)
        {
            var parsed = Parse(line);


            for (var i = parsed.X1; i <= parsed.X2; i++)
            {
                for (var j = parsed.Y1; j <= parsed.Y2; j++)
                {
                    grid[i, j] = parsed.state ?? !grid[i, j];

                }
            }

        }

        return grid.Cast<bool>().Count(x => x);
    }

    private static (bool? state, int X1, int Y1, int X2, int Y2) Parse(string input)
    {
        var parsed = Regex.Match(input, @"(turn on|turn off|toggle) (\d*),(\d*) through (\d*),(\d*)");

        if (parsed.Success && parsed.Groups.Count == 6)
        {
            bool? state = null;
            switch (parsed.Groups[1].Value)
            {
                case "turn on": state = true; break;
                case "turn off": state = false; break;
            }

            return (state,
                parsed.Groups[2].Value.ToInt(),
                parsed.Groups[3].Value.ToInt(),
                parsed.Groups[4].Value.ToInt(),
                parsed.Groups[5].Value.ToInt());
        }

        throw new FormatException();
    }

    public override long Part2(string[] input)
    {
        var grid = new long[1000, 1000];

        foreach (var line in input)
        {
            var parsed = Parse(line);


            for (var i = parsed.X1; i <= parsed.X2; i++)
            {
                for (var j = parsed.Y1; j <= parsed.Y2; j++)
                {
                    grid[i, j] = parsed.state switch
                    {
                        true => grid[i, j] +1,
                        false => Math.Max(grid[i, j] - 1, 0),
                        null => grid[i, j] +2
                    };
                }
            }

        }

        return grid.Cast<long>().Sum();
    }
}
