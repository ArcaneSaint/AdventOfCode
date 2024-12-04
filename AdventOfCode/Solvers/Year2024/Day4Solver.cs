namespace AdventOfCode.Solvers.Year2024;

internal class Day4Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2024(4, part1Test, part2Test)
{
    public override long Part1(string[] input)
    {
        var data = new char[input.Length, input[0].Length];

        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                data[i, j] = input[i][j];
            }
        }

        var results = 0;
        for (int i = 0; i < data.GetLength(0); i++)
        {
            for (int j = 0; j < data.GetLength(1); j++)
            {

                if (Check(data, i, j, (-1, -1), 'X','M','A','S'))
                {
                    results++;
                }
                if (Check(data, i, j, (-1, 0), 'X', 'M', 'A', 'S'))
                {
                    results++;
                }
                if (Check(data, i, j, (-1, 1), 'X', 'M', 'A', 'S'))
                {
                    results++;
                }


                if (Check(data, i, j, (0, -1), 'X', 'M', 'A', 'S'))
                {
                    results++;
                }
                if (Check(data, i, j, (0, 1), 'X', 'M', 'A', 'S'))
                {
                    results++;
                }


                if (Check(data, i, j, (1, -1), 'X', 'M', 'A', 'S'))
                {
                    results++;
                }
                if (Check(data, i, j, (1, 0), 'X', 'M', 'A', 'S'))
                {
                    results++;
                }
                if (Check(data, i, j, (1, 1), 'X', 'M', 'A', 'S'))
                {
                    results++;
                }




            }
        }


        return results;

    }

    private bool CheckData(char[,] data, int i, int j, (int x, int y) direction)
    {
        var (x, y) = direction;
        if (data[i, j] == 'X')
        {
            var (nextX, nextY) = (i + x, j + y);
            if (nextX < data.GetLength(0)
                && nextY < data.GetLength(1)
                && nextX >= 0
                && nextY >= 0
                && data[nextX, nextY] == 'M')
            {
                (nextX, nextY) = (nextX + x, nextY + y);
                if (nextX < data.GetLength(0)
                    && nextY < data.GetLength(1)
                    && nextX >= 0
                    && nextY >= 0
                    && data[nextX, nextY] == 'A')
                {
                    (nextX, nextY) = (nextX + x, nextY + y);
                    if (nextX < data.GetLength(0)
                        && nextY < data.GetLength(1)
                        && nextX >= 0
                        && nextY >= 0
                        && data[nextX, nextY] == 'S')
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private bool Check(char[,] data, int i, int j, (int x, int y) direction, params char[] leftToCheck)
    {
        if (!leftToCheck.Any())
        {
            return true;
        }

        if (i >= 0
            && j >= 0
            && i < data.GetLength(0)
            && j < data.GetLength(1)
            && data[i, j] == leftToCheck[0])
        {
            return Check(data, i + direction.x, j + direction.y, direction, leftToCheck[1..]);
        }

        return false;
    }

    public override long Part2(string[] input)
    {
        throw new NotImplementedException();
    }
}
