using AdventOfCode.Helpers.Enums;

namespace AdventOfCode.Helpers;

internal static class TupleHelper
{
    internal static (int row, int col) Offset(this (int row, int col) coordinates, Direction direction)
    {
        switch (direction)
        {
            default: return coordinates;
            case Direction.Up: return (coordinates.row - 1, coordinates.col);
            case Direction.Down: return (coordinates.row + 1, coordinates.col);
            case Direction.Left: return (coordinates.row, coordinates.col - 1);
            case Direction.Right: return (coordinates.row, coordinates.col + 1);
        }
    }
}

