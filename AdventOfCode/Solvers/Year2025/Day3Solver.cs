namespace AdventOfCode.Solvers.Year2025;

internal class Day3Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2025<long>(3, part1Test, part2Test)
{
    public override long Part1(string[] input)
    {
        //return input.Sum(line => RecursiveNumber(line, 1));
        return input.Sum(FindBiggestTwoDigitNumber);
    }

    private int BruteForceMethodTwoDigitNumber(string input)
    {
        var result = 0;
        for (var i = 0; i < input.Length - 1; ++i)
        {
            for (var j = i + 1; j < input.Length; ++j)
            {
                var toComp = (input[i] - '0') * 10 + (input[j] - '0');
                if (toComp > result)
                {
                    result = toComp;
                }
            }
        }
        return result;
    }

    private int FindBiggestTwoDigitNumber(string input)
    {
        var x = '0';
        var y = '0';
        for (var i = 0; i < input.Length - 1; ++i)
        {
            if (input[i] > x)
            {
                x = input[i];
                y = '0';
            }
            if (input[i] == x && y != '9')
            {
                for (var j = i + 1; j < input.Length; ++j)
                {
                    if (input[j] > y)
                    {
                        y = input[j];
                    }
                }
            }
            if (x == '9' && y == '9')
            {
                return 99;
            }
        }
        return (x - '0') * 10 + (y - '0');
    }

    private long RecursiveNumber(string remainingInput, int remainingDigits)
    {
        if (remainingDigits < 0)
        {
            return 0;
        }

        var largest = '0';
        var largestIndex = 0;
        //get the largest digit
        for (var i = 0; i < remainingInput.Length - remainingDigits; ++i)
        {
            if (remainingInput[i] > largest)
            {
                largest = remainingInput[i];
                largestIndex = i + 1;
            }
        }

        long result = ((long)(largest - '0') * (long)Math.Pow(10L, remainingDigits));

        return result + RecursiveNumber(remainingInput[largestIndex..], remainingDigits - 1);
    }

    public override long Part2(string[] input)
    {
        return input.Sum(line => RecursiveNumber(line, 11));
    }
}