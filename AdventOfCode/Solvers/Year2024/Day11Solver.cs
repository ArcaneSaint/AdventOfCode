namespace AdventOfCode.Solvers.Year2024;

internal class Day11Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2024<long>(11, part1Test, part2Test)
{
    const int NumberOfBlinksPartOne = 25;
    const int NumberOfBlinksPartTwo = 75;
    public override long Part1(string[] input)
    {
        var data = ParseInput(input[0]);
        for (int i = 0; i < NumberOfBlinksPartOne; ++i)
        {
            data = Blink(data);
            
        }

        return data.Sum(kvp => kvp.Value);
    }

    private Dictionary<string, long> ParseInput(string input) => input.Split(' ').GroupBy(x => x).ToDictionary(g => g.Key, g => (long)g.Count());

    private Dictionary<string, long> Blink(Dictionary<string, long> stones)
    {
        var nextStep = new Dictionary<string, long>();
        foreach (var stone in stones)
        {
            if (stone.Key == "0")
            {
                nextStep.TryGetValue("1", out var value);
                nextStep["1"] = value + stone.Value;
            }
            else if (stone.Key.Length % 2 == 0)
            {
                var nextKey1 = stone.Key[0..(stone.Key.Length / 2)];
                var nextKey2 = (stone.Key[(stone.Key.Length / 2)..].TrimStart('0')).AsNullIfWhiteSpace() ?? "0";

                nextStep.TryGetValue(nextKey1, out var value1);
                nextStep[nextKey1] = value1 + stone.Value;

                nextStep.TryGetValue(nextKey2, out var value2);
                nextStep[nextKey2] = value2 + stone.Value;

            }
            else
            {
                var nextKey = (long.Parse(stone.Key) * 2024).ToString();
                nextStep.TryGetValue(nextKey, out var value);
                nextStep[nextKey] = value + stone.Value;
            }
        }

        return nextStep;
    }
    

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used for debugging display")]
    private void Display(List<string> input)
    {
        Console.WriteLine(String.Join(" ", input));
    }

    public override long Part2(string[] input)
    {
        var data = ParseInput(input[0]);
        for (int i = 0; i < NumberOfBlinksPartTwo; ++i)
        {
            data = Blink(data);
        }

        return data.Sum(kvp => kvp.Value);
    }
}
