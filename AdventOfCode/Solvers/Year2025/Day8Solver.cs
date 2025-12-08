using System.Linq;
using System.Runtime.CompilerServices;
using AdventOfCode.Helpers;

namespace AdventOfCode.Solvers.Year2025;

internal class Day8Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2025<long>(8, part1Test, part2Test)
{
    private int NumberOfPairsToCheck(string[] input)
    {
        return input.Length > 100 ? 1000 : 10;
    }

    private double FastDistance((int X, int Y, int Z) point1, (int X, int Y, int Z) point2)
    {
        return Math.Pow(point1.X - point2.X, 2)
            + Math.Pow(point1.Y - point2.Y, 2)
            + Math.Pow(point1.Z - point2.Z, 2);
    }

    private List<(int X, int Y, int Z)> ParseInput(string[] input)
    {
        var result = new List<(int X, int Y, int Z)>();

        foreach (var line in input)
        {
            var data = line.Split(',');
            result.Add((data[0].ToInt(), data[1].ToInt(), data[2].ToInt()));
        }

        return result;
    }

    private void JoinCircuits(Circuit circuit1, Circuit circuit2)
    {
        circuit1.Boxes.AddRange(circuit2.Boxes);
        circuit2.Boxes.Clear();
    }

    public override long Part1(string[] input)
    {
        var pairsToCheck = NumberOfPairsToCheck(input);
        var allBoxes = ParseInput(input); int id = 0;
        var circuits = allBoxes.Select(x => new Circuit()
        {
            Boxes = [x]
        }).ToList();

        Dictionary<double, ((int, int, int), (int, int, int))> boxesGroupedByDistances = new();
        for (var i = 0; i < allBoxes.Count; ++i)
        {
            for (var j = i + 1; j < allBoxes.Count; ++j)
            {
                var distance = FastDistance(allBoxes[i], allBoxes[j]);
                boxesGroupedByDistances[distance] = ((allBoxes[i], allBoxes[j]));
            }
        }

        var allActualDistances = boxesGroupedByDistances.Keys.Order().ToList();
        for (int i = 0; i < pairsToCheck; ++i)
        {
            var distance = allActualDistances[i];
            var boxesToJoin = boxesGroupedByDistances[distance];
            var (box1, box2) = boxesToJoin; //tuple deconstruction for the win

            var circuit1 = circuits.FirstOrDefault(c => c.Boxes.Contains(box1));
            var circuit2 = circuits.FirstOrDefault(c => c.Boxes.Contains(box2));
            if (circuit1 != circuit2)
            {
                JoinCircuits(circuit1, circuit2);
            }
        }

        var top3 = circuits.OrderByDescending(x => x.Boxes.Count).Take(3).Select(x => x.Boxes.Count).ToList();
        return top3[0] * top3[1] * top3[2];
    }

    public override long Part2(string[] input)
    {
        var pairsToCheck = NumberOfPairsToCheck(input);
        var allBoxes = ParseInput(input); int id = 0;
        var circuits = allBoxes.Select(x => new Circuit()
        {
            Boxes = [x]
        }).ToList();

        Dictionary<double, ((int, int, int), (int, int, int))> boxesGroupedByDistances = new();
        for (var i = 0; i < allBoxes.Count; ++i)
        {
            for (var j = i + 1; j < allBoxes.Count; ++j)
            {
                var distance = FastDistance(allBoxes[i], allBoxes[j]);
                boxesGroupedByDistances[distance] = ((allBoxes[i], allBoxes[j]));
            }
        }

        var allActualDistances = boxesGroupedByDistances.Keys.Order().ToList();

        (int X, int Y, int Z) box1 = (0, 0, 0), box2 = (0, 0, 0);
        int index = 0;
        do
        {
            var distance = allActualDistances[index];
            var boxesToJoin = boxesGroupedByDistances[distance];
            (box1, box2) = boxesToJoin; //tuple deconstruction for the win

            var circuit1 = circuits.FirstOrDefault(c => c.Boxes.Contains(box1));
            var circuit2 = circuits.FirstOrDefault(c => c.Boxes.Contains(box2));
            if (circuit1 != circuit2)
            {
                JoinCircuits(circuit1, circuit2);
            }
            ++index;
        } while (circuits.Count(c => c.Boxes.Any()) > 1);

        return (long)box1.X * (long)box2.X;
    }

    internal class Circuit
    {
        public List<(int X, int Y, int Z)> Boxes { get; set; } = new();
    }
}