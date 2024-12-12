using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Helpers;

internal static class GridHelpers
{
    internal static bool IsInBounds<T>(this T[,] data, (int i, int j) up)
    {
        return up.i >= 0
               && up.j >= 0
               && up.i < data.GetLength(0)
               && up.j < data.GetLength(1);
    }

    internal static T GetItem<T>(this T[,] data, (int i, int j) coordinate)
    {
        return data[coordinate.i, coordinate.j];
    }
}

