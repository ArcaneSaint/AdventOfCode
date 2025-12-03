using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers.Year2015;

internal partial class Day12Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2015(12, part1Test, part2Test)
{
    public override long Part1(string[] input)
    {
        return FindAllNumbers(input[0]).Sum();
    }

    public override long Part2(string[] input)
    {
        throw new NotImplementedException();
    }


    private static List<int> FindAllNumbers(string input)
    {
        var matches = NumberRegex().Matches(input);
        var result = new List<int>();
        foreach (var match in matches.ToList())
        {
            result.Add(Int32.Parse(match.Value));
        }
        return result;
    }



    [GeneratedRegex("-?\\d+")]
    private static partial Regex NumberRegex();
}
