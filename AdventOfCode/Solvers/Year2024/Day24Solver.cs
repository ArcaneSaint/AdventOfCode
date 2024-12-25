namespace AdventOfCode.Solvers.Year2024;

internal class Day24Solver(long part1Test = 0, string part2Test = "") : BaseSolver2024<long, string>(24, part1Test, part2Test)
{


    private interface IGate
    {
        public bool Value { get; }
        public string Name { get; }
        public List<string> AllPredecessors { get; }
        public string NestedFullName { get; }
    }

    private interface IOperatorGate : IGate
    {
        public IGate Gate1 { get; set; }
        public IGate Gate2 { get; set; }
        public bool Value { get; }
        public string Name { get; }
        public string Operator { get; set; }
        public bool IsSwapped { get; set; }


    }

    private class ConstantGate(bool value, string name) : IGate
    {
        public bool Value { get; set; } = value;

        public string Name => name;
        public List<string> AllPredecessors => [name];

        public string NestedFullName => Name;
    }
    private class OperatorGate(string name, IGate gate1, IGate gate2,string @operator) : IOperatorGate
    {
        public string Name => name;
        public bool Value => Operator switch
        {
            "XOR" => Gate1.Value ^ Gate2.Value,
            "OR" => Gate1.Value | Gate2.Value,
            "AND" => Gate1.Value & Gate2.Value
        };

        public IGate Gate1 { get; set; } = gate1;
        public IGate Gate2 { get; set; } = gate2;
        public bool IsSwapped { get; set; }
        public List<string> AllPredecessors => [.. Gate1.AllPredecessors, .. Gate2.AllPredecessors, name];
        public string Operator { get; set; } = @operator;
        public string NestedFullName => $"({Gate1.NestedFullName}) {Operator} ({Gate2.NestedFullName})";
    }



    private List<IGate> ParseInput(string[] input)
    {
        var result = new List<IGate>();
        var i = 0;
        for (i = 0; i < input.Length && !String.IsNullOrWhiteSpace(input[i]); i++)
        {
            var data = input[i].Split(": ");
            result.Add(new ConstantGate(data[1] == "1", data[0]));
        }

        var toProcess = new Queue<string>();
        input.Skip(i + 1).ToList().ForEach(toProcess.Enqueue);

        while (toProcess.Any())
        {
            var item = toProcess.Dequeue();
            var data = item.Split(" ");
            //ntg XOR fgs -> mjb
            //{gate1} {operator} {gate2} {->} {target}
            var gate1Name = data[0];
            var @operator = data[1];
            var gate2Name = data[2];
            var name = data[4];

            var gate1 = result.FirstOrDefault(x => x.Name == gate1Name);
            var gate2 = result.FirstOrDefault(x => x.Name == gate2Name);
            var ownGate = result.FirstOrDefault(x => x.Name == name);
            if (ownGate == null && gate1 != null && gate2 != null)
            {
                result.Add(new OperatorGate(name, gate1, gate2, @operator));
            }
            else
            {
                toProcess.Enqueue(item);
            }
        }


        return result;
    }

    public override long Part1(string[] input)
    {
        var gates = ParseInput(input);

        var gatesWithZ = gates.Where(x => x.Name.StartsWith('z')).OrderBy(x => x.Name).ToList();


        //foreach (var gate in gates)
        //{
        //    Console.WriteLine($"{gate.Name}: {(gate.Value ? '1' : '0')}");
        //}

        return GetNumber(gatesWithZ);
    }

    private long GetNumber<T>(ICollection<T> gates) where T : IGate
    {
        gates = gates.OrderBy(x => x.Name).ToList();
        var bit = 1L;
        var result = 0L;

        foreach (var gate in gates)
        {
            result += bit * (gate.Value ? 1 : 0);
            bit *= 2;
        }
        return result;
    }

    private bool isTest;
    private bool TestNumbers(List<IGate> gates)
    {
        var random = new Random();
        var gatesWithX = gates.Where(x => x.Name.StartsWith('x')).Cast<ConstantGate>().ToList();
        var gatesWithY = gates.Where(x => x.Name.StartsWith('y')).Cast<ConstantGate>().ToList();
        var gatesWithZ = gates.Where(x => x.Name.StartsWith('z')).ToList();

        for (int rounds = 0; rounds < 10; ++rounds)
        {

            var input1 = GetNumber(gatesWithX);
            var input2 = GetNumber(gatesWithY);
            var expected = input1 + input2;
            if (isTest)
            {
                expected = input1 & input2; //de test-case is een bitwise AND
            }

            var result = GetNumber(gatesWithZ);
            if (expected != result)
            {
                return false;
            }

            for (int i = 0; i < gatesWithX.Count; i++)
            {
                gatesWithX[i].Value = random.NextDouble() > 0.5;
            }
            for (int i = 0; i < gatesWithY.Count; i++)
            {
                gatesWithY[i].Value = random.NextDouble() > 0.5;
            }
        }

        return true;
    }

    private bool TryASwapparoo(List<IOperatorGate> gates, int swapsLeft, List<IGate> allGates)
    {
        if (swapsLeft == 0 || gates.Count == 0)
        {
            return TestNumbers(allGates);
        }

        for (var i = 0; i < gates.Count; ++i)
        {
            //Console.SetCursorPosition(0, 4 - swapsLeft);
            // Console.Write($"{i}/{gates.Count}    ");
            for (var j = i + 1; j < gates.Count; ++j)
            {
                //swap two gates and check deeper:
                var swap1 = gates[i];
                var swap2 = gates[j];

                if (!swap1.AllPredecessors.Contains(swap2.Name) && !swap2.AllPredecessors.Contains(swap1.Name))
                {
                    Swap(swap1, swap2);
                    //no more swapping the swapped
                    gates.Remove(swap1);
                    gates.Remove(swap2);

                    if (TryASwapparoo(gates, swapsLeft - 1, allGates))
                    {
                        //terugzetten voor volgende iteratie
                        gates.Add(swap1);
                        gates.Add(swap2);
                        return true;
                    }

                    //terugzetten voor volgende iteratie
                    gates.Add(swap1);
                    gates.Add(swap2);
                    Swap(swap2, swap1);
                }
            }
        }


        return false;
    }

    private void Swap(IOperatorGate swap1, IOperatorGate swap2)
    {
        (swap1.Gate1, swap1.Gate2, swap1.Operator, swap1.IsSwapped, swap2.Gate1, swap2.Gate2, swap2.Operator, swap2.IsSwapped) =
            (swap2.Gate1, swap2.Gate2, swap2.Operator,!swap1.IsSwapped, swap1.Gate1, swap1.Gate2, swap1.Operator, !swap2.IsSwapped);
    }

    public override string Part2(string[] input)
    {
        var gates = ParseInput(input);
        isTest = false;
        if (input.Count() == 19)
        {
            isTest = true;
        }

        var gatesWithX = gates.Where(x => x.Name.StartsWith('x')).ToList();
        var gatesWithY = gates.Where(x => x.Name.StartsWith('y')).ToList();
        var gatesWithZ = gates.Where(x => x.Name.StartsWith('z')).ToList();

        var swappableGates = gates
            .Where(x => !x.Name.StartsWith('x') && !x.Name.StartsWith('y'))
            .Cast<IOperatorGate>()
            .ToList();

        var swapsToTry = 1;
        if (!isTest)
        {
            //swapsToTry = 4;
            foreach (var gate in gates.Where(x => x is IOperatorGate).Cast<IOperatorGate>().OrderBy(x => x.Name))
            {
                Console.WriteLine($"{gate.Gate1.Name} {gate.Name}");
                Console.WriteLine($"{gate.Gate2.Name} {gate.Name}");
            }
        }
        bool valid = false;



        // PrintOut(gates);

        while (!valid)
        {
            valid = TryASwapparoo(swappableGates, swapsToTry, gates);
            swapsToTry += 1;
        }

        //find the swapped gates
        var swapped = swappableGates.Where(x => x.IsSwapped).OrderBy(x => x.Name).ToList();

        return String.Join(",", swapped.Select(x => x.Name));
    }

    private void PrintOut(List<IGate> gates)
    {

        var gatesWithX = gates.Where(x => x.Name.StartsWith('x')).OrderBy(x => x.Name).ToList();
        var gatesWithY = gates.Where(x => x.Name.StartsWith('y')).OrderBy(x => x.Name).ToList();
        var operatorGates = gates.Where(x => x is IOperatorGate).Cast<IOperatorGate>().ToList();

        for (int i = 0; i < gatesWithX.Count; i++)
        {
            var startingGateX = gatesWithX[i];
            var startingGateY = gatesWithY[i];
            var nextGates = operatorGates.Where(x => (x.Gate1 == startingGateX && x.Gate2 == startingGateY) || (x.Gate2 == startingGateX && x.Gate1 == startingGateY));

            foreach (var nextGate in nextGates)
            {
                Console.WriteLine($"{startingGateX.Name} {nextGate.Operator} {startingGateX.Name} -> {nextGate.Name} ");
            }

        }

    }
}
