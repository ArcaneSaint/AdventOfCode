namespace AdventOfCode.Solvers.Year2015;

internal class Day1Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2015(1, part1Test, part2Test)
{
    public override long Part1(string[] input)
    {
        return input[0].Sum(c => (c - '(') * -2 + 1);

    }

    private readonly Dictionary<int, int> _scores = [];

    public override long Part2(string[] input)
    {
        var current = 0;
        for(int i = 0; i < input[0].Length; ++i)
        {
            current += (input[0][i] - '(') * -2 + 1;
            if (current == -1)
            {
                return i + 1;
            }
        }
        return 0;
    }
}
