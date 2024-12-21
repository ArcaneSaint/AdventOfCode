using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers.Year2024;

internal class Day14Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2024<long>(14, part1Test, part2Test)
{
    struct Robot
    {
        public (int X, int Y) Position { get; set; }
        public (int dX, int dY) Velocity { get; set; }
    }

    Robot ParseRobot(string input)
    {
        var line1 = Regex.Match(input, @"p=(\d+),(\d+) v=(-?\d+),(-?\d+)");

        return new Robot()
        {
            Position = new(line1.Groups[1].Value.ToInt(), line1.Groups[2].Value.ToInt()),
            Velocity = new(line1.Groups[3].Value.ToInt(), line1.Groups[4].Value.ToInt()),
        };
    }

    List<Robot> ParseInput(string[] input)
    {
        var result = new List<Robot>();
        for (int i = 0; i < input.Length; i++)
        {
            result.Add(ParseRobot(input[i]));
        }
        return result;
    }


    public override long Part1(string[] input)
    {
        var width = 101;
        var height = 103;
        //test case
        if (input.Count() == 12)
        {
            width = 11;
            height = 7;
        }

        var middle = (X: width / 2, Y: height / 2);
        var quadrants = (First: 0, Second: 0, Third: 0, Fourth: 0);

        var robots = ParseInput(input);

        var results = new int[height, width];

        foreach (var robot in robots)
        {
            var endPos = (X: (robot.Position.X + robot.Velocity.dX * 100), Y: (robot.Position.Y + robot.Velocity.dY * 100));
            var wrappedX = Wrap(endPos.X, width);
            var wrappedY = Wrap(endPos.Y, height);
            if (wrappedX < middle.X && wrappedY < middle.Y)
            {
                quadrants.First++;
            }
            else if (wrappedX < middle.X && wrappedY > middle.Y)
            {
                quadrants.Second++;
            }
            else if (wrappedX > middle.X && wrappedY < middle.Y)
            {
                quadrants.Third++;
            }
            else if (wrappedX > middle.X && wrappedY > middle.Y)
            {
                quadrants.Fourth++;
            }
            results[wrappedY, wrappedX]++;
        }




        return quadrants.First * quadrants.Second * quadrants.Third * quadrants.Fourth;
    }
    private void Display(int[,] results)
    {

        for (int i = 0; i < results.GetLength(0) && i < Console.WindowHeight; ++i)
        {
            for (int j = 0; j < results.GetLength(1); ++j)
            {
                Console.SetCursorPosition(j, i);
                Console.Write(results[i, j] == 0 ? ' ' : '#');
            }
        }
    }
    private long Wrap(long pos, long max)
    {
        var wrapped = pos % max;
        if (wrapped < 0)
        {
            return max + wrapped;
        }
        return wrapped;
    }

    public override long Part2(string[] input)
    {
        var width = 101;
        var height = 103;
        //test case
        if (input.Count() == 12)
        {
            return 0;
        }

        var robots = ParseInput(input);
        long seconds = 1;

        var hasATree = false;
        while (!hasATree)
        {
            seconds++;

            var results = new int[height, width];

            foreach (var robot in robots)
            {
                var endPos = (X: (robot.Position.X + robot.Velocity.dX * seconds), Y: (robot.Position.Y + robot.Velocity.dY * seconds));
                var wrappedX = Wrap(endPos.X, width);
                var wrappedY = Wrap(endPos.Y, height);
                results[wrappedY, wrappedX]++;
            }

            hasATree = HasATree(results);
        }



        return seconds;
    }

    private bool HasATree(int[,] data)
    {
        var fullLines = 0;
        var partialLines = 0;

        var fullColumns = 0;
        var partialColumns = 0;

        for (int row = 0; row < data.GetLength(0); ++row)
        {
            int sum = 0;
            for (int col = 0; col < data.GetLength(1); ++col)
            {
                sum += data[row, col];
            }
            if (sum >= 31)
            {
                ++fullLines;
            }
            else if (sum >= 9)
            {
                ++partialLines;
            }
        }

        for (int col = 0; col < data.GetLength(1); ++col)
        {
            int sum = 0;
            for (int row = 0; row < data.GetLength(0); ++row)
            {
                sum += data[row, col];
            }

            if (sum >= 33)
            {
                ++fullColumns;
            }
            else if (sum >= 23)
            {
                ++partialColumns;
            }
        }

        if (fullLines == 2 && partialLines > 10 && fullColumns == 2 && partialColumns == 3)
        {
            return true;
        }
        return false;
    }
}
