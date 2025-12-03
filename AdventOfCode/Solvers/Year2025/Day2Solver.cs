using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode.Solvers.Year2025;

internal class Day2Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2025<long>(2, part1Test, part2Test)
{
    private string patternToUse;
    public override long Part1(string[] input)
    {
        // language=regex
        patternToUse = "/^(.*)(\\1)$/";
        var ranges = input[0].Split(',');
        var total = 0l;
        foreach (var range in ranges)
        {
            var start = Int64.Parse(range.Split('-')[0]);
            var end = Int64.Parse(range.Split('-')[1]);

            total += ProcessRange(start, end);
        }
        return total;
    }

    private long ProcessRange(long start, long end)
    {
        var total = 0l;
        for (var i = start; i <= end; ++i)
        {
            if (IsMatch(i))
            {
                total += i;
            }
        }
        return total;
    }

    private bool IsMatch(long value)
    {
        return Regex.IsMatch(value.ToString(), patternToUse);
    }

    public override long Part2(string[] input)
    {
        // language=regex
        patternToUse = "/^(.*)(\\1)+$/";
        var ranges = input[0].Split(',');
        var total = 0l;
        foreach (var range in ranges)
        {
            var start = Int64.Parse(range.Split('-')[0]);
            var end = Int64.Parse(range.Split('-')[1]);

            total += ProcessRange(start, end);
        }
        return total;
    }
}
