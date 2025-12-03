using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers.Year2015;

internal class Day8Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2015(8, part1Test, part2Test)
{
    public override long Part1(string[] input)
    {
        return input.Sum(StringCharacters) - input.Sum(CodeCharacters);
    }


    private static int StringCharacters(string line) => line.Length;
    private static int CodeCharacters(string line) => Regex.Replace(line, "\\\\x[0-9A-Fa-f]{2}", "0")
        .Replace(@"\\", "0")
        .Replace("\\\"", "0")
        .Length - 2;


    private static string Encode(string line)
    {
        return Regex.Replace(line, "\\\\x[0-9A-Fa-f]{2}", "00000")
        .Replace(@"\\", "0000")
        .Replace("\\\"", "0000")
        .Replace("\"", "000");
    }


    public override long Part2(string[] input)
    {
        return input.Sum(x => StringCharacters(Encode(x))) - input.Sum(StringCharacters);
    }
}
