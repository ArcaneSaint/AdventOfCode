using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers.Year2024;

internal class Day19Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2024<long>(19, part1Test, part2Test)
{
    private Dictionary<string, long> cache = new Dictionary<string, long>();

    private bool CanMatch(string remaining, ICollection<string> patterns)
    {
        if (String.IsNullOrWhiteSpace(remaining))
        {
            return true;
        }

        //var startings = patterns.Where(x => remaining.StartsWith(x)).ToList();
        var endings = patterns.Where(x => remaining.EndsWith(x)).ToList();

        foreach (var item in endings)
        {
            if (CanMatch(remaining[..^item.Length], patterns))
            {
                return true;
            }
        }

        return false;
    }

    private long CountPossibleWays(string remaining, Dictionary<string, long> towelMap)
    {
        //get biggest subTowel:
        if (String.IsNullOrWhiteSpace(remaining))
        {
            return 1;
        }
        var subTowel = towelMap.Keys.Where(key => remaining.EndsWith(key)).OrderByDescending(x => x.Length).FirstOrDefault();

        var leftResult = towelMap[subTowel];
        var rightResult = CountPossibleWays(remaining[..^subTowel.Length], towelMap);


        return leftResult * rightResult;
    }


    private long CountPossibleWays(string remaining, ICollection<string> patterns)
    {
        if (cache.TryGetValue(remaining, out var result))
        {
            return result;
        }
        if (String.IsNullOrWhiteSpace(remaining))
        {
            return 1;
        }

        var endings = patterns.Where(x => remaining.EndsWith(x)).ToList();

        var total = endings.Sum(item => CountPossibleWays(remaining[..^item.Length], patterns));

        cache[remaining] = total;
        return total;
    }

    public override long Part1(string[] input)
    {
        var patterns = input[0].Split(',').Select(x => x.Trim()).ToList();

        var data = input[2..];
        var result = 0;

        foreach (var item in data)
        {
            if (CanMatch(item, patterns))
            {
                result++;
            }
        }


        return result;
    }

    public override long Part2(string[] input)
    {
        cache = new Dictionary<string, long>();
        var towels = input[0].Split(',').Select(x => x.Trim()).ToList();

        var data = input[2..];
        var result = 0L;
        var toCount = data.Where(x => CanMatch(x, towels)).ToList();

        Dictionary<string, long> map = new Dictionary<string, long>();

        foreach (var item in toCount)
        {
            result += CountPossibleWays(item, towels);
        }

        return result;
    }
}
