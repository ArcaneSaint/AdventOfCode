namespace AdventOfCode.Solvers.Year2025;

internal class Day1Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2025<long>(1, part1Test, part2Test)
{
    private int total = 50;
    public override long Part1(string[] input)
    {
        total = 50;
        var result = 0;
        foreach (var line in input)
        {
            result += HandleInput(line);
        }

        return result;
    }

    private int HandleInput(string line)
    {
        var number = Int32.Parse(line.Substring(1));
        if (line.StartsWith('L'))
        {
            number *= -1;
        }
        total += number;

        if (total % 100 == 0)
        {
            return 1;
        }
        return 0;
    }


    private int HandleInputPartTwo(string line)
    {
        var number = Int32.Parse(line.Substring(1));
        var offset = line.StartsWith('L') ? -1 : 1;
        var countOfZeroes = 0;

        for(var i = 0; i < number; ++i)
        {
            total += offset;

            if (total % 100 == 0)
            {
                ++countOfZeroes;
            }
        }

        return countOfZeroes;
    }

    public override long Part2(string[] input)
    {
        total = 50;
        var result = 0;
        foreach (var line in input)
        {
            result += HandleInputPartTwo(line);
        }

        return result;
    }
}
