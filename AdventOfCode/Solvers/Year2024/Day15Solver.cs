namespace AdventOfCode.Solvers.Year2024;

internal class Day15Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2024<long>(15, part1Test, part2Test)
{
    enum Command
    {
        Up,
        Down,
        Left,
        Right
    }

    class Tile
    {
        public (int Row, int Col) Position { get; set; }
        public bool IsWall { get; set; }
        public bool HasBox { get; set; }
        public bool HasRobot { get; set; }

        public bool IsRightBox { get; set; }
        public bool IsLeftBox { get; set; }
    }

    private Tile ParseTile(char input, int row, int col)
    {
        return new Tile
        {
            Position = (row, col),
            IsWall = input == '#',
            HasBox = input == 'O',
            HasRobot = input == '@'
        };
    }
    private Tile[] ParseWideTile(char input, int row, int col)
    {
        var left = new Tile()
        {
            Position = (row, col)
        };
        var right = new Tile()
        {
            Position = (row, col + 1)
        };
        switch (input)
        {
            case 'O':
                {
                    left.IsLeftBox = true;
                    right.IsRightBox = true;
                    left.HasBox = right.HasBox = true;
                    break;
                }
            case '#':
                {
                    left.IsWall = right.IsWall = true;
                    break;
                }
            case '@':
                {
                    left.HasRobot = true;
                    break;
                }
        }

        return [left, right];
    }
    private List<Tile> ParseTiles(string[] input)
    {
        var result = new List<Tile>();
        for (var i = 0; i < input.Length; ++i)
        {
            for (var j = 0; j < input[i].Length; ++j)
            {
                result.Add(ParseTile(input[i][j], i, j));
            }
        }
        return result;
    }

    private List<Tile> ParseWideTiles(string[] input)
    {
        var result = new List<Tile>();
        for (var i = 0; i < input.Length; ++i)
        {
            for (var j = 0; j < input[i].Length; ++j)
            {
                result.AddRange(ParseWideTile(input[i][j], i, j * 2));
            }
        }
        return result;
    }

    private List<Command> ParseCommands(string[] input)
    {
        var result = new List<Command>();
        for (var i = 0; i < input.Length; ++i)
        {
            for (var j = 0; j < input[i].Length; ++j)
            {
                result.Add(input[i][j] switch
                {
                    'v' => Command.Down,
                    '>' => Command.Right,
                    '<' => Command.Left,
                    '^' => Command.Up,
                });
            }
        }
        return result;
    }

    private (List<Tile> tiles, List<Command> commands) ParseInput(string[] input)
    {
        var tiles = ParseTiles(input.TakeWhile(x => !String.IsNullOrWhiteSpace(x)).ToArray());
        var commands = ParseCommands(input.SkipWhile(x => !String.IsNullOrWhiteSpace(x)).ToArray());
        return (tiles, commands);
    }

    private (List<Tile> tiles, List<Command> commands) ParseWideInput(string[] input)
    {
        var tiles = ParseWideTiles(input.TakeWhile(x => !String.IsNullOrWhiteSpace(x)).ToArray());
        var commands = ParseCommands(input.SkipWhile(x => !String.IsNullOrWhiteSpace(x)).ToArray());
        return (tiles, commands);
    }

    private (int Row, int Col) PositionOffset((int Row, int Col) startingPosition, Command command)
    {
        switch (command)
        {
            default: return startingPosition;
            case Command.Up: return (startingPosition.Row - 1, startingPosition.Col);
            case Command.Down: return (startingPosition.Row + 1, startingPosition.Col);
            case Command.Left: return (startingPosition.Row, startingPosition.Col - 1);
            case Command.Right: return (startingPosition.Row, startingPosition.Col + 1);
        }
    }


    private (bool canMove, List<Tile> tiles) CanMove(Command command, Tile pushed, List<Tile> tiles)
    {
        var resultingTiles = new List<Tile>() { pushed };
        var target = tiles.FirstOrDefault(x => x.Position == PositionOffset(pushed.Position, command));
        if (target.IsWall)
        {
            return (false, new()); //cannot move into wall
        }

        if (target.HasBox)
        {
            var mainBoxResuilt = CanMove(command, target, tiles);
            if (!mainBoxResuilt.canMove)
            {
                return (false, new());
            }

            if (command == Command.Up || command == Command.Down)
            {
                //we could move, maybe:
                if (target.IsRightBox)
                {
                    //left box has to be able to move as well
                    var attachedBox = tiles.FirstOrDefault(x => x.Position == PositionOffset(target.Position, Command.Left));
                    //can only move if all box mnoved
                    var result = CanMove(command, attachedBox, tiles);
                    if (!result.canMove)
                    {
                        return (false, new());
                    }
                    mainBoxResuilt.tiles.AddRange(result.tiles);
                }
                else if (target.IsLeftBox)
                {
                    var attachedBox = tiles.FirstOrDefault(x => x.Position == PositionOffset(target.Position, Command.Right));
                    //can only move if all box mnoved
                    var result = CanMove(command, attachedBox, tiles);
                    if (!result.canMove)
                    {
                        return (false, new());
                    }
                    mainBoxResuilt.tiles.AddRange(result.tiles);
                }
            }

            //everything can move:
            resultingTiles.AddRange(mainBoxResuilt.tiles);
        }

        //everything can move, someone else do the moving for us?
        return (true, resultingTiles);
    }

    private bool HandlePush(Command command, Tile pushed, List<Tile> tiles)
    {
        var target = tiles.FirstOrDefault(x => x.Position == PositionOffset(pushed.Position, command));
        if (target.IsWall)
        {
            return false; //cannot move into wall
        }
        if (target.HasBox)
        {
            if (target.IsRightBox)
            {
                //can only move if all box mnoved
                if (!HandlePush(command, tiles.FirstOrDefault(x => x.Position == PositionOffset(target.Position, Command.Left)), tiles))
                {
                    return false;
                }
            }
            else if (target.IsLeftBox)
            {
                //can only move if all box mnoved
                if (!HandlePush(command, tiles.FirstOrDefault(x => x.Position == PositionOffset(target.Position, Command.Right)), tiles))
                {
                    return false;
                }
            }
            //can only move if all box mnoved
            else if (!HandlePush(command, target, tiles))
            {
                return false;
            }
        }

        //we can move, box has to move
        pushed.HasBox = false;
        target.HasBox = true;
        return true;
    }

    private void Step(Command command, List<Tile> tiles)
    {
        var robot = tiles.FirstOrDefault(x => x.HasRobot);
        var robotTarget = tiles.FirstOrDefault(x => x.Position == PositionOffset(robot.Position, command));

        if (robotTarget.IsWall)
        {
            return;
        }
        if (robotTarget.HasBox)
        {
            var pushAnalysis = CanMove(command, robotTarget, tiles);
            if (command == Command.Up || command == Command.Down)
            {
                if (robotTarget.IsRightBox)
                {
                    //left box has to be able to move as well
                    var attachedBox = tiles.FirstOrDefault(x => x.Position == PositionOffset(robotTarget.Position, Command.Left));
                    var attachedResult = CanMove(command, attachedBox, tiles);
                    pushAnalysis.canMove &= attachedResult.canMove;
                    pushAnalysis.tiles.AddRange(attachedResult.tiles);
                }
                else if (robotTarget.IsLeftBox)
                {
                    var attachedBox = tiles.FirstOrDefault(x => x.Position == PositionOffset(robotTarget.Position, Command.Right));
                    var attachedResult = CanMove(command, attachedBox, tiles);
                    pushAnalysis.canMove &= attachedResult.canMove;
                    pushAnalysis.tiles.AddRange(attachedResult.tiles);
                }
            }


            if (!pushAnalysis.canMove)
            {
                return;
            }

            Dictionary<Tile, Tile> newSettings = new();
            foreach (var tile in pushAnalysis.tiles.Distinct())
            {
                var offsetTile = tiles.FirstOrDefault(x => x.Position == PositionOffset(tile.Position, command));
                newSettings[offsetTile] = new Tile()
                {
                    HasBox = true,
                    IsLeftBox = tile.IsLeftBox,
                    IsRightBox = tile.IsRightBox,
                };
                tile.HasBox = false;
                tile.IsLeftBox = false;
                tile.IsRightBox = false;
            }

            foreach (var tile in newSettings.Keys)
            {
                var newTile = newSettings[tile];
                tile.HasBox = newTile.HasBox;
                tile.IsLeftBox = newTile.IsLeftBox;
                tile.IsRightBox = newTile.IsRightBox;
            }
        }

        robot.HasRobot = false;
        robotTarget.HasRobot = true;

    }

    private void Display(List<Tile> tiles, bool forceAll = false)
    {
        //only redraw the tiles around the robot
        var robot = tiles.FirstOrDefault(x => x.HasRobot);

        foreach (var tile in tiles.Where(x => forceAll
        || x.Position.Col == robot.Position.Col - 1
        || x.Position.Col == robot.Position.Col
        || x.Position.Col == robot.Position.Col + 1
        || x.Position.Row == robot.Position.Row - 1
        || x.Position.Row == robot.Position.Row
        || x.Position.Row == robot.Position.Row + 1).ToList())
        {
            Console.SetCursorPosition(tile.Position.Col, tile.Position.Row);
            Console.Write(tile.HasRobot ? '@' : tile.HasBox ? 'O' : tile.IsWall ? '#' : '.');
        }

    }

    public override long Part1(string[] input)
    {
        var (tiles, commands) = ParseInput(input);
        var result = 0;
        //Console.CursorVisible = false;
        //Console.Clear();
        //Display(tiles,true);

        foreach (var command in commands)
        {
            Step(command, tiles);
            //Display(tiles);
        }

        result = tiles.Where(x => x.HasBox).Sum(tile => tile.Position.Col + tile.Position.Row * 100);

        return result;
    }

    public override long Part2(string[] input)
    {
        var (tiles, commands) = ParseWideInput(input);
        var result = 0;
        //Console.CursorVisible = false;
        //Console.Clear();
        //Display(tiles, true);

        foreach (var command in commands)
        {
            Step(command, tiles);
            //Display(tiles);
        }

        result = tiles.Where(x => x.IsLeftBox).Sum(tile => tile.Position.Col + tile.Position.Row * 100);

        return result;
    }
}
