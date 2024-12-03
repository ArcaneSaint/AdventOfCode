using System.Diagnostics;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode.Solvers.Year2024;

internal class Day3Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2024(3, part1Test, part2Test)
{
    public override long Part1(string[] input)
    {
        var result = 0;
        foreach (var line in input)
        {
            result += Match(line);
        }


        return result;
    }

    private int Match(string line)
    {
        var result = 0;
        var pattern = new Regex("mul\\((\\d+),(\\d+)\\)");
        var matches = pattern.Matches(line);

        foreach (Match match in matches)
        {
            var val1 = Int32.Parse(match.Groups[1].Value);
            var val2 = Int32.Parse(match.Groups[2].Value);
            result += val1 * val2;

        }

        return result;
    }

    public override long Part2(string[] input)
    {
        var result = 0;
        var line = string.Join("", input);
            var donts = ("do()"+line).Split("don't()").ToList();
            result += Match(donts[0]);
            for (var i = 1; i < donts.Count; i++)
            {
                var index = donts[i].IndexOf("do()", StringComparison.Ordinal);
                if (index > +0)
                {
                    var toProcess = donts[i][index..];

                    result += Match(toProcess);
                }
            
        }


        return result;
    }
}
