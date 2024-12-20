namespace AdventOfCode.Solvers.Year2024;

internal class Day20Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2024<long>(20, part1Test, part2Test)
{

    class Tile
    {
        public bool IsWall { get; set; }
        public bool IsEnd { get; set; }
        public bool IsStart { get; set; }
        public int DistanceToEnd { get; set; } = Int32.MaxValue;
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
    void MarkWalls(string[] input, List<Tile> tiles)
    {
        for (var row = 0; row < input.Length; row++)
        {
            for (var col = 0; col < input[row].Length; col++)
            {
                switch (input[row][col])
                {
                    case '#':
                        tiles.FirstOrDefault(x => x.Position == (row, col)).IsWall = true;
                        break;
                    case 'S':
                        tiles.FirstOrDefault(x => x.Position == (row, col)).IsStart = true;
                        break;
                    case 'E':
                        tiles.FirstOrDefault(x => x.Position == (row, col)).IsEnd = true;
                        break;
                }
            }
        }
    }

    void CalucateDistances(List<Tile> tiles, Tile endingTile)
    {
        var possibleTiles = tiles.Where(x => !x.IsWall).ToList();
        var currentDistance = 0;
        endingTile.DistanceToEnd = currentDistance;
        var frontier = new List<Tile>();
        var nextFrontier = new List<Tile>(){
            endingTile
        };

        while (nextFrontier.Count > 0)
        {
            frontier = nextFrontier.Distinct().ToList();
            nextFrontier = new List<Tile>();
            foreach (var tile in frontier)
            {
                var neighbors = possibleTiles.Where(x =>
                x.DistanceToEnd > currentDistance && (
                x.Position == (tile.Position.row - 1, tile.Position.col)
            || x.Position == (tile.Position.row + 1, tile.Position.col)
            || x.Position == (tile.Position.row, tile.Position.col - 1)
            || x.Position == (tile.Position.row, tile.Position.col + 1))
                ).ToList();

                foreach (var neighbor in neighbors)
                {
                    neighbor.DistanceToEnd = currentDistance + 1;
                    nextFrontier.Add(neighbor);
                }
            }

            ++currentDistance;
        }
    }

    public override long Part1(string[] input)
    {
        var targetSeconds = 100;
        if (input.Length == 15)
        {
            targetSeconds = 10;
        }

        var tiles = CreateGrid(input.Length, input.Length);
        MarkWalls(input, tiles);

        var startingTile = tiles.FirstOrDefault(x => x.IsStart);
        var endingTile = tiles.FirstOrDefault(x => x.IsEnd);

        CalucateDistances(tiles, endingTile);

        var paths = CountTeleportsOver(tiles, startingTile, targetSeconds, 2);

        return paths;
    }

    private int CountPathsOver(List<Tile> tiles, Tile? startingTile, int targetSeconds)
    {
        var wallTiles = tiles.Where(x => x.IsWall).ToList();
        var pathTiles = tiles.Where(x => !x.IsWall).ToList();
        var results = 0;

        foreach (var wallTile in wallTiles)
        {
            var cheatingOptions = pathTiles.Where(x =>
            (
                x.Position == (wallTile.Position.row - 1, wallTile.Position.col)
            || x.Position == (wallTile.Position.row + 1, wallTile.Position.col)
            || x.Position == (wallTile.Position.row, wallTile.Position.col - 1)
            || x.Position == (wallTile.Position.row, wallTile.Position.col + 1))
                )
                .OrderByDescending(x => x.DistanceToEnd)
                .ToList();

            for (var i = 0; i < cheatingOptions.Count; i++)
            {
                for (var j = i + 1; j < cheatingOptions.Count; j++)
                {
                    if ((cheatingOptions[i].DistanceToEnd - cheatingOptions[j].DistanceToEnd - 2) >= targetSeconds)
                    {
                        results++;
                    }
                }
            }
        }

        return results;
    }

    private int CountTeleportsOver(List<Tile> tiles, Tile startingTile, int targetSeconds, int distanceToSkip)
    {
        //we can reach any tile where the Manhattan distance is < distanceToSkip
        var pathTiles = tiles.Where(x => !x.IsWall).OrderByDescending(x=>x.DistanceToEnd).ToList();
        var results = 0;

        for(var pI = 0;  pI < pathTiles.Count; ++pI)
        {
            var tile = pathTiles[pI];
            var possibleTeleports = pathTiles[pI..].Where(x => Manhattan(x.Position, tile.Position) <= distanceToSkip).ToList();

            for (var i = 0; i < possibleTeleports.Count; i++)
            {
                if ((tile.DistanceToEnd - possibleTeleports[i].DistanceToEnd - Manhattan(tile.Position, possibleTeleports[i].Position)) >= targetSeconds)
                {
                    var distanceDebug = tile.DistanceToEnd - possibleTeleports[i].DistanceToEnd;
                    var distanceJumped = Manhattan(tile.Position, possibleTeleports[i].Position);
                    results++;
                }

            }

        }


        return results;
    }

    //private Dictionary<((int, int), (int, int)), int> manhattanCache = new();
    private int Manhattan((int row, int col) position1, (int row, int col) position2)
    {
        return Math.Abs(position1.row - position2.row) + Math.Abs(position1.col - position2.col);
    }

    public override long Part2(string[] input)
    {
        var distanceToSkip = 20;
        var targetSeconds = 100;
        if (input.Length == 15)
        {
            targetSeconds = 50;
        }

        var tiles = CreateGrid(input.Length, input.Length);
        MarkWalls(input, tiles);

        var startingTile = tiles.FirstOrDefault(x => x.IsStart);
        var endingTile = tiles.FirstOrDefault(x => x.IsEnd);

        CalucateDistances(tiles, endingTile);

        var paths = CountTeleportsOver(tiles, startingTile, targetSeconds, distanceToSkip);

        return paths;
    }
}
