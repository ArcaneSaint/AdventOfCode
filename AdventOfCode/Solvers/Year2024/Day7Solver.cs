using System.Linq;

namespace AdventOfCode.Solvers.Year2024;

internal class Day7Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2024<long>(7, part1Test, part2Test)
{

    public override long Part1(string[] input)
    {
        var results = 0L;
        foreach (var line in input)
        {
            var (target, operands) = ParseLine(line);

            if (TryOperators(target, operands[1..], operands[0]) is { Item1: true } success)
            {
                results += target;
            }

        }


        return results;
    }

    private static (long target, List<long> operands) ParseLine(string line)
    {
        var data = line.Split(':');
        return (data[0].ToLong(), data[1].Trim().Split(' ').Select(long.Parse).ToList());
    }

    private static (bool, string) TryOperators(long target, List<long> remaining, long current)
    {
        if (current == target && remaining.Count == 0)
        {
            return (true, $"");
        }
        if (current > target || remaining.Count == 0)
        {
            return (false, $"");
        }

        if (TryOperators(target, remaining[1..], current * remaining[0]) is { Item1: true } successMul)
        {
            return (true, $" * {remaining[0]}){successMul.Item2}");
        }
        else if (TryOperators(target, remaining[1..], current + remaining[0]) is { Item1: true } successSum)
        {
            return (true, $" + {remaining[0]}){successSum.Item2}");
        }
        return (false, $"{remaining[0]}");
    }

    private static (bool, string) TryOperatorsWithConcat(long target, List<long> remaining, long current)
    {
        if (current == target && remaining.Count == 0)
        {
            return (true, $"");
        }
        if (current > target || remaining.Count == 0)
        {
            return (false, $"");
        }

        if (TryOperatorsWithConcat(target, remaining[1..], current * remaining[0]) is { Item1: true } successMul)
        {
            return (true, $" * {remaining[0]}){successMul.Item2}");
        }
        else if (TryOperatorsWithConcat(target, remaining[1..], current + remaining[0]) is { Item1: true } successSum)
        {
            return (true, $" + {remaining[0]}){successSum.Item2}");
        }
        else if (TryOperatorsWithConcat(target, remaining[1..], $"{current}{remaining[0]}".ToLong()) is { Item1: true } successConcat)
        {
            return (true, $" || {remaining[0]}){successConcat.Item2}");
        }
        return (false, $"{remaining[0]}");
    }

    public override long Part2(string[] input)
    {
        var results = 0L;

        Parallel.ForEach(input, (line, state) => {
            var (target, operands) = ParseLine(line);

            if (TryOperatorsWithConcat(target, operands[1..], operands[0]) is { Item1: true } success)
            {
                results += target;
            }
        });

        //foreach (var line in input)
        //{

        //    var (target, operands) = ParseLine(line);

        //    if (TryOperatorsWithConcat(target, operands[1..], operands[0]) is { Item1: true } success)
        //    {
        //        results += target;
        //    }
        //}


        return results;
    }
}
