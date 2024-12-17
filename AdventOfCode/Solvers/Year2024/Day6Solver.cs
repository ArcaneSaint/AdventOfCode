namespace AdventOfCode.Solvers.Year2024;

internal class Day6Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2024<long>(6, part1Test, part2Test)
{
    public override long Part1(string[] input)
    {
        var (spaces, direction, guardPosition) = ParseInput(input);
        //InitDisplay(spaces);
        return GetFullPath(spaces, direction, guardPosition).Cast<TileType>().Count(x => x == TileType.Visited);
    }


    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used for debugging display")]
    private static void InitDisplay(TileType[,] spaces)
    {
        Console.Clear();
        Console.CursorVisible = false;
        for (var i = 0; i < spaces.GetLength(0) && i < Console.WindowHeight; ++i)
        {
            for (var j = 0; j < spaces.GetLength(1) && j < Console.WindowWidth; ++j)
            {
                switch (spaces[i, j])
                {
                    case TileType.Empty:
                        {
                            using (new ColorOutputter(ConsoleColor.DarkGray))
                            {
                                Console.Write('.');
                            }
                            break;
                        }
                    case TileType.Obstacle:
                        {
                            using (new ColorOutputter(ConsoleColor.Red))
                            {
                                Console.Write('#');
                            }
                            break;
                        }
                    default:
                        {

                            using (new ColorOutputter(ConsoleColor.White))
                            {
                                Console.Write('*');
                            }
                            break;
                        }
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used for debugging display")]
    private static void Display((int x, int y) guardPosition)
    {
        //Console.Clear();
        Console.CursorVisible = false;
        if (guardPosition.x < Console.WindowHeight && guardPosition.y < Console.WindowWidth)
        {
            Console.SetCursorPosition(guardPosition.y, guardPosition.x);
            Console.Write('*');
        }

        Thread.Sleep(1);
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

    private static bool IsLoop(TileType[,] spaces, (int dX, int dY) direction, (int x, int y) guardPosition, int newX, int newY)
    {
        var currentVisit = TileType.Visited;

        if (direction == (0, 1))
        {
            currentVisit = TileType.VisitedRight;
        }
        else if (direction == (0, -1))
        {
            currentVisit = TileType.VisitedLeft;
        }
        else if (direction == (1, 0))
        {
            currentVisit = TileType.VisitedDown;
        }
        else if (direction == (-1, 0))
        {
            currentVisit = TileType.VisitedUp;
        }


        spaces[guardPosition.x, guardPosition.y] = currentVisit;
        spaces[newX, newY] = TileType.Obstacle;

        while (guardPosition.x + direction.dX >= 0
               && guardPosition.y + direction.dY >= 0
               && guardPosition.x + direction.dX < spaces.GetLength(0)
               && guardPosition.y + direction.dY < spaces.GetLength(1))
        {
            var nextSpace = spaces[guardPosition.x + direction.dX, guardPosition.y + direction.dY];
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
                    (0, -1) => (-1, 0),
                    _ => throw new Exception("invalid direction")
                };
                currentVisit = currentVisit switch
                {
                    TileType.VisitedDown => TileType.VisitedLeft,
                    TileType.VisitedUp => TileType.VisitedRight,
                    TileType.VisitedLeft => TileType.VisitedUp,
                    TileType.VisitedRight => TileType.VisitedDown,
                    _ => throw new Exception("invalid visiting type")
                };
            }
        };

        return false;
    }

    private static TileType[,] GetFullPath(TileType[,] spaces, (int dX, int dY) direction, (int x, int y) guardPosition)
    {

        while (guardPosition.x + direction.dX >= 0
               && guardPosition.y + direction.dY >= 0
               && guardPosition.x + direction.dX < spaces.GetLength(0)
               && guardPosition.y + direction.dY < spaces.GetLength(1))
        {
            //Display(guardPosition);
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
                    (0, -1) => (-1, 0),
                    _ => throw new Exception("invalid direction")
                };
            }
        };

        return spaces;
    }

    private static (TileType[,] spaces, (int dX, int dY) direction, (int x, int y) guardPosition) ParseInput(string[] input)
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
                        '^' => (-1, 0),
                        _ => throw new Exception("invalid guard input character")
                    };
                    guardPosition = (i, j);

                    spaces[i, j] = TileType.Visited;
                }
            }
        }

        return (spaces, direction, guardPosition);
    }

    public override long Part2(string[] input)
    {
        var (spaces, direction, guardPosition) = ParseInput(input);
        var searchSpace = GetFullPath((spaces.Clone() as TileType[,])!, direction, guardPosition);
        var results = 0;
        var pathsChecked = 0;

        Parallel.For(0, input.Length * input[0].Length, i =>
        {

            if (searchSpace[i / input.Length, i % input.Length] == TileType.Visited)
            {
                pathsChecked++;
                if (IsLoop((spaces.Clone() as TileType[,])!, direction, guardPosition, i / input.Length, i % input.Length))
                {
                    results++;
                }
            }
        });

        AdditionalInfo = $"paths checked: {pathsChecked}";

        return results;
    }
}
