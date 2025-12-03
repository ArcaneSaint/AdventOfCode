using System.Text;

namespace AdventOfCode.Solvers.Year2015;

internal class Day10Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2015(10, part1Test, part2Test)
{
    public override long Part1(string[] input)
    {
        var interations = Int32.Parse(input[1]);
        var value = input[0];

        for (int i = 0; i < interations; ++i)
        {
            value = Iterate(value);
        }
        return value.Length;
    }

    public override long Part2(string[] input)
    {
        var interations = Int32.Parse(input[1]);
        var value = input[0];

        for (int i = 0; i < 50; ++i)
        {
            value = Iterate(value);
        }
        return value.Length;
    }




    private static string Iterate(string input)
    {
        var result = new StringBuilder();

        var digit = input[0];
        var digitCount = 1;
        for (var i = 1; i < input.Length; ++i)
        {
            var currentDigit = input[i];
            if (currentDigit == digit)
            {
                digitCount++;
            }
            else
            {
                result.Append($"{digitCount}{digit}");
                digit = currentDigit;
                digitCount = 1;
            }
        }
        result.Append($"{digitCount}{digit}");

        return result.ToString();
    }
}
