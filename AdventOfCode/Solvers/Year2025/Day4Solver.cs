namespace AdventOfCode.Solvers.Year2025;

internal class Day4Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2025<long>(4, part1Test, part2Test)
{
    public override long Part1(string[] input)
    {
        return GoThroughGrid(input).Count;
    }

    public override long Part2(string[] input)
    {
        var removableTiles = GoThroughGrid(input);
        var total = 0;

        while (removableTiles.Any())
        {
            total += removableTiles.Count;
            input = RemoveTiles(removableTiles, input);
            removableTiles = GoThroughGrid(input);
        }

        return total;
    }

    private string[] RemoveTiles(List<(int X, int Y)> tilesToRemove, string[] input)
    {
        var charArrays = input.Select(line => line.ToCharArray()).ToList();
        tilesToRemove.ForEach(tile => charArrays[tile.X][tile.Y] = '.');
        return [.. charArrays.Select(line => new string(line))];
    }

    private bool IsPaperTile(int x, int y, string[] grid)
    {
        if (x < 0 || x >= grid.Length || y < 0 || y >= grid[x].Length)
        {
            return false;
        }

        return grid[x][y] == '@';
    }

    private List<(int, int)> GoThroughGrid(string[] grid)
    {
        var result = new List<(int, int)>();

        for (var x = 0; x < grid.Length; ++x)
        {
            for (var y = 0; y < grid[x].Length; ++y)
            {
                if (IsPaperTile(x, y, grid))
                {
                    var paperSpaces = 0;

                    if (IsPaperTile(x - 1, y - 1, grid))
                    {
                        ++paperSpaces;
                    }
                    if (IsPaperTile(x - 1, y, grid))
                    {
                        ++paperSpaces;
                    }
                    if (IsPaperTile(x - 1, y + 1, grid))
                    {
                        ++paperSpaces;
                    }

                    if (IsPaperTile(x, y - 1, grid))
                    {
                        ++paperSpaces;
                    }
                    if (IsPaperTile(x, y + 1, grid))
                    {
                        ++paperSpaces;
                    }

                    if (IsPaperTile(x + 1, y - 1, grid))
                    {
                        ++paperSpaces;
                    }
                    if (IsPaperTile(x + 1, y, grid))
                    {
                        ++paperSpaces;
                    }
                    if (IsPaperTile(x + 1, y + 1, grid))
                    {
                        ++paperSpaces;
                    }

                    if (paperSpaces < 4)
                    {
                        result.Add((x, y));
                    }
                }
            }
        }

        return result;
    }

}