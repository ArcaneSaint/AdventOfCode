using System.Reflection.Metadata.Ecma335;
using Microsoft.Win32;

namespace AdventOfCode.Solvers.Year2024;

internal class Day17Solver(string part1Test, string part2Test) : BaseSolver2024<string>(17, part1Test, part2Test)
{
    public class Computer
    {
        private int RegisterA { get; set; }
        private int RegisterB { get; set; }
        private int RegisterC { get; set; }
        private int InstructionPointer { get; set; } = 0;
        private List<int> Input { get; set; }
        public List<int> Output { get; }


        private Dictionary<int, Action<int>> commandTable;
        public Computer(int registerA, int registerB, int registerC, List<int> input)
        {
            RegisterA = registerA;
            RegisterB = registerB;
            RegisterC = registerC;

            commandTable = new Dictionary<int, Action<int>>() {
                { 0, Adv},
                { 1, Bxl},
                { 2, Bst},
                { 3, Jnz},
                { 4, Bxc},
                { 5, Out},
                { 6, Bdv},
                { 7, Cdv}
            };

            Input = input;
        }

        public void Process()
        {
            var inputLength = Input.Count;
            while (InstructionPointer < inputLength)
            {
                var opCode = Input[InstructionPointer++];
                var operand = Input[InstructionPointer++];
                var command = commandTable[opCode];
                command(operand);
            }
        }

        private int TranslateComboOperand(int comboOperand)
        {
            return comboOperand switch
            {
                >= 0 and <= 3 => comboOperand,
                4 => RegisterA,
                5 => RegisterB,
                6 => RegisterC,
                7 or _ => 0
            };
        }

        private void Adv(int comboOperand)
        {
            RegisterA = RegisterA / TranslateComboOperand(comboOperand);
        }
        private void Bxl(int literalOperand)
        {
            RegisterB = RegisterB | literalOperand;
        }
        private void Bst(int comboOperand)
        {
            var value = TranslateComboOperand(comboOperand);
            RegisterB = value % 8;
        }
        private void Jnz(int literalOperand)
        {
            if (RegisterA == 0)
            {
                return;
            }
            InstructionPointer = literalOperand;
        }
        private void Bxc(int ignoredOperand)
        {
            RegisterB = RegisterB ^ RegisterC;
        }
        private void Out(int comboOperand)
        {
            var value = TranslateComboOperand(comboOperand);
            Output.Add(value % 8);
        }
        private void Bdv(int comboOperand)
        {
            RegisterB = RegisterA / TranslateComboOperand(comboOperand);
        }
        private void Cdv(int comboOperand)
        {
            RegisterC = RegisterA / TranslateComboOperand(comboOperand);
        }

    }

    private (int a, int b, int c, List<int> commands) ParseInput(string[] input)
    {
        var a = input[0].Split(':').Last().Trim().ToInt();
        var b = input[1].Split(':').Last().Trim().ToInt();
        var c = input[2].Split(':').Last().Trim().ToInt();

        var commands = input[4].Split(':').Last().Split(',').Select(x => x.ToInt()).ToList();
        return (a, b, c, commands);
    }
    public override long Part1(string[] input)
    {
        var (a, b, c, commands) = ParseInput(input);
        var shittyElfPc = new Computer(a, b, c, commands);
        shittyElfPc.Process();
        var result = String.Join(',', shittyElfPc.Output);
        return 0;
    }

    public override long Part2(string[] input)
    {
        return 0;
    }
}