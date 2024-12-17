using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers.Year2024;

internal class Day5Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2024<long>(5, part1Test, part2Test)
{
    public override long Part1(string[] input)
    {
        var correctUpdates = new List<string[]>();
        var rules = new List<(string Lesser, string Greater)>();
        var result = 0;

        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (Regex.Match(line, @"(\d+)\|(\d+)") is { Success: true } match)
            {
                rules.Add((match.Groups[1].Value, match.Groups[2].Value));
            }
            else
            {
                if (IsValid(line, rules))
                {
                    correctUpdates.Add(line.Split(","));
                }
            }
        }

        result = correctUpdates.Select(update => update[update.Length / 2].ToInt()).Sum();

        return result;

    }

    public override long Part2(string[] input)
    {
        var inCorrectUpdates = new List<List<string>>();
        var rules = new List<(string Lesser, string Greater)>();
        var result = 0;

        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (Regex.Match(line, @"(\d+)\|(\d+)") is { Success: true } match)
            {
                rules.Add((match.Groups[1].Value, match.Groups[2].Value));
            }
            else
            {
                if (!IsValid(line, rules))
                {
                    inCorrectUpdates.Add(line.Split(",").ToList());
                }
            }
        }

        var correctedUpdates = new List<List<string>>();

        Parallel.ForEach(inCorrectUpdates, inCorrectUpdate =>
        {
            var corrected = new List<string>();

            foreach (var value in inCorrectUpdate)
            {
                NextStep(value, corrected, rules);
            }

            correctedUpdates.Add(corrected);
        });

        result = correctedUpdates.Select(update => update[update.Count() / 2].ToInt()).Sum();

        return result;
    }

    private void NextStep(string value, List<string> corrected, List<(string Lesser, string Greater)> rules)
    {
        for (int i = 0; i <= corrected.Count; i++)
        {
            corrected.Insert(i, value);
            if (IsValid(corrected, rules))
            {
                return;
            }
            else
            {
                corrected.RemoveAt(i);
            }
        }
    }

    private bool IsValid(string line, List<(string Lesser, string Greater)> rules)
    {
        return IsValid(line.Split(",").ToList(), rules);
    }
    private bool IsValid(List<string> values, List<(string Lesser, string Greater)> rules)
    {
        for(int i = 0; i < values.Count; ++i)
        {
            var page = values[i];
            if(!values[..i].TrueForAll(pastValue => !rules.Any(rule=>rule.Greater == pastValue && rule.Lesser == page)))
            {
                return false;
            }
        }

        return true;
    }
}
