using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers.Year2024;

internal class Day5Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2024(5, part1Test, part2Test)
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

        //var correctedUpdates = new List<List<string>>();
        //foreach (var update in inCorrectUpdates)
        //{
        //    var permutations = CreatePermutations(update);

        //    correctedUpdates.Add(permutations.FirstOrDefault(x => IsValid(x, rules)).ToList());
        //}

        var rng = new Random();
        var correctedUpdates = new List<List<string>>();
        for (var i = 0; i < inCorrectUpdates.Count; ++i)
        {
            var update = inCorrectUpdates[i];
            var pastValues = new List<string>();

            do
            {
                pastValues = new List<string>();
                foreach (var value in update)
                {
                    if (pastValues.TrueForAll(pastValue =>
                            !rules.Any(rule => rule.Greater == pastValue && rule.Lesser == value)))
                    {
                        pastValues.Add(value);
                    }
                    else
                    {
                        pastValues.Insert(0, value);
                    }
                }

                update = pastValues.ToList();
            } while (!IsValid(pastValues, rules));


            if (IsValid(pastValues, rules))
            {
                correctedUpdates.Add(pastValues);
            }
            else
            {

            }
        }



        result = correctedUpdates.Select(update => update[update.Count() / 2].ToInt()).Sum();

        return result;
    }

    private bool IsValid(string line, List<(string Lesser, string Greater)> rules)
    {
        return IsValid(line.Split(","), rules);
    }
    private bool IsValid(IEnumerable<string> values, List<(string Lesser, string Greater)> rules)
    {
        var pastValues = new List<string>();
        foreach (var page in values)
        {
            if (pastValues.TrueForAll(pastValue =>
                    !rules.Any(rule => rule.Greater == pastValue && rule.Lesser == page)))
            {
                pastValues.Add(page);
            }
            else
            {
                return false;
            }
        }

        return true;
    }


    IEnumerable<IEnumerable<T>> CreatePermutations<T>(List<T> list)
    {
        if (list.Count() == 1)
        {
            yield return [list[0]];
        }
        else
        {
            for (int i = 0; i < list.Count(); ++i)
            {
                var next = new List<T>() { list[i] };
                var data = CreatePermutations<T>(list.Except(next).ToList()).Select(perm => perm.Prepend(list[i]));
                foreach (var value in data)
                {
                    yield return value;
                }

            }
        }
    }
}
