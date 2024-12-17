namespace AdventOfCode.Solvers.Year2024;

internal class Day12Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2024<long>(12, part1Test, part2Test)
{
    [Flags]
    enum Direction
    {
        None = 0,
        North = 0b0001,
        East = 0b0010,
        South = 0b0100,
        West = 0b1000
    }

    struct Tile
    {
        public int Edges { get; set; }
        public Direction Fence { get; set; }

        public int NorthSideId;
        public int EastSideId;
        public int SouthSideId;
        public int WestSideId;

        public string SideId => $"-{NorthSideId}-{EastSideId}-{SouthSideId}-{WestSideId}-";

        public int Row { get; set; }
        public int Col { get; set; }
        public char Identifier { get; set; }
        public int ZoneId { get; set; }
    }

    private Tile[,] ParseInput(string[] input)
    {
        var result = new Tile[input.Length, input[0].Length];

        for (int i = 0; i < input.Length; ++i)
        {
            for (int j = 0; j < input[i].Length; ++j)
            {
                result[i, j] = new Tile()
                {
                    Row = i,
                    Col = j,
                    Edges =
                          (i == 0 ? 1 : 0)
                        + (j == 0 ? 1 : 0)
                        + (i == input.Length - 1 ? 1 : 0)
                        + (j == input[i].Length - 1 ? 1 : 0),
                    Fence =
                          (i == 0 ? Direction.North : Direction.None)
                        | (j == 0 ? Direction.West : Direction.None)
                        | (i == input.Length - 1 ? Direction.South : Direction.None)
                        | (j == input[i].Length - 1 ? Direction.East : Direction.None),
                    Identifier = input[i][j]
                };
            }
        }

        return result;
    }

    private Tile[,] FillArea(Tile starting, Tile[,] map)
    {
        var frontier = new Queue<Tile>();
        frontier.Enqueue(starting);

        while (frontier.TryDequeue(out var tile))
        {
            ((int Row, int Col) position, Direction direction)[] coordinates = [
             ((Row: tile.Row, Col: tile.Col - 1), Direction.West),
             ((Row: tile.Row, Col: tile.Col + 1), Direction.East),
             ((Row: tile.Row - 1, Col: tile.Col), Direction.North),
             ((Row: tile.Row + 1, Col: tile.Col), Direction.South)];

            foreach (var coordinate in coordinates)
            {
                if (map.IsInBounds(coordinate.position) && map[coordinate.position.Row, coordinate.position.Col] is { } item)
                {
                    if (item.Identifier == tile.Identifier)
                    {
                        if (item.ZoneId != tile.ZoneId)
                        {
                            item.ZoneId = tile.ZoneId;
                            map[coordinate.position.Row, coordinate.position.Col] = item;
                            frontier.Enqueue(item);
                        }
                    }
                    else
                    {
                        //adjacent to other
                        tile.Edges += 1;
                        tile.Fence |= coordinate.direction;
                        map[tile.Row, tile.Col] = tile;
                    }
                }
            }

        }

        return map;
    }

    private void Display(Tile[,] map)
    {
        for (int i = 0; i < map.GetLength(0) && i < Console.WindowHeight; ++i)
        {
            for (int j = 0; j < map.GetLength(1) && j < Console.WindowWidth; ++j)
            {
                Console.SetCursorPosition(j, i);
                var item = map[i, j];
                ConsoleColor color = ConsoleColor.DarkGray;
                if (item.Edges > 0 && item.ZoneId > 0)
                {
                    color = ConsoleColor.Red;
                }
                else if (item.ZoneId > 0)
                {
                    color = ConsoleColor.White;
                }
                var output = '.';

                if (item.Fence == Direction.None)
                {

                }
                else if (item.Fence == Direction.North)
                {
                    //output = '┬';
                    output = '─';
                }
                else if (item.Fence == Direction.East)
                {
                    //output = '┤';
                    output = '│';
                }
                else if (item.Fence == Direction.South)
                {
                    //output = '┴';
                    output = '─';
                }
                else if (item.Fence == Direction.West)
                {
                    //output = '├';
                    output = '│';
                }
                else if (item.Fence == (Direction.North | Direction.East))
                {
                    output = '┐';
                }
                else if (item.Fence == (Direction.South | Direction.East))
                {
                    output = '┘';
                }
                else if (item.Fence == (Direction.North | Direction.West))
                {
                    output = '┌';
                }
                else if (item.Fence == (Direction.South | Direction.West))
                {
                    output = '└';
                }
                else
                {
                    output = '·';
                }



                using (new ColorOutputter(color))
                {
                    Console.Write(output);
                }
            }
        }
    }

    private void PrepareDisplay()
    {
        Console.Clear();
        Console.CursorVisible = false;
    }

    public override long Part1(string[] input)
    {
        var map = ParseInput(input);
        var zoneId = 1;

        for (int i = 0; i < map.GetLength(0); ++i)
        {
            for (int j = 0; j < map.GetLength(1); ++j)
            {
                if (map[i, j].ZoneId == 0)
                {
                    map[i, j].ZoneId = zoneId++;
                    FillArea(map[i, j], map);
                }
            }
        }


        return map.Cast<Tile>()
            .GroupBy(t => t.ZoneId)
            .Select(group => group.Count() * group.Sum(t => t.Edges))
            .Sum();
    }


    private Tile[,] FillSideId(Tile starting, Direction direction, Tile[,] map)
    {
        var frontier = new Queue<Tile>();
        frontier.Enqueue(starting);
        /*
            ((int Row, int Col) position, Direction direction)[] coordinates = [
             ((Row: tile.Row, Col: tile.Col - 1), Direction.West),
             ((Row: tile.Row, Col: tile.Col + 1), Direction.East),
             ((Row: tile.Row - 1, Col: tile.Col), Direction.North),
             ((Row: tile.Row + 1, Col: tile.Col), Direction.South)];*/
        while (frontier.TryDequeue(out var tile))
        {
            (int Row, int Col)[] coordinates = [];
            switch (direction)
            {
                case Direction.North:
                    coordinates = [
                 (Row: tile.Row , Col: tile.Col-1),
                 (Row: tile.Row , Col: tile.Col+1)];
                    break;
                case Direction.East:
                    coordinates = [
                 (Row: tile.Row-1, Col: tile.Col),
                 (Row: tile.Row+1, Col: tile.Col)];
                    break;
                case Direction.South:
                    coordinates = [
                 (Row: tile.Row , Col: tile.Col-1),
                 (Row: tile.Row , Col: tile.Col+1)];
                    break;
                case Direction.West:
                    coordinates = [
                     (Row: tile.Row-1, Col: tile.Col),
                     (Row: tile.Row+1, Col: tile.Col)];
                    break;
            }


            foreach (var coordinate in coordinates)
            {
                if (map.IsInBounds(coordinate) && map[coordinate.Row, coordinate.Col] is { } item && item.ZoneId == tile.ZoneId)
                {
                    if (item.Fence.HasFlag(direction))
                    {
                        switch (direction)
                        {
                            case Direction.North:
                                if (item.NorthSideId == 0)
                                {
                                    item.NorthSideId = tile.NorthSideId;
                                    map[item.Row, item.Col] = item;
                                    frontier.Enqueue(item);
                                }
                                break;
                            case Direction.East:
                                if (item.EastSideId == 0)
                                {
                                    item.EastSideId = tile.EastSideId;
                                    map[item.Row, item.Col] = item;
                                    frontier.Enqueue(item);
                                }
                                break;
                            case Direction.South:
                                if (item.SouthSideId == 0)
                                {
                                    item.SouthSideId = tile.SouthSideId;
                                    map[item.Row, item.Col] = item;
                                    frontier.Enqueue(item);
                                }
                                break;
                            case Direction.West:
                                if (item.WestSideId == 0)
                                {
                                    item.WestSideId = tile.WestSideId;
                                    map[item.Row, item.Col] = item;
                                    frontier.Enqueue(item);
                                }
                                break;
                        }

                    }
                }
            }

        }

        return map;
    }

    public override long Part2(string[] input)
    {
        var map = ParseInput(input);
        var zoneId = 1;
        var sideId = 1;

        for (int i = 0; i < map.GetLength(0); ++i)
        {
            for (int j = 0; j < map.GetLength(1); ++j)
            {
                if (map[i, j].ZoneId == 0)
                {
                    map[i, j].ZoneId = zoneId++;
                    FillArea(map[i, j], map);
                }
            }
        }


        for (int i = 0; i < map.GetLength(0); ++i)
        {
            for (int j = 0; j < map.GetLength(1); ++j)
            {
                if (map[i, j].Fence.HasFlag(Direction.North) && map[i, j].NorthSideId == 0)
                {
                    map[i, j].NorthSideId = sideId++;
                    FillSideId(map[i, j], Direction.North, map);
                }
                if (map[i, j].Fence.HasFlag(Direction.East) && map[i, j].EastSideId == 0)
                {
                    map[i, j].EastSideId = sideId++;
                    FillSideId(map[i, j], Direction.East, map);
                }
                if (map[i, j].Fence.HasFlag(Direction.South) && map[i, j].SouthSideId == 0)
                {
                    map[i, j].SouthSideId = sideId++;
                    FillSideId(map[i, j], Direction.South, map);
                }
                if (map[i, j].Fence.HasFlag(Direction.West) && map[i, j].WestSideId == 0)
                {
                    map[i, j].WestSideId = sideId++;
                    FillSideId(map[i, j], Direction.West, map);
                }
            }
        }


        return map.Cast<Tile>()
            .GroupBy(t => t.ZoneId)
            .Select(group => group.Count() *
            (
                group.Where(t => t.NorthSideId != 0).GroupBy(t => t.NorthSideId).Count()
                + group.Where(t => t.EastSideId != 0).GroupBy(t => t.EastSideId).Count()
                + group.Where(t => t.SouthSideId != 0).GroupBy(t => t.SouthSideId).Count()
                + group.Where(t => t.WestSideId != 0).GroupBy(t => t.WestSideId).Count()
            ))
            .Sum();
    }
}
