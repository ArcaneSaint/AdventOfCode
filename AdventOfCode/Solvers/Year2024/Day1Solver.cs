using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.Solvers.Year2024;

namespace AdventOfCode.Solvers.Year2024;

internal class Day1Solver() : BaseSolver2024(1)
{
    public override string Part1(string input)
    {
        var split = input.Split(' ', '\r', '\n').Where(x => !String.IsNullOrWhiteSpace(x)).ToList();

        var lists = Split(split);
        var total = 0;

        for (var i = 0; i < lists.List1.Count; i++)
        {
            total += Math.Abs(lists.List1[i] - lists.List2[i]);
        }

        return total.ToString();
    }

    private Dictionary<int, int> scores = new();

    public override string Part2(string input)
    {
        var split = input.Split(' ', '\r', '\n').Where(x => !String.IsNullOrWhiteSpace(x)).ToList();

        var lists = Split(split);

        var total = 0;

        for (var i = 0; i < lists.List1.Count; i++)
        {
            if (!scores.ContainsKey(i))
            {
                scores[i] = lists.List2.Where(x => x == lists.List1[i]).Count() * lists.List1[i];
            }

            total += scores[i];
        }

        return total.ToString();
    }

    private (List<int> List1, List<int> List2) Split(List<string> input)
    {
        return (
            input.Where((x, i) => i % 2 == 1).Select(Int32.Parse).ToList(),
            input.Where((x, i) => i % 2 == 0).Select(Int32.Parse).ToList()
        );
    }
}
