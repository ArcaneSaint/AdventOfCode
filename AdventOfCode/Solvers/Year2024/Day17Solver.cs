using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Win32;

namespace AdventOfCode.Solvers.Year2024;

internal class Day17Solver(string part1Test, string part2Test) : BaseSolver2024<string>(17, part1Test, part2Test)
{

    public abstract class Computer
    {
        protected long RegisterA { get; set; }
        protected long RegisterB { get; set; }
        protected long RegisterC { get; set; }
        protected int InstructionPointer { get; set; } = 0;
        protected List<int> Input { get; set; }
        public List<int> Output { get; protected set; }
        protected Dictionary<int, Action<int>> commandTable;

        protected Computer(long registerA, long registerB, long registerC, List<int> input)
        {
            RegisterA = registerA;
            RegisterB = registerB;
            RegisterC = registerC;

            Input = input;
            Output = new List<int>();
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
        }

        protected long TranslateComboOperand(int comboOperand)
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
        protected void Adv(int comboOperand)
        {
            var denominator = (int)Math.Pow(2, TranslateComboOperand(comboOperand));
            RegisterA = (long)(RegisterA / denominator);
        }
        protected void Bxl(int literalOperand)
        {
            RegisterB = RegisterB ^ literalOperand;
        }
        protected void Bst(int comboOperand)
        {
            var value = TranslateComboOperand(comboOperand);
            RegisterB = value % 8;
        }
        protected void Jnz(int literalOperand)
        {
            if (RegisterA == 0)
            {
                return;
            }
            InstructionPointer = literalOperand;
        }
        protected void Bxc(int ignoredOperand)
        {
            RegisterB = RegisterB ^ RegisterC;
        }
        protected void Out(int comboOperand)
        {
            var value = TranslateComboOperand(comboOperand);
            Output.Add((int)(value % 8));
        }
        protected void Bdv(int comboOperand)
        {
            var denominator = (int)Math.Pow(2, TranslateComboOperand(comboOperand));
            RegisterB = (long)(RegisterA / denominator);
        }
        protected void Cdv(int comboOperand)
        {
            var denominator = Math.Pow(2, TranslateComboOperand(comboOperand));
            RegisterC = (long)(RegisterA / denominator);
        }
    }

    public class BasicComputer(long registerA, long registerB, long registerC, List<int> input) : Computer(registerA, registerB, registerC, input)
    {
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
    }

    public class CSharpComputer(long registerA, long registerB, long registerC, List<int> input) : Computer(registerA, registerB, registerC, input)
    {

        private int outputLength = -1;

        public void Process()
        {

            do
            {
                //4,1
                RegisterB = (((RegisterA % 8) ^ 7) ^ 7) ^ RegisterA / (long)Math.Pow(2, ((RegisterA % 8) ^ 7));
                //5,5
                Output.Add((int)(RegisterB % 8));
                //3,0
                //0,3
                RegisterA /= 8;
            } while (RegisterA > 0);


            //do
            //{
            //    //2,4
            //    RegisterB = RegisterA % 8;
            //    //1,7
            //    RegisterB ^= 7;
            //    //7,5
            //    RegisterC = RegisterA / (long)Math.Pow(2, RegisterB);
            //    //0,3
            //    RegisterA /= 8;
            //    //1,7
            //    RegisterB ^= 7;
            //    //4,1
            //    RegisterB ^= RegisterC;
            //    //5,5
            //    Output.Add((int)(RegisterB % 8));
            //    //3,0
            //} while (RegisterA > 0);



        }

    }

    public class DebugComputer : Computer
    {
        private int outputLength = -1;

        public DebugComputer(long registerA, long registerB, long registerC, List<int> input) : base(registerA, registerB, registerC, input)
        {
            commandTable[5] = DebugOut;
        }

        public bool IsGood { get; private set; } = true;
        public void Process()
        {
            var inputLength = Input.Count;
            while (IsGood && InstructionPointer < inputLength)
            {
                var opCode = Input[InstructionPointer++];
                var operand = Input[InstructionPointer++];
                var command = commandTable[opCode];
                command(operand);
            }
        }
        protected void DebugOut(int comboOperand)
        {
            var value = TranslateComboOperand(comboOperand);
            Output.Add((int)(value % 8));
            ++outputLength;

            IsGood &= (Output[outputLength] == Input[outputLength]);
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



    public override string Part1(string[] input)
    {
        var (a, b, c, commands) = ParseInput(input);
        var shittyElfPc = new BasicComputer(a, b, c, commands);
        shittyElfPc.Process();
        var result = String.Join(',', shittyElfPc.Output);
        return result;
    }

    private void TestMinified(int b, int c, List<int> commands)
    {
        for (int i = 0; i < 500000; ++i)
        {
            var shittyElfPc = new BasicComputer(i, b, c, commands);
            var otherShittyElfPc = new CSharpComputer(i, b, c, commands);
            otherShittyElfPc.Process();
            shittyElfPc.Process();
            if (!shittyElfPc.Output.SequenceEqual(otherShittyElfPc.Output))
            {
            }
        }
    }

    private int[] CheckDigits(long registerA, List<int> commands)
    {
        var shittyElfPc = new BasicComputer(registerA, 0, 0, commands);
        shittyElfPc.Process();

        return shittyElfPc.Output.ToArray();
    }

    public override string Part2(string[] input)
    {
        var (a, b, c, commands) = ParseInput(input);
        //if (a == 2024)
        //{
        //    return "";
        //}
        //TestMinified(b, c, commands);
        // commands = [2, 4, 1, 7, 7, 5,5,0];
        var success = false;

        var testInput = 9;

        long registerA = 1;

        var digitToCheck = 1;
        var expectedData = commands.ToArray();

        while (digitToCheck < commands.Count + 1)
        {
            var outputDigits = CheckDigits(registerA, commands);
            //Console.WriteLine($"({registerA})  {String.Join(',', outputDigits)}");
            var outputDigit = outputDigits[^digitToCheck];
            var expectedDigit = expectedData[^digitToCheck];

            if (digitToCheck > 1 && outputDigits[^(digitToCheck - 1)] != expectedData[^(digitToCheck - 1)])
            {
                digitToCheck--;
                registerA /= 8;
            }
            else if (outputDigit == expectedDigit)
            {
                digitToCheck++;
                registerA *= 8; //this means we need to divide by 8 at the end!

            }
            else
            {
                registerA += 1;
            }
        }
        registerA /= 8;

        //while (Console.ReadLine() is { Length: > 0 } consoleInput)
        //{
        //    if (consoleInput.StartsWith("+"))
        //    {
        //        registerA += 1;
        //    }
        //    else if (consoleInput.StartsWith("-"))
        //    {
        //        registerA -= 1;
        //    }
        //    else if (consoleInput.StartsWith("*"))
        //    {
        //        registerA *= 8;
        //    }
        //    else if (consoleInput.StartsWith("/"))
        //    {
        //        registerA /= 8;
        //    }
        //    else
        //    {
        //        registerA = consoleInput.ToLong();
        //    }
        //    var shittyElfPc = new BasicComputer(registerA, b, c, commands);
        //    shittyElfPc.Process();
        //    var result = String.Join(',', shittyElfPc.Output);
        //    Console.WriteLine($"({registerA})  {result}");
        //}


        // var shittyElfPc = new Computer(117440, b, c, commands);
        //shittyElfPc.Process();
        //var result = String.Join(',', shittyElfPc.Output);
        //Console.WriteLine(result);
        return registerA.ToString();
    }


}