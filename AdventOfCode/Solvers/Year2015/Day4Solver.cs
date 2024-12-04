using System.Security.Cryptography;

namespace AdventOfCode.Solvers.Year2015;

internal class Day4Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2015(4, part1Test, part2Test)
{
    public override long Part1(string[] input)
    {
        var line = input[0];
        var result = Int32.MaxValue;
        
        Parallel.For(0, Int32.MaxValue, new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount
        },
        (i, state) =>
        {
            if (i > result) { return; }
            var hash = CreateMD5(line + i);
            if (hash.StartsWith("00000") && i < result)
            {
                result = i;
            }
        });

        return result;
    }

    public override long Part2(string[] input)
    {
        var line = input[0];
        var result = 0;
        var hash = "";
        do
        {
            result++;
            hash = CreateMD5(line + result);
        } while (!hash.StartsWith("000000"));

        return result;
    }


    public static string CreateMD5(string input)
    {
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = MD5.HashData(inputBytes);
        return Convert.ToHexString(hashBytes); // .NET 5 +
    }
}
