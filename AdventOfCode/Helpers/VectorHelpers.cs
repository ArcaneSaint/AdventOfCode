namespace AdventOfCode.Helpers;

internal class VectorHelpers
{
    public static (int X, int Y) CalculateVector(int X1, int Y1, int X2, int Y2)
    {
        return (X: X1 - X2, Y: Y1 - Y2);
    }
}

