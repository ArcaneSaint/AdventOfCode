using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode.Solvers.Year2024;

internal class Day2Solver() : BaseSolver2024(2)
{
    public override string Part1(string[] input)
    {
        var result = 0;
        
        foreach (var line in input)
        {
            if (Validate(line.Split(' ').Select(Int32.Parse).ToList()))
            {
                ++result;
            }
        }

        return result.ToString();
    }

    private static bool Validate(List<int> data)
    {
        var valid = true;
        var isIncrease = true;
        var isDecrease = true;

        for (int i = 1; i < data.Count; i++)
        {
            var diff = Math.Abs(data[i] - data[i - 1]);
            isIncrease &= data[i] >= data[i - 1];
            isDecrease &= data[i] <= data[i - 1];
            valid &= diff is >= 1 and <= 3;
        }

        return valid && (isIncrease || isDecrease);
    }

    public override string Part2(string[] input)
    {
        var result = 0;

        foreach (var line in input)
        {
            var valid = false;
            var data = line.Split(' ').Select(Int32.Parse).ToList();
            for (int i = 0; i < line.Length-1; i++)
            {
                var list = data.Take(i).ToList();
                list.AddRange(data.Skip(i + 1));
                valid |= Validate(list);
            }

            if (valid)
            {
                ++result;
            }
        }

        return result.ToString();
    }
}
