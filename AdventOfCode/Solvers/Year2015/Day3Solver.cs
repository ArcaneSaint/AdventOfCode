using System.Text.RegularExpressions;
namespace AdventOfCode.Solvers.Year2015;

internal class Day3Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2015(3, part1Test, part2Test)
{
    private List<(int, int)> visited = [(0, 0)];
    public override long Part1(string[] input)
    {
        visited = [];
        var coord = (X: 0, Y: 0);

        foreach (var c in input[0])
        {
            switch (c)
            {
                case '>': coord.Y += 1; break;
                case '<': coord.Y -= 1; break;
                case 'v': coord.X -= 1; break;
                case '^': coord.X += 1; break;
            }
            visited.Add(coord);
        }

        return visited.Distinct().Count();
    }

    public override long Part2(string[] input)
    {
        visited = [(0, 0)];
        var coord = (X: 0, Y: 0);
        var roboCoord = (X: 0, Y: 0);

        for (int i = 0; i < input[0].Length; i++)
        {
            var c = input[0][i];
            if (i % 2 == 0)
            {
                switch (c)
                {
                    case '>': coord.Y += 1; break;
                    case '<': coord.Y -= 1; break;
                    case 'v': coord.X -= 1; break;
                    case '^': coord.X += 1; break;
                }
                visited.Add(coord);
            }
            else
            {
                switch (c)
                {
                    case '>': roboCoord.Y += 1; break;
                    case '<': roboCoord.Y -= 1; break;
                    case 'v': roboCoord.X -= 1; break;
                    case '^': roboCoord.X += 1; break;
                }
                visited.Add(roboCoord);
            }
        }


        return visited.Distinct().Count();
    }
}
