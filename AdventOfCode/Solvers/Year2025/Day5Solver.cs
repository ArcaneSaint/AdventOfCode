using System.Security.Cryptography;

namespace AdventOfCode.Solvers.Year2025;

internal class Day5Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2025<long>(5, part1Test, part2Test)
{
    public override long Part1(string[] input)
    {
        var ranges = new List<(long start, long end)>();
        var result = 0;
        int i = 0;
        for (; !String.IsNullOrWhiteSpace(input[i]); ++i)
        {
            ranges.Add(new(input[i].Split('-')[0].ToLong(), input[i].Split('-')[1].ToLong()));
        }

        ++i;
        for (; i < input.Length; ++i)
        {
            var n = input[i].ToLong();
            if (ranges.Any(range => range.start <= n && range.end >= n))
            {
                ++result;
            }
        }
        return result;
    }

    public override long Part2(string[] input)
    {
        var ranges = new List<(long start, long end)>();
        var result = 0;
        for (var i = 0; !String.IsNullOrWhiteSpace(input[i]); ++i)
        {
            ranges.Add(new(input[i].Split('-')[0].ToLong(), input[i].Split('-')[1].ToLong()));
        }
        ranges = ranges.OrderBy(r => r.start).ToList();

        var totalRanges = ranges.Count;
        do
        {
            totalRanges = ranges.Count;
            ranges = MergeRanges(ranges);
        } while (ranges.Count != totalRanges);

        return ranges.Sum(r => r.end - r.start + 1);
    }

    private List<(long start, long end)> MergeRanges(List<(long start, long end)> ranges)
    {
        var newRanges = new List<(long start, long end)>();

        for (var i = 0; i < ranges.Count; ++i)
        {
            var r1 = ranges[i];
            var newStart = r1.start;
            var newEnd = r1.end;
            for (var j = 0; j < ranges.Count; ++j)
            {
                var r2 = ranges[j];
                //other range starts within current, so join together
                if (r2.start >= r1.start && r2.start <= newEnd)
                {
                    newEnd = Math.Max(newEnd, r2.end); //biggest end is our end
                    i = j; //skip to here in the outer loop as well, so we don't add this one again
                }
            }
            newRanges.Add((newStart, newEnd));
        }

        newRanges = newRanges.OrderBy(r => r.start).ToList();
        return newRanges;
    }
}