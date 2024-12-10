using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode.Solvers.Year2024;

internal class Day10Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2024(10, part1Test, part2Test)
{

    public override long Part1(string[] input)
    {
        var data = ParseInput(input);
        var results = 0;

        for (var i = 0; i < data.GetLength(0); ++i)
        {
            for (var j = 0; j < data.GetLength(1); ++j)
            {
                var item = data[i, j];
                if (item == 0)
                {
                    results += ValidPaths(i, j, data);
                }
            }
        }
        return results;
    }

    private int ValidPaths(int i, int j, int[,] data)
    {
        DisplayGrid(data);
        var currentHeight = data[i, j];
        var results = 0;

        var frontier = new List<(int i, int j)> { (i, j) };

        while (frontier.Any() && currentHeight < 9)
        {
            var targetHeight = currentHeight + 1;

            var next = new List<(int i, int j)>();

            foreach (var item in frontier)
            {
                var up = (i: item.i - 1, j: item.j);
                var down = (i: item.i + 1, j: item.j);
                var left = (i: item.i, j: item.j - 1);
                var right = (i: item.i, j: item.j + 1);

                if (IsInBounds(up, data) && data[up.i, up.j] == targetHeight)
                {
                    next.Add(up);
                }
                if (IsInBounds(down, data) && data[down.i, down.j] == targetHeight)
                {
                    next.Add(down);
                }
                if (IsInBounds(left, data) && data[left.i, left.j] == targetHeight)
                {
                    next.Add(left);
                }
                if (IsInBounds(right, data) && data[right.i, right.j] == targetHeight)
                {
                    next.Add(right);
                }
            }


            currentHeight = targetHeight;
            frontier = next.Distinct().ToList();
            //Display(frontier);
            DisplayMarks(frontier);
            if (targetHeight == 9)
            {
                results += frontier.Count;
            }



        }

        return results;
    }

    private void DisplayGrid(int[,] data)
    {
        Console.Clear();
        for (var i = 0; i < data.GetLength(0); ++i)
        {
            for (var j = 0; j < data.GetLength(1); ++j)
            {
                Console.Write(data[i,j]);
            }

            Console.WriteLine();
        }

    }

    private void DisplayMarks(List<(int i, int j)> frontier)
    {
        foreach (var item in frontier)
        {
            Console.SetCursorPosition(item.j, item.i);
            Console.Write('X');
        }
    }

    private void Display(List<(int i, int j)> next)
    {
        foreach (var item in next)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine("----");
    }

    private static bool IsInBounds((int i, int j) up, int[,] data)
    {
        return up.i >= 0
               && up.j >= 0
               && up.i < data.GetLength(0)
               && up.j < data.GetLength(1);
    }


    public override long Part2(string[] input)
    {
        return 0;
    }

    private static int[,] ParseInput(string[] input)
    {
        var result = new int[input.Length, input[0].Length];

        for (var i = 0; i < input.Length; ++i)
        {
            for (var j = 0; j < input[i].Length; ++j)
            {
                result[i, j] = input[i][j] - '0';
            }
        }


        return result;
    }
}
