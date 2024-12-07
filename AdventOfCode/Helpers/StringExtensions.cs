namespace AdventOfCode.Helpers;

internal static class StringExtensions
{
    internal static int ToInt(this string str)
    {
        return int.Parse(str);
    }
    internal static long ToLong(this string str)
    {
        return long.Parse(str);
    }
    internal static ushort ToUInt16(this string str) { return ushort.Parse(str); }
}

