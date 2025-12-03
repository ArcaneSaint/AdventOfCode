using System.Text;

namespace AdventOfCode.Solvers.Year2015;

internal class Day11Solver(string part1Test, string part2Test) : BaseSolver<string, string>(11, 2015, part1Test, part2Test)
{
    public override string Part1(string[] input)
    {
        var password = input[0];
        password = FindNextValidPassword(password);
        return password;
    }

    public override string Part2(string[] input)
    {
        var password = input[0];
        password = FindNextValidPassword(password);
        password = FindNextValidPassword(password);
        return password;
    }

    private string FindNextValidPassword(string password)
    {
        password = NextPassword(password);
        while (!IsValid(password))
        {
            password = NextPassword(password);
        }
        return password;
    }

    private bool IsValid(string password)
    {
        return Rule1(password)
             && Rule2(password)
             && Rule3(password);
    }

    private bool Rule1(string password)
    {
        for (var i = 0; i < password.Length - 2; ++i)
        {
            if (password[i] == password[i + 1] - 1 && password[i + 1] == password[i + 2] - 1)
            {
                return true;
            }
        }
        return false;
    }
    private bool Rule2(string password) => !password.Any(x => x == 'i' || x == 'o' || x == 'l');
    private bool Rule3(string password)
    {
        for (var i = 0; i < password.Length - 1; ++i)
        {
            if (password[i] == password[i + 1] && Rule3B(password, password[i]))
            {
                return true;
            }
        }
        return false;
    }
    private bool Rule3B(string password, char prev)
    {
        for (var i = 0; i < password.Length - 1; ++i)
        {
            if (password[i] == password[i + 1] && password[i] != prev)
            {
                return true;
            }
        }
        return false;
    }

    private string NextPassword(string input)
    {
        var arr = input.ToCharArray();
        for (var n = arr.Length - 1; n >= 0; n--)
        {
            if (arr[n] == 'z')
            {
                arr[n] = 'a';
            }
            else
            {
                arr[n] = (char)(arr[n] + 1);
                return new string(arr);
            }
        }
        return new string(arr);
    }
}
