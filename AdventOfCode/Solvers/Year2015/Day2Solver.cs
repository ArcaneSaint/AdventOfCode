namespace AdventOfCode.Solvers.Year2015;

internal class Day2Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2015(2, part1Test, part2Test)
{
    public override long Part1(string[] input)
    {
        var result = 0;

        foreach (var line in input)
        {
            var (l, w, h) = Parse(line);

            result += (2 * l * w) + (2 * w * h) + (2 * h * l) + l*w;
        }

        return result;
    }

    private (int l, int w, int h) Parse(string line)
    {
        var dim = line.Split('x').Select(Int32.Parse).Order().ToList();
        return (dim[0], dim[1], dim[2]);
    }

    public override long Part2(string[] input)
    {
        var result = 0;

        foreach (var line in input)
        {
            var (l, w, h) = Parse(line);

            result += l * w * h + 2 * l + 2 * w;
        }

        return result;
    }
}
