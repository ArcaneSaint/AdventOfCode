using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers.Year2015;

internal class Day5Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2015(5, part1Test, part2Test)
{
    public override long Part1(string[] input)
    {
        var result = 0;
        foreach (var line in input)
        {
            if (
                line.Count(c => c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u') >= 3 &&
                line.Any(c => line.Contains($"{c}{c}")) &&
                !line.Contains("ab") &&
                !line.Contains("cd") &&
                !line.Contains("pq") &&
                !line.Contains("xy"))
            {
                ++result;
            }
        }
        return result;
    }

    public override long Part2(string[] input)
    {
        var result = 0;
        foreach (var line in input)
        {
            if (Regex.IsMatch(line, @"(?=.*(..).*(\1).*)(?=.*(.).(\3).*)"))
            {
                ++result;
            }
        }
        return result;
        throw new NotImplementedException();
    }
}
