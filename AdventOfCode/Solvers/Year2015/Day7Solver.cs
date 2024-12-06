using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers.Year2015;

internal partial class Day7Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2015(7, part1Test, part2Test)
{


    public override long Part1(string[] input)
    {
        Dictionary<string, ushort> gates = new()
        {
            {"1", 1 }
        };

        var queue = new Queue<string>(input);


        gates = SolveSignals(queue, gates);

        return gates["a"];
    }

    private static Dictionary<string, ushort> SolveSignals(Queue<string> queue, Dictionary<string, ushort> gates)
    {
        while (queue.Count > 0)
        {
            var line = queue.Dequeue();

            if (StaticValueRegex().Match(line) is { Success: true } staticValueMatch)
            {
                var value = staticValueMatch.Groups[1].Value.ToUInt16();
                var destination = staticValueMatch.Groups[2].Value;

                gates[destination] = (ushort)value;
            }
            else if (PassThroughRegex().Match(line) is { Success: true } passThroughValueMatch)
            {
                var source = passThroughValueMatch.Groups[1].Value;
                var destination = passThroughValueMatch.Groups[2].Value;

                if (gates.TryGetValue(source, out var value))
                {
                    gates[destination] = value;
                }
                else
                {
                    queue.Enqueue(line);
                }
            }
            else if (InverseRegex().Match(line) is { Success: true } inverseMatch)
            {
                var source = inverseMatch.Groups[1].Value;
                var destination = inverseMatch.Groups[2].Value;
                if (gates.TryGetValue(source, out var value))
                {
                    gates[destination] = (ushort)(65535 - value);
                }
                else
                {
                    queue.Enqueue(line);
                }
            }
            else if (OperatorRegex().Match(line) is { Success: true } operatorMatch)
            {
                var source = operatorMatch.Groups[1].Value;
                var @operator = operatorMatch.Groups[2].Value;
                var parameter = operatorMatch.Groups[3].Value;
                var destination = operatorMatch.Groups[4].Value;

                if (gates.TryGetValue(source, out var sourceValue))
                {
                    switch (@operator)
                    {
                        case "AND":
                            {
                                if (gates.TryGetValue(parameter, out var parameterValue))
                                {
                                    gates[destination] = (ushort)(sourceValue & parameterValue);
                                }
                                else
                                {
                                    queue.Enqueue(line);
                                }
                                break;
                            }
                        case "OR":
                            {
                                if (gates.TryGetValue(parameter, out var parameterValue))
                                {
                                    gates[destination] = (ushort)(sourceValue | parameterValue);
                                }
                                else
                                {
                                    queue.Enqueue(line);
                                }
                                break;
                            }

                        case "LSHIFT":
                            {
                                gates[destination] = (ushort)(sourceValue << parameter.ToUInt16());
                                break;
                            }
                        case "RSHIFT":
                            {
                                gates[destination] = (ushort)(sourceValue >> parameter.ToUInt16());
                                break;
                            }
                    }
                }
                else
                {
                    queue.Enqueue(line);
                }
            }
            else
            {
                queue.Enqueue(line);
            }


        }

        return gates;
    }

    public override long Part2(string[] input)
    {
        var aValue = Part1(input);


        Dictionary<string, ushort> gates = new()
        {
            {"1", 1 },
        };

        var queue = new Queue<string>();
        foreach (var line in input)
        {
            if (StaticValueToBRegex().IsMatch(line))
            {
                queue.Enqueue($"{aValue} -> b");
            }
            else
            {
                queue.Enqueue(line);
            }
        }

        gates = SolveSignals(queue, gates);


        return gates["a"];
    }

    [GeneratedRegex(@"^(\d+) -> (.*)")]
    private static partial Regex StaticValueRegex();
    [GeneratedRegex(@"^NOT (\w*) -> (\w*)")]
    private static partial Regex InverseRegex();
    [GeneratedRegex(@"^(\w*) (\w*) (\w*) -> (\w*)")]
    private static partial Regex OperatorRegex();
    [GeneratedRegex(@"^(\w+) -> (.*)")]
    private static partial Regex PassThroughRegex();
    [GeneratedRegex(@"^(\d+) -> b$")]
    private static partial Regex StaticValueToBRegex();
}
