using System.Drawing;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode.Solvers.Year2024;

internal class Day13Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2024<long>(13, part1Test, part2Test)
{
    struct Machine
    {
        public (long X, long Y) A { get; set; }
        public (long X, long Y) B { get; set; }
        public (long X, long Y) Target { get; set; }
    }

    Machine ParseMachine(string[] input, long offset = 0)
    {
        var line1 = Regex.Match(input[0], @"Button A: X\+(\d+), Y\+(\d+)");
        var line2 = Regex.Match(input[1], @"Button B: X\+(\d+), Y\+(\d+)");
        var line3 = Regex.Match(input[2], @"Prize: X=(\d+), Y=(\d+)");

        return new Machine()
        {
            A = (line1.Groups[1].Value.ToInt(), line1.Groups[2].Value.ToInt()),
            B = (line2.Groups[1].Value.ToInt(), line2.Groups[2].Value.ToInt()),
            Target = (line3.Groups[1].Value.ToLong() + offset, line3.Groups[2].Value.ToLong() + offset),
        };
    }

    List<Machine> ParseInput(string[] input, long offset = 0)
    {
        var result = new List<Machine>();
        for (int i = 0; i < input.Length; i += 4)
        {
            result.Add(ParseMachine(input[i..(i + 3)], offset));
        }
        return result;
    }

    private long SolveMachine(Machine machine)
    {
        long? cost = null;

        for (int i = 0; i < 100; ++i)
        {
            var currentX = machine.A.X * i;
            var currentY = machine.A.Y * i;
            var remainingX = machine.Target.X - currentX;
            var remainingY = machine.Target.Y - currentY;

            var possibleWithB = remainingX % machine.B.X == 0;
            possibleWithB &= remainingY % machine.B.Y == 0;



            if (possibleWithB)
            {
                var pressesOnBX = remainingX / machine.B.X;
                var pressesOnBY = remainingY / machine.B.Y;
                if (pressesOnBX == pressesOnBY && pressesOnBX < 100)
                {
                    var calculatedCost = i * 3L + pressesOnBX * 1L;
                    if ((cost ?? Int32.MaxValue) > calculatedCost)
                    {
                        cost = calculatedCost;


                        Display(machine, i, pressesOnBX);
                    }
                }
                else
                {

                }
            }

        }
        return cost ?? 0L;
    }

    private long SolveMachineUnlimited(Machine machine)
    {
        var a = (machine.Target.X * machine.B.Y - machine.Target.Y * machine.B.X) / (machine.A.X * machine.B.Y - machine.A.Y * machine.B.X);
        var b = (machine.Target.Y * machine.A.X - machine.Target.X * machine.A.Y) / (machine.A.X * machine.B.Y - machine.A.Y * machine.B.X);

        var checkSumX = a * machine.A.X + b * machine.B.X;
        var checkSumY = a * machine.A.Y + b * machine.B.Y;

        var isValidX = checkSumX == machine.Target.X;
        var isValidY = checkSumY == machine.Target.Y;
        if ((isValidX && isValidY))
        {
            return a * 3L + b;
        }

        return 0;

    }
    public override long Part1(string[] input)
    {
        var machines = ParseInput(input);
        var total = 0L;
        foreach (var machine in machines)
        {
            var result = SolveMachineUnlimited(machine);



            total += result;
        }
        return total;
    }

    private void Display(Machine machine, long a, long b)
    {

        var checkSumX = a * machine.A.X + b * machine.B.X;
        var checkSumY = a * machine.A.Y + b * machine.B.Y;

        var isValidX = checkSumX == machine.Target.X;
        var isValidY = checkSumY == machine.Target.Y;
        if (!(isValidX && isValidY))
        {
            Console.WriteLine($"Button A: X+{machine.A.X}, Y+{machine.A.Y}");
            Console.WriteLine($"Button B: X+{machine.B.X}, Y+{machine.B.Y}");
            Console.WriteLine($"Prize: X={machine.Target.X}, Y={machine.Target.Y}");

            Console.WriteLine($"{a} * A; {b} * B");
            Console.WriteLine();
        }
    }

    public override long Part2(string[] input)
    {
        var machines = ParseInput(input, 10000000000000);
        var total = 0L;
        foreach (var machine in machines)
        {
            var result = SolveMachineUnlimited(machine);


            total += result;
        }
        return total;
    }
}
