namespace AdventOfCode.Solvers.Year2024;

internal class Day9Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2024<long>(9, part1Test, part2Test)
{
    struct DiskPosition
    {
        public int Id { get; set; }
        public int Size { get; set; }
        public bool IsEmpty { get; set; }
    }


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
        for (var index = 0; index < data.Length; ++index)
        {
            result += data[index].Id * index;
        }

        return result;
    }

    private static DiskPosition[] ExpandFiles(List<DiskPosition> files)
    {
        return [..files.SelectMany(f => Enumerable.Range(0, f.Size).Select(x => new DiskPosition
        {
            Id = f.Id,
            IsEmpty = f.IsEmpty,
        }))];
    }

    private static DiskPosition[] BuildDisk(string diskMap)
    {
        var result = new List<DiskPosition>();

        for (var index = 0; index < diskMap.Length; ++index)
        {
            var count = diskMap[index] - '0';
            if (index % 2 == 0)
            {
                result.AddRange(Enumerable.Range(0, count).Select(x => new DiskPosition() { Id = index / 2, IsEmpty = false, Size = 1 }));
            }
            else
            {
                result.AddRange(Enumerable.Range(0, count).Select(x => new DiskPosition() { IsEmpty = true, Size = 1 }));
            }
        }

        return [.. result];
    }

    private static List<DiskPosition> BuildFileDisk(string diskMap)
    {
        var result = new List<DiskPosition>();

        for (var index = 0; index < diskMap.Length; ++index)
        {
            var count = diskMap[index] - '0';
            if (index % 2 == 0)
            {
                result.Add(new DiskPosition() { Id = index / 2, IsEmpty = false, Size = count });
            }
            else
            {
                result.Add(new DiskPosition() { IsEmpty = true, Size = count });
            }
        }

        return result;
    }

    private void MoveToFront(int endIndex, List<DiskPosition> data)
    {
        for (var i = 0; i < endIndex; ++i)
        {
            if (data[i].IsEmpty && data[i].Size >= data[endIndex].Size)
            {
                var empty = data[i];
                var newSize = data[i].Size - data[endIndex].Size;
                empty.Size = newSize;
                data[i] = empty;

                var toMove = data[endIndex];

                //data//.RemoveAt(endIndex);
                data[endIndex] = new DiskPosition()
                {
                    IsEmpty = true,
                    Size = toMove.Size,
                };
                data.Insert(i, toMove);
                return;
            }
        }
    }

    public override long Part2(string[] input)
    {
        var data = BuildFileDisk(input[0]);
        // var startIndex = 0;
        //Render(ExpandFiles(data));
        for (var endIndex = data.Count - 1; endIndex > 0; --endIndex)
        {
            if (!data[endIndex].IsEmpty)
            {
                MoveToFront(endIndex, data);
                //Render(ExpandFiles(data));
            }
        }


        return GetChecksum(ExpandFiles(data));
    }
}
