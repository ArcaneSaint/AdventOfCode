using System.Reflection.PortableExecutable;
using static AdventOfCode.Solvers.Year2024.Day17Solver;

namespace AdventOfCode.Solvers.Year2024;

internal class Day23Solver(string part1Test, string part2Test) : BaseSolver2024<string>(23, part1Test, part2Test)
{
    static Random rng = new Random();

    class Computer
    {
        public List<Computer> Connections { get; set; } = new List<Computer>();
        public string Name { get; set; }
        public int Id { get; set; } = rng.Next();
    }


    private List<Computer> ParseInput(string[] input)
    {
        var result = new List<Computer>();
        foreach (var line in input)
        {
            var first = line.Split('-')[0];
            var second = line.Split('-')[1];

            if (result.FirstOrDefault(x => x.Name == first) is not { } computer1)
            {
                computer1 = new Computer() { Name = first };
                result.Add(computer1);
            }
            if (result.FirstOrDefault(x => x.Name == second) is not { } computer2)
            {
                computer2 = new Computer() { Name = second };
                result.Add(computer2);
            }

            computer1.Connections.Add(computer2);
            computer2.Connections.Add(computer1);
        }

        return result;
    }

    public override string Part1(string[] input)
    {
        var computers = ParseInput(input);

        var toConsider = computers.Where(x => x.Name.StartsWith('t')).ToList();

        var result = 0;
        var pairings = new List<string>();
        foreach (var computer in toConsider)
        {
            var neighbours = computer.Connections;

            //check if at least 1 neighbours has another neighbour as neighbour

            foreach (var neighbour in neighbours)
            {
                var othersOfThruple = neighbours.Where(n => n.Connections.Contains(neighbour)).ToList();

                foreach (var other in othersOfThruple)
                {
                    pairings.Add(String.Join(',', new[] { computer.Name, neighbour.Name, other.Name }.Order()));
                }
            }



        }
        pairings = pairings.Distinct().ToList();
        //foreach (var pair in pairings)
        //{
        //    Console.WriteLine($"{pair}");
        //}
        return pairings.Count.ToString();
    }


    private Dictionary<string, List<string>> ParseConnections(string[] input)
    {
        var dictionary = new Dictionary<string, List<string>>();


        foreach (var line in input)
        {
            var first = line.Split('-')[0];
            var second = line.Split('-')[1];

            if (!dictionary.ContainsKey(first))
            {
                dictionary[first] = new List<string>();
            }
            if (!dictionary.ContainsKey(second))
            {
                dictionary[second] = new List<string>();
            }

            dictionary[first].Add(second);
            dictionary[second].Add(first);
        }

        return dictionary;
    }


    Dictionary<(List<int>, List<int>), List<Computer>> memoizations = new();

    List<Computer> Party(List<Computer> remaining, List<Computer> currentParty)
    {
        if (remaining.Count == 0)
        {
            return currentParty;
        }
        else
        {
            var biggestParty = currentParty;
            var key = (remaining.Select(c => c.Id).ToList(), currentParty.Select(c => c.Id).ToList());

            if (memoizations.ContainsKey(key))
            {
                return memoizations[key];
            }

            foreach (var computer in remaining)
            {
                //is this one connected to the entire party?
                if (currentParty.All(pc => pc.Connections.Any(c => c == computer)))
                {
                    var party = Party(remaining.Where(x => x != computer).ToList(), [.. currentParty, computer]);
                    if (party.Count > biggestParty.Count)
                    {
                        biggestParty = party;
                    }
                }
            }

            memoizations[key] = biggestParty;
            return biggestParty;
        }
    }


    Dictionary<string, List<string>> connectionsGraph;
    List<string> FindClique(List<string> currentClique, List<string> remaining)
    {
        if (remaining.Count == 0)
        {
            return currentClique;
        }

        var maximumAtThisLevel = currentClique;
        string toAddNow = null;


        foreach (var other in remaining)
        {
            var totalParty = currentClique.Intersect(connectionsGraph[other]).ToList();
            if (totalParty.Count >= maximumAtThisLevel.Count)
            {
                maximumAtThisLevel = totalParty;
                toAddNow = other;
            }
        }
        if (toAddNow != null)
        {
            //maximumAtThisLevel now has the biggest clique we can currently make, try the next levels:
            return FindClique([..currentClique, toAddNow], remaining.Where(x => x != toAddNow).ToList());
        }
        else
        {
            //adding anything now results in a smaller clique
            return maximumAtThisLevel;
        }
    }

    public override string Part2(string[] input)
    {
        connectionsGraph = ParseConnections(input);

        foreach (var dangler in connectionsGraph.Where(v => v.Value.Count == 1).ToList())
        {
            connectionsGraph.Remove(dangler.Key);
        }

        var biggestParty = new List<string>();
        foreach (var key in connectionsGraph.Keys)
        {
            var others = connectionsGraph[key];
            var totalParty = others;

            totalParty = FindClique([key], connectionsGraph[key]);
            if(totalParty.Count > biggestParty.Count)
            {
                biggestParty = totalParty;
            }
        }





        return String.Join(",", biggestParty.Order());
    }
}
