namespace AdventOfCode.Solvers.Year2024;

internal class Day18Solver(string part1Test, string part2Test) : BaseSolver2024<string>(18, part1Test, part2Test)
{
    class Tile
    {
        public bool IsWall { get; set; }
        public int Cost { get; set; } = Int32.MaxValue;
        public (int row, int col) Position { get; set; }
    }


    List<Tile> CreateGrid(int width, int height)
    {
        List<Tile> grid = new List<Tile>();
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                grid.Add(new Tile()
                {
                    Position = (i, j)
                });
            }
        }
        return grid;
    }

    void MarkWalls(string[] input, List<Tile> tiles, int bytesToFall)
    {
        foreach (var item in input[..bytesToFall])
        {
            var col = item.Split(',')[0].ToInt();
            var row = item.Split(',')[1].ToInt();

            tiles.FirstOrDefault(x => x.Position == (row, col)).IsWall = true;
        }

    }
    private void DrawGrid(List<Tile> tiles)
    {
        using (new ColorOutputter(ConsoleColor.White))
        {
            Console.Clear();
            Console.CursorVisible = false;
            foreach (var tile in tiles)
            {
                Console.SetCursorPosition(tile.Position.col, tile.Position.row);
                Console.Write(tile.IsWall ? '#' : '.');
            }
        }
    }

    int SolveMaze(List<Tile> tiles, Tile startingTile, Tile endingTile)
    {
        var possibleTiles = tiles.Where(x => !x.IsWall).ToList();
        startingTile.Cost = 0;
        var frontier = new List<Tile>();
        var nextFrontier = new List<Tile>(){
            startingTile
        };
        while (nextFrontier.Count > 0)
        {
            frontier = nextFrontier.Distinct().ToList();
            nextFrontier = new List<Tile>();
            foreach (var tile in frontier)
            {
                if (tile == endingTile)
                {
                    return tile.Cost;
                }
                //DrawPosition(item.row, item.col, 'O');
                var currentCost = tile.Cost;

                var neighbors = possibleTiles.Where(x =>
                x.Cost > currentCost && (
                x.Position == (tile.Position.row - 1, tile.Position.col)
            || x.Position == (tile.Position.row + 1, tile.Position.col)
            || x.Position == (tile.Position.row, tile.Position.col - 1)
            || x.Position == (tile.Position.row, tile.Position.col + 1))
                ).ToList();

                foreach (var neighbor in neighbors)
                {
                    neighbor.Cost = currentCost + 1;
                    nextFrontier.Add(neighbor);
                }
            }
        }

        return endingTile.Cost;
    }

    public override string Part1(string[] input)
    {
        int width, height;
        int bytesToFall = 0;
        if (input.Count() == 25)
        {
            width = 7;
            height = 7;
            bytesToFall = 12;
        }
        else
        {
            width = 71;
            height = 71;
            bytesToFall = 1024;
        }

        var tiles = CreateGrid(width, height);
        MarkWalls(input, tiles, bytesToFall);
        //DrawGrid(tiles);
        var startingTile = tiles.FirstOrDefault(x => x.Position == (0, 0));
        var endingTile = tiles.FirstOrDefault(x => x.Position == (height - 1, width - 1));

        return SolveMaze(tiles, startingTile, endingTile).ToString();
    }

    public override string Part2(string[] input)
    {
        int width, height;
        var lowestIndex = 0;
        if (input.Count() == 25)
        {
            width = 7;
            height = 7;
            lowestIndex = 12;
        }
        else
        {
            width = 71;
            height = 71;
            lowestIndex = 1024;
        }

        var endingCost = Int32.MaxValue;

        var highestIndex = input.Length;


        var middleIndex = lowestIndex + ((highestIndex - lowestIndex) / 2);


        do
        {
            var tiles = CreateGrid(width, height);
            MarkWalls(input, tiles, middleIndex);
            //Console.WriteLine($"{lowestIndex} - {middleIndex} - {highestIndex}");
            //DrawGrid(tiles);
            var startingTile = tiles.FirstOrDefault(x => x.Position == (0, 0));
            var endingTile = tiles.FirstOrDefault(x => x.Position == (height - 1, width - 1));
            endingCost = SolveMaze(tiles, startingTile, endingTile);
            if (endingCost == Int32.MaxValue)
            {
                highestIndex = middleIndex;
            }
            else
            {
                lowestIndex = middleIndex;
            }
            middleIndex = lowestIndex + ((highestIndex - lowestIndex) / 2);
        } while (middleIndex != lowestIndex);






        return input[lowestIndex];
    }
}
