using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Helpers;

internal class ColorOutputter : IDisposable
{
    private readonly ConsoleColor previous;

    public ColorOutputter(ConsoleColor color)
    {
        previous = Console.ForegroundColor;
        Console.ForegroundColor = color;
    }

    public void Dispose() => Console.ForegroundColor = previous;

}

