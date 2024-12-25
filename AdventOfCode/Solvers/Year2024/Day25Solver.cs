namespace AdventOfCode.Solvers.Year2024;

internal class Day25Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2024<long>(25, part1Test, part2Test)
{

    private (List<List<int>> locks, List<List<int>> keys) ParseInput(string[] input)
    {
        var isLock = false;
        var isKey = false;
        var isNew = true;
        var keys = new List<List<int>>();
        var locks = new List<List<int>>();
        var current = new List<int>() { -1, -1, -1, -1, -1 };



        foreach (var line in input)
        {
            if (String.IsNullOrWhiteSpace(line))
            {
                isNew = true;
                if (isLock)
                {
                    locks.Add(current);
                }
                else
                {
                    keys.Add(current);
                }
                current = new List<int>() { -1, -1, -1, -1, -1 };
            }
            else
            {
                if (isNew)
                {
                    if (line == "#####")
                    {
                        (isLock, isKey) = (true, false);
                    }
                    else
                    {
                        (isLock, isKey) = (false, true);
                    }
                    isNew = false;
                }
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == '#')
                    {
                        current[i]++;
                    }
                }
            }

        }

        if (isLock)
        {
            locks.Add(current);
        }
        else
        {
            keys.Add(current);
        }


        return (locks, keys);
    }

    /*#####
.####
.####
.####
.#.#.
.#...
.....

#####
##.##
.#.##
...##
...#.
...#.
.....
*/
    public override long Part1(string[] input)
    {
        var (locks, keys) = ParseInput(input);

        var result = 0;

        foreach (var @lock in locks)
        {
            foreach (var key in keys)
            {
                var isOk = true;
                for (var i = 0; i < key.Count; ++i)
                {
                    if(key[i] + @lock[i] > 5)
                    {
                        isOk = false;
                    }
                }
                if (isOk)
                {
                    result++;
                }
            }
        }

        return result;
    }

    public override long Part2(string[] input)
    {
        return 0;
    }
}
