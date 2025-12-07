namespace AdventOfCode.Solvers.Year2025;

internal class Day6Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2025<long>(6, part1Test, part2Test)
{
    public override long Part1(string[] input)
    {
        var problems = new Dictionary<int, Problem>();
        foreach (var line in input)
        {
            var data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            for (int i = 0; i < data.Length; ++i)
            {
                if (!problems.ContainsKey(i))
                {
                    problems[i] = new Problem();
                }
                if (Int32.TryParse(data[i], out var value))
                {
                    problems[i].Numbers.Add(value);
                }
                else
                {
                    problems[i].Operator = data[i] == "*" ? Operator.Multiply : Operator.Add;
                }
            }
        }
        return problems.Sum(problem => problem.Value.Solve());
    }

    public override long Part2(string[] input)
    {
        var problems = new List<Problem>();
        var height = input.Length;
        var width = input[0].Length;
        var newProblem = new Problem();

        for (var i = width - 1; i >= 0; --i)
        {
            var numberInProgress = 0;
            for (var j = 0; j < height; ++j)
            {
                var c = input[j][i];
                if (c != ' ')
                {
                    if (Int32.TryParse(c.ToString(), out var value))
                    {
                        numberInProgress = numberInProgress * 10 + value;
                    }
                    else
                    {
                        newProblem.Numbers.Add(numberInProgress);
                        newProblem.Operator = c == '*' ? Operator.Multiply : Operator.Add;
                        problems.Add(newProblem);
                        newProblem = new Problem();
                        numberInProgress = 0;
                    }
                }
            }
            if (numberInProgress != 0)
            {
                newProblem.Numbers.Add(numberInProgress);
            }
        }

        return problems.Sum(problem => problem.Solve());
    }


    internal class Problem
    {
        public List<long> Numbers { get; set; } = new();
        public Operator Operator { get; set; }
        public long Solve()
        {
            if (Operator == Operator.Add)
            {
                return Numbers.Sum();
            }
            else
            {
                var total = 1L;
                Numbers.ForEach(n => total *= n);
                return total;
            }
        }
    }

    internal enum Operator
    {
        Add,
        Multiply
    }
}