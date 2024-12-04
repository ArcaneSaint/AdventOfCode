using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers.Year2015;

internal class Day6Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2015(6, part1Test, part2Test)
{
    bool[,] grid = new bool[1000, 1000];

    public override long Part1(string[] input)
    {
        foreach (var line in input)
        {
        }

        return 0;
    }

    private static (int X1, int Y1, int X2, int Y2) Parse(string input)
    {
        //Regex.Match(input, @"turn on (\d)*,(\d)* through (\d)*,(\d)*");

        //turn on 0,0 through 999,999
        input = input.Substring("turn on".Length);
        var s1 = input.Substring(0, input.IndexOf("through"));
        /*
            if (Regex.IsMatch(line, @"(?=.*(..).*(\1).*)(?=.*(.).(\3).*)"))
            {
                ++result;
            }*/
        throw new Exception();
    }

    public override long Part2(string[] input)
    {
        throw new NotImplementedException();
    }
}
