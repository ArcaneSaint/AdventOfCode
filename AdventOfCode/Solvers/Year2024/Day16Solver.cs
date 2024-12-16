using System.IO.Pipes;
using System.Runtime.InteropServices;
using AdventOfCode.Helpers;

namespace AdventOfCode.Solvers.Year2024;

internal class Day16Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2024(16, part1Test, part2Test)
{
    class Tile
    {
        public bool IsWall { get; set; }
        public int Cost { get; set; } = Int32.MaxValue;
        public int CostDelta { get; set; } = Int32.MaxValue;
        public (int row, int col) Position { get; set; }
    }
    private (Tile[,] tiles, (int row, int col) startingPosition, (int row, int col) endingPosition) ParseInput(string[] input)
    {
        var result = new Tile[input.Length, input[0].Length];
        (int i, int j) startingPosition = (0, 0);
        (int i, int j) endingPosition = (0, 0);

        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[i].Length; j++)
            {
                var position = (i, j);
                switch (input[i][j])
                {
                    case '#':
                        {
                            result[i, j] = new Tile()
                            {
                                IsWall = true,
                                Position = position,
                            };
                            break;
                        }
                    case '.':
                        {
                            result[i, j] = new Tile()
                            {
                                IsWall = false,
                                Position = position,
                            };
                            break;
                        }
                    case 'E':
                        {
                            result[i, j] = new Tile()
                            {
                                IsWall = false,
                                Position = position,
                            };
                            endingPosition = (i, j);
                            break;
                        }
                    case 'S':
                        {
                            result[i, j] = new Tile()
                            {
                                IsWall = false,
                                Position = position,
                                Cost = 0,
                                CostDelta = 0
                            };
                            startingPosition = (i, j);
                            break;
                        }
                }
            }
        }

        return (result, startingPosition, endingPosition);
    }

    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private Direction[] GetRightCorners(Direction direction)
    {
        return direction switch
        {
            Direction.Up => [Direction.Left, Direction.Right],
            Direction.Down => [Direction.Left, Direction.Right],
            Direction.Left => [Direction.Up, Direction.Down],
            Direction.Right => [Direction.Up, Direction.Down],
            _ => []
        };
    }
    private (int row, int col) PositionOffset((int row, int col) startingPosition, Direction direction)
    {
        switch (direction)
        {
            default: return startingPosition;
            case Direction.Up: return (startingPosition.row - 1, startingPosition.col);
            case Direction.Down: return (startingPosition.row + 1, startingPosition.col);
            case Direction.Left: return (startingPosition.row, startingPosition.col - 1);
            case Direction.Right: return (startingPosition.row, startingPosition.col + 1);
        }
    }

    private void DrawGrid(Tile[,] tiles)
    {
        using (new ColorOutputter(ConsoleColor.White))
        {
            Console.Clear();
            Console.CursorVisible = false;
            for (var i = 0; i < tiles.GetLength(0) && i < Console.WindowHeight; i++)
            {
                for (var j = 0; j < tiles.GetLength(1) && j < Console.WindowWidth; j++)
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write(tiles[i, j].IsWall ? '#' : '.');
                }
            }
        }
    }

    private int turns = 0;

    private void DrawPosition(int row, int col, Direction direction) =>
        DrawPosition(row, col, direction switch
        {
            Direction.Up => '^',
            Direction.Down => 'v',
            Direction.Left => '<',
            Direction.Right => '>',
            _ => '.'
        });

    private void DrawPosition(int row, int col, char symbol)
    {
        if (row < Console.WindowHeight && col < Console.WindowWidth)
        {
            Console.SetCursorPosition(col, row);
            Console.Write(symbol);
        }
    }

    private int Solve(Tile[,] tiles, (int row, int col) startingPosition, (int row, int col) endingPosition)
    {
        var frontier = new List<(int row, int col, Direction direction)>();
        var visited = new List<Tile>();

        int currentBest = 0;

        var nextFrontier = new List<(int row, int col, Direction direction)>(){
            (startingPosition.row, startingPosition.col, Direction.Right)
        };

        while (nextFrontier.Count > 0)
        {
            frontier = nextFrontier.Where(x => tiles[x.row, x.col].Cost <= currentBest).ToList();
            nextFrontier = nextFrontier.Where(x => tiles[x.row, x.col].Cost > currentBest).ToList();


            currentBest = Int32.MaxValue;

            foreach (var item in frontier)
            {
                //DrawPosition(item.row, item.col, 'O');
                var currentCost = tiles[item.row, item.col].Cost;
                var forwardPosition = PositionOffset((item.row, item.col), item.direction);
                var forwardTile = tiles.GetItem(forwardPosition);
                var forwardCost = currentCost + 1;
                //DrawPosition(item.row, item.col, item.direction);

                var sideDirections = GetRightCorners(item.direction); //also look to the sides


                if (forwardTile.Cost > forwardCost && !forwardTile.IsWall)
                {
                    forwardTile.Cost = forwardCost;
                    forwardTile.CostDelta = forwardCost - currentCost;
                    currentBest = Math.Min(forwardCost, currentBest);
                    nextFrontier.Add((forwardPosition.row, forwardPosition.col, item.direction));
                }
                foreach (var sideDirection in sideDirections)
                {
                    var sideCost = currentCost + 1001;
                    var sidePosition = PositionOffset((item.row, item.col), sideDirection);
                    var sideTile = tiles.GetItem(sidePosition);
                    if (sideTile.Cost > sideCost && !sideTile.IsWall)
                    {
                        sideTile.Cost = sideCost;
                        sideTile.CostDelta = sideCost - currentCost;
                        currentBest = Math.Min(sideCost, currentBest);
                        nextFrontier.Add((sidePosition.row, sidePosition.col, sideDirection));
                    }

                }

            }
        }




        return tiles.GetItem(endingPosition).Cost;



    }





    public override long Part1(string[] input)
    {
        var (tiles, startingPosition, endingPosition) = ParseInput(input);
        //DrawGrid(tiles);
        return Solve(tiles, startingPosition, endingPosition);
    }


    private (int row, int col)? previousPosition = null;
    private void DrawPositionWithExpiry(int row, int col, char symbol)
    {
        if (previousPosition != null)
        {
            Console.SetCursorPosition(previousPosition.Value.col, previousPosition.Value.row);
            using (new ColorOutputter(ConsoleColor.DarkGray))
            {
                Console.Write(symbol);
            }
        }
        if (row < Console.WindowHeight && col < Console.WindowWidth)
        {
            previousPosition = (row, col);
            Console.SetCursorPosition(col, row);
            using (new ColorOutputter(ConsoleColor.Green))
            {
                Console.Write(symbol);
            }
        }
    }

    private List<(int row, int col)> GetPossiblePaths(int maxCost, int totalCost, (int row, int col) position, (int row, int col) endingPosition, Direction currentDirection, Tile[,] tiles, List<(int row, int col)> visited)
    {
        var tile = tiles.GetItem(position);
        //DrawPositionWithExpiry(position.row, position.col, '0');
        //totalCost += tile.CostDelta;
        if (totalCost > maxCost || totalCost > tile.Cost + 2000)
        {
            return null; //no possible path, we are in a dead end
        }

        var nextVisited = new List<(int row, int col)>(visited);
        nextVisited.Add(position);

        if (position == endingPosition)
        {
            return nextVisited; //we are on the finish line
        }


        var neighbourDirections = new[] { Direction.Up, Direction.Down, Direction.Left, Direction.Right };

        var result = new List<(int row, int col)>();

        foreach (var neighbourDirection in neighbourDirections)
        {
            var neighbourPosition = PositionOffset(position, neighbourDirection);

            if (!visited.Contains(neighbourPosition) && tiles.GetItem(neighbourPosition) is { IsWall: false } neighbour)
            {
                var costDelta = neighbourDirection == currentDirection ? 1 : 1001;
                var possibility = GetPossiblePaths(maxCost, totalCost + costDelta, neighbour.Position, endingPosition, neighbourDirection, tiles, nextVisited);
                if (possibility != null)
                {
                    result.AddRange(possibility);
                }
            }
        }

        return result.Any() ? result : null;

    }

    public override long Part2(string[] input)
    {
        var (tiles, startingPosition, endingPosition) = ParseInput(input);
        var maxCost = Solve(tiles, startingPosition, endingPosition);

        //DrawGrid(tiles);
        var numberOfTiles = 0;

        var possiblePaths = GetPossiblePaths(maxCost, 0, startingPosition, endingPosition, Direction.Right, tiles, new List<(int row, int col)>());


        // var toDebug = possiblePaths.Distinct().OrderBy(x=>x.Cost).ToList();
        //foreach (var item in toDebug)
        //{
        // Console.SetCursorPosition(item.Position.col, item.Position.row);
        //   Console.Write('O');
        // }

        numberOfTiles = possiblePaths.Distinct().Count();
        //var bestPathCost = tiles.GetItem(endingPosition).Cost;

        //var frontier = new List<(int row, int col)>();
        //var nextFrontier = new List<(int row, int col)>()
        //{
        //    endingPosition
        //};

        //var foundTiles = new List<Tile>();

        //while (nextFrontier.Count > 0)
        //{
        //    numberOfTiles += nextFrontier.Count;
        //    frontier = nextFrontier.ToList();
        //    nextFrontier.Clear();
        //    var possibleNextFrontier = new List<Tile>();

        //    foreach (var item in frontier)
        //    {
        //        DrawPosition(item.row, item.col, 'O');
        //        var neighbours = new[] {tiles.GetItem(PositionOffset(item, Direction.Up))
        //            ,tiles.GetItem(PositionOffset(item, Direction.Down))
        //            ,tiles.GetItem(PositionOffset(item, Direction.Left))
        //            ,tiles.GetItem(PositionOffset(item, Direction.Right)) }.ToList();

        //        neighbours = neighbours.Where(x => x.Cost <= bestPathCost && !x.IsWall).ToList();
        //        possibleNextFrontier.AddRange(neighbours);
        //    }

        //    if (possibleNextFrontier.Any())
        //    {
        //        //lowest of the possible is the lowest
        //        var nextBestCost = possibleNextFrontier.Min(x => x.Cost);
        //        bestPathCost = nextBestCost;
        //        nextFrontier = possibleNextFrontier.Where(x => x.Cost == bestPathCost).Select(x => x.Position).ToList();
        //    }
        //}




        return numberOfTiles;
    }
}
