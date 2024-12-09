using System.Reflection;

namespace AdventOfCode.Solvers.Year2024;

internal class Day9Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2024(9, part1Test, part2Test)
{
    public override long Part1(string[] input)
    {
        var data = BuildDisk(input[0]);
        //Render(data);
        var endIndex = data.Length - 1;
        for (var index = 0; index < endIndex; ++index)
        {
            if (data[index].IsEmpty)
            {
                (data[index], data[endIndex]) = (data[endIndex], data[index]);
                while (data[endIndex].IsEmpty && endIndex > index)
                {
                    --endIndex;
                }

                //Render(data);
            }
        }


        return GetChecksum(data);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "For debugging purposes")]
    private static void Render(DiskPosition[] data)
    {
        for (var index = 0; index < data.Length; ++index)
        {
            if (data[index].IsEmpty)
            {
                Console.Write('.');
            }
            else
            {
                Console.Write(data[index].Id);
            }
        }
        Console.WriteLine();
    }

    private static long GetChecksum(DiskPosition[] data)
    {
        var result = 0L;
        for (var index = 0; !data[index].IsEmpty; ++index)
        {
            result += data[index].Id * index;
        }

        return result;
    }

    struct DiskPosition
    {
        public int Id { get; set; }
        public bool IsEmpty { get; set; }

    }

    static DiskPosition[] BuildDisk(string diskMap)
    {
        var result = new List<DiskPosition>();

        for (var index = 0; index < diskMap.Length; ++index)
        {
            var count = diskMap[index] - '0';
            if (index % 2 == 0)
            {
                result.AddRange(Enumerable.Range(0, count).Select(x => new DiskPosition() { Id = index / 2, IsEmpty = false }));
            }
            else
            {
                result.AddRange(Enumerable.Range(0, count).Select(x => new DiskPosition() { IsEmpty = true }));
            }
        }

        return [..result];
    }

    public override long Part2(string[] input)
    {
        return 0;
    }
}
