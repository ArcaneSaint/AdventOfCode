namespace AdventOfCode.Solvers.Year2024;

internal class Day1Solver() : BaseSolver2024(1)
{
    public override string Part1(string input)
    {
        var split = input.Split(' ', '\r', '\n').Where(x => !String.IsNullOrWhiteSpace(x)).ToList();

        var (list1, list2) = Split(split);
        var total = 0;

        for (var i = 0; i < list1.Count; i++)
        {
            total += Math.Abs(list1[i] - list2[i]);
        }

        return total.ToString();
    }

    private readonly Dictionary<int, int> _scores = [];

    public override string Part2(string input)
    {
        var split = input.Split(' ', '\r', '\n').Where(x => !String.IsNullOrWhiteSpace(x)).ToList();

        var (list1, list2) = Split(split);

        var total = 0;

        for (var i = 0; i < list1.Count; i++)
        {
            if (!_scores.TryGetValue(i, out int value))
            {
                value = list2.Count(x => x == list1[i]) * list1[i];
                _scores[i] = value;
            }

            total += value;
        }

        return total.ToString();
    }

    private static (List<int> List1, List<int> List2) Split(List<string> input)
    {
        return (
            input.Where((x, i) => i % 2 == 1).Select(Int32.Parse).ToList(),
            input.Where((x, i) => i % 2 == 0).Select(Int32.Parse).ToList()
        );
    }
}
