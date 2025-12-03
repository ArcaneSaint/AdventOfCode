using System.Linq;

namespace AdventOfCode.Solvers.Year2015;

internal class Day9Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2015(9, part1Test, part2Test)
{

    Dictionary<string, Location> locations = new();

    public override long Part1(string[] input)
    {
        locations = new();
        input.ToList().ForEach(Parse);

        int shortestDistance = Int32.MaxValue;
        foreach (var loc in locations.Keys)
        {
            var shortDistance = BruteForce(loc, [.. locations.Keys.Where(x => x != loc)]);
            if (shortDistance < shortestDistance)
            {
                shortestDistance = shortDistance;
            }
        }
        return shortestDistance;
    }


    private int BruteForce(string current, List<string> remaining)
    {
        if (!remaining.Any())
        {
            return 0;
        }

        int shortestDistance = Int32.MaxValue;
        foreach (var loc in remaining)
        {
            var distance = locations[current].Distances[loc];

            var nextRemaining = remaining.Where(x => x != loc).ToList();
            var shortDistance = BruteForce(loc, nextRemaining) + distance;
            
            if (shortDistance < shortestDistance)
            {
                shortestDistance = shortDistance;
            }
        }
        return shortestDistance;
    }
    private int BruteForce2(string current, List<string> remaining)
    {
        if (!remaining.Any())
        {
            return 0;
        }

        int longestDistance = Int32.MinValue;
        foreach (var loc in remaining)
        {
            var distance = locations[current].Distances[loc];

            var nextRemaining = remaining.Where(x => x != loc).ToList();
            var distanceResult = BruteForce2(loc, nextRemaining) + distance;

            if (distanceResult > longestDistance)
            {
                longestDistance = distanceResult;
            }
        }
        return longestDistance;
    }


    public override long Part2(string[] input)
    {
        locations = new();
        input.ToList().ForEach(Parse);

        int longestDistance = Int32.MinValue;
        foreach (var loc in locations.Keys)
        {
            var distanceResult = BruteForce2(loc, [.. locations.Keys.Where(x => x != loc)]);
            if (distanceResult > longestDistance)
            {
                longestDistance = distanceResult;
            }
        }
        return longestDistance;
    }


    private void Parse(string line)
    {
        var data = line.Split(" to ");
        var loc1 = data[0];
        var loc2 = data[1].Split(" =")[0];
        var dist = Int32.Parse(data[1].Split(" = ")[1]);

        if (!locations.TryGetValue(loc1, out var location1))
        {
            location1 = new Location();
            locations[loc1] = location1;
        }
        if (!locations.TryGetValue(loc2, out var location2))
        {
            location2 = new Location();
            locations[loc2] = location2;
        }
        //locations

        location1.Distances[loc2] = dist;
        location2.Distances[loc1] = dist;

    }


    class Location
    {
        public string Name { get; set; }

        public Dictionary<string, int> Distances { get; set; } = new();
    }
}
