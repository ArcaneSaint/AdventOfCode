using AdventOfCode.Helpers;

namespace AdventOfCode.Solvers.Year2024;

internal class Day8Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2024(8, part1Test, part2Test)
{

    public struct Node
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public string Frequency { get; set; }
    }


    public override long Part1(string[] input)
    {
        var locations = new List<(int Row, int Col)>();
        var (nodes, height, width) = ParseInput(input);


        foreach (var node in nodes)
        {
            foreach (var matchingNode in nodes.Where(x => x.Frequency == node.Frequency))
            {
                var vector = (X: node.Row - matchingNode.Row, Y: node.Col - matchingNode.Col);
                if (vector is not { X: 0, Y: 0 })
                {
                    var antiNode = (Row: node.Row + vector.X, Col: node.Col + vector.Y);

                    if (antiNode.Row >= 0 && antiNode.Row < height && antiNode.Col >= 0 && antiNode.Col < width)
                    {
                        locations.Add(antiNode);
                    }
                }
            }
        }

        return locations.Distinct().Count();
    }


    public override long Part2(string[] input)
    {
        var locations = new List<(int Row, int Col)>();
        var (nodes, height, width) = ParseInput(input);


        Parallel.ForEach(nodes, node =>
        {
            Parallel.ForEach(nodes.Where(x => x.Frequency == node.Frequency), matchingNode =>
            {
                var vector = VectorHelpers.CalculateVector(node.Row, node.Col, matchingNode.Row, matchingNode.Col);
                if (vector is not { X: 0, Y: 0 })
                {
                    (int Row, int Col) antiNode = (node.Row, node.Col);

                    while (antiNode.Row >= 0 && antiNode.Row < height && antiNode.Col >= 0 && antiNode.Col < width)
                    {
                        locations.Add(antiNode);
                        antiNode = (Row: antiNode.Row + vector.X, Col: antiNode.Col + vector.Y);
                    }
                }
            });
        });



        foreach (var node in nodes)
        {
            foreach (var matchingNode in nodes.Where(x => x.Frequency == node.Frequency))
            {
                var vector = (X: node.Row - matchingNode.Row, Y: node.Col - matchingNode.Col);
                if (vector is not { X: 0, Y: 0 })
                {
                    (int Row, int Col) antiNode = (node.Row, node.Col);

                    while (antiNode.Row >= 0 && antiNode.Row < height && antiNode.Col >= 0 && antiNode.Col < width)
                    {
                        locations.Add(antiNode);
                        antiNode = (Row: antiNode.Row + vector.X, Col: antiNode.Col + vector.Y);
                    }
                }
            }
        }

        return locations.Distinct().Count();
    }

    private static (List<Node> nodes, int height, int width) ParseInput(string[] input)
    {
        var (height, width) = (input.Length, input[0].Length);
        var nodes = new List<Node>();
        for (var i = 0; i < height; ++i)
        {
            for (var j = 0; j < width; ++j)
            {
                if (input[i][j] != '.')
                {
                    nodes.Add(new()
                    {
                        Frequency = input[i][j].ToString(),
                        Row = i,
                        Col = j,
                    });
                }
            }
        }
        return (nodes, height, width);
    }
}
