
namespace AdventOfCode.Solvers.Year2025;

internal class Day10Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2025<long>(10, part1Test, part2Test)
{

    public override long Part1(string[] input)
    {
        var result = 0;

        foreach (var line in input)
        {
            result += SolveMachine(line);
        }

        return result;
    }

    private int SolveMachine(string line)
    {
        var data = line.Split(' ');
        var endGoal = data[0][1..^1].Reverse();
        var options = data[1..^1];

        int endState = 0;
        foreach (var ch in endGoal)
        {
            if (ch == '#')
            {
                endState ^= 1;
            }
            endState <<= 1;
        }
        endState >>= 1;

        var machine = new Machine(0);

        foreach (var option in options)
        {
            machine.Options.Add(option[1..^1].Split(',').Select(x => x.ToInt()).ToList());
        }

        var frontier = new List<Machine>() { machine };
        var steps = 0;
        while (!frontier.Any(x => x.State == endState))
        {
            steps++;
            frontier = frontier.SelectMany(x => x.GetAllNeighbours()).DistinctBy(x => x.State).ToList();
        }


        return steps;
    }

    public override long Part2(string[] input)
    {
        var result = 0;

        foreach (var line in input)
        {
            result += SolveJoltageMachine(line);
        }

        return result;
    }

    private int SolveJoltageMachine(string line)
    {
        var data = line.Split(' ');
        var endGoal = data[^1][1..^1];
        var options = data[1..^1];

        var endState = new List<int>();
        foreach (var s in endGoal.Split(','))
        {
            endState.Add(s.ToInt());
        }

        var machine = new Machine(0)
        {
            Joltages = Enumerable.Range(0, endState.Count).Select(x => 0).ToList(),
        };

        foreach (var option in options)
        {
            machine.Options.Add(option[1..^1].Split(',').Select(x => x.ToInt()).ToList());
        }

        var frontier = new List<Machine>() { machine };
        var seen = new List<string>();
        var steps = 0;


        for (int joltageIndex = 0; joltageIndex < endState.Count; ++joltageIndex)
        {

        }

        //while (!frontier.Any(x => x.Joltages.SequenceEqual(endState)))
        //{
        //    steps++;
        //    var nextFrontier = frontier.SelectMany(x => x.GetAllJoltagedNeighbours())
        //        .DistinctBy(machine => String.Join(",", machine.Joltages))
        //        .Where(machine => !seen.Contains(String.Join(",", machine.Joltages)))
        //        .ToList();

        //    frontier.Clear();
        //    foreach (var next in nextFrontier)
        //    {
        //        var ok = true;
        //        for (int i = 0; i < next.Joltages.Count && ok; i++)
        //        {
        //            if (next.Joltages[i] > endState[i])
        //            {
        //                ok = false;
        //            }
        //        }
        //        if (ok)
        //        {
        //            frontier.Add(next);
        //        }

        //        seen.Add(String.Join(",", machine.Joltages));
        //    }


        //}


        return steps;
    }

    internal class Machine
    {
        public int State { get; set; }
        public List<int> Joltages { get; set; }

        public List<List<int>> Options { get; set; } = new();

        public Machine(int state)
        {
            State = state;
        }

        public List<Machine> GetAllJoltagedNeighbours()
        {
            var result = new List<Machine>();
            foreach (var option in Options)
            {
                var nextJoltages = Joltages.Select(x => x).ToList();
                foreach (var index in option)
                {
                    nextJoltages[index]++;
                }
                var nextMachine = new Machine(0)
                {
                    Joltages = nextJoltages,
                    Options = Options
                };
                result.Add(nextMachine);
            }
            return result;
        }


        public List<Machine> GetAllNeighbours()
        {
            var result = new List<Machine>();
            foreach (var option in Options)
            {
                var nextState = State;
                foreach (var index in option)
                {
                    var mask = 0x1 << index;
                    nextState ^= mask;
                }
                var nextMachine = new Machine(nextState)
                {
                    Options = Options
                };
                result.Add(nextMachine);
            }
            return result;
        }
    }
}