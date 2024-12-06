namespace AdventOfCode.Solvers.Year2024;

internal class Day6Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2024(6, part1Test, part2Test)
{
    public override long Part1(string[] input)
    {
        TileType[,] spaces = new TileType[input.Length, input[0].Length];
        (int dX, int dY) direction = (0, 0);
        (int x, int y) guardPosition = (0, 0);
        for (var i = 0; i < input.Length; ++i)
        {
            var line = input[i];
            for (var j = 0; j < line.Length; ++j)
            {
                if (line[j] == '#')
                {
                    spaces[i, j] = TileType.Obstacle;
                }
                else if (line[j] == '.')
                {
                    spaces[i, j] = TileType.Empty;
                }
                else
                {
                    var guardIndicator = line[j];
                    direction = guardIndicator switch
                    {
                        '>' => (0, 1),
                        '<' => (0, -1),
                        'v' => (1, 0),
                        '^' => (-1, 0)
                    };
                    guardPosition = (i, j);

                    spaces[i, j] = TileType.Visited;
                }
            }
        }

        while (guardPosition.x + direction.dX >= 0
               && guardPosition.y + direction.dY >= 0
               && guardPosition.x + direction.dX < spaces.GetLength(0)
               && guardPosition.y + direction.dY < spaces.GetLength(1))
        {
            //Display(spaces);
            var nextSpace = spaces[guardPosition.x + direction.dX, guardPosition.y + direction.dY];
            if (nextSpace != TileType.Obstacle)
            {
                spaces[guardPosition.x + direction.dX, guardPosition.y + direction.dY] = TileType.Visited;
                guardPosition = (guardPosition.x + direction.dX, guardPosition.y + direction.dY);
            }
            else
            {
                direction = direction switch
                {
                    (1, 0) => (0, -1),
                    (0, 1) => (1, 0),
                    (-1, 0) => (0, 1),
                    (0, -1) => (-1, 0)
                };
            }
        };


        return spaces.Cast<TileType>().Count(x => x == TileType.Visited);
    }

    private void Display(TileType[,] spaces)
    {
        Console.Clear();
        for (var i = 0; i < spaces.GetLength(0); ++i)
        {
            for (var j = 0; j < spaces.GetLength(1); ++j)
            {
                Console.Write(spaces[i, j] switch
                {
                    TileType.Visited => '*',
                    TileType.Empty => '.',
                    TileType.Obstacle => '#'
                });
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    [Flags]
    enum TileType
    {
        Empty = 0b0000_0000,
        Obstacle = 0b0000_0001,
        VisitedUp = 0b0000_0010,
        VisitedDown = 0b0000_0100,
        VisitedLeft = 0b0000_1000,
        VisitedRight = 0b0001_0000,
        Visited = 0b0001_1110
    }

    public bool IsLoop(string[] input, int newX, int newY)
    {
        TileType[,] spaces = new TileType[input.Length, input[0].Length];
        var currentVisit = TileType.Visited;
        (int dX, int dY) direction = (0, 0);
        (int x, int y) guardPosition = (0, 0);
        for (var i = 0; i < input.Length; ++i)
        {
            var line = input[i];
            for (var j = 0; j < line.Length; ++j)
            {
                if (line[j] == '#')
                {
                    spaces[i, j] = TileType.Obstacle;
                }
                else if (line[j] == '.')
                {
                    if (i == newX && j == newY)
                    {
                        spaces[i, j] = TileType.Obstacle;
                    }
                    else
                    {
                        spaces[i, j] = TileType.Empty;
                    }
                }
                else
                {
                    var guardIndicator = line[j];
                    direction = guardIndicator switch
                    {
                        '>' => (0, 1),
                        '<' => (0, -1),
                        'v' => (1, 0),
                        '^' => (-1, 0)
                    };
                    currentVisit = guardIndicator switch
                    {
                        '>' => TileType.VisitedRight,
                        '<' => TileType.VisitedLeft,
                        'v' => TileType.VisitedDown,
                        '^' => TileType.VisitedUp
                    };
                    guardPosition = (i, j);

                    spaces[i, j] = currentVisit;
                }
            }
        }

        var loop = false;
        while (guardPosition.x + direction.dX >= 0
               && guardPosition.y + direction.dY >= 0
               && guardPosition.x + direction.dX < spaces.GetLength(0)
               && guardPosition.y + direction.dY < spaces.GetLength(1))
        {
            //Display(spaces);
            var thisSpace = spaces[guardPosition.x, guardPosition.y];

            var nextSpace = spaces[guardPosition.x + direction.dX, guardPosition.y + direction.dY];
            //if we have already walked this path, we are in a loop!
            if ((nextSpace & currentVisit) > 0)
            {
                return true;
            }
            if (nextSpace != TileType.Obstacle)
            {
                spaces[guardPosition.x + direction.dX, guardPosition.y + direction.dY] |= currentVisit;
                guardPosition = (guardPosition.x + direction.dX, guardPosition.y + direction.dY);
            }
            else
            {
                direction = direction switch
                {
                    (1, 0) => (0, -1),
                    (0, 1) => (1, 0),
                    (-1, 0) => (0, 1),
                    (0, -1) => (-1, 0)
                };
                currentVisit = currentVisit switch
                {
                    TileType.VisitedDown => TileType.VisitedLeft,
                    TileType.VisitedUp => TileType.VisitedRight,
                    TileType.VisitedLeft => TileType.VisitedUp,
                    TileType.VisitedRight => TileType.VisitedDown,
                };
            }
        };

        return false;
    }

    public override long Part2(string[] input)
    {
        var results = 0;
        for (var i = 0; i < input.Length; ++i)
        {
            for (var j = 0; j < input[i].Length; ++j)
            {
                if (IsLoop(input, i, j))
                {
                    results++;
                }
            }
        }

        return results;
    }
}
