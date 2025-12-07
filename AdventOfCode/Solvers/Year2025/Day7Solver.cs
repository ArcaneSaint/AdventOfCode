using System.Linq;

namespace AdventOfCode.Solvers.Year2025;

internal class Day7Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2025<long>(7, part1Test, part2Test)
{

    public override long Part1(string[] input)
    {
        var beamGrid = CreateBeamGrid(input);
        return ProcessBeams(beamGrid, input);
    }


    private long ProcessBeams(bool[,] beamGrid, string[] input)
    {
        var count = 0L;
        for (var i = 1; i < input.Length; i++)
        {
            for (var j = 0; j < input[0].Length; j++)
            {
                if (beamGrid[i - 1, j])
                {
                    //we are hit by a beam.
                    if (input[i][j] == '^')
                    {
                        //split
                        beamGrid[i, j - 1] = true;
                        beamGrid[i, j + 1] = true;
                        ++count;
                    }
                    else
                    {
                        //no split
                        beamGrid[i, j] = true;
                    }
                }
            }
        }
        return count;
    }

    private bool[,] CreateBeamGrid(string[] input)
    {
        var result = new bool[input.Length, input[0].Length];
        result[0, input[0].IndexOf('S')] = true;
        return result;
    }

    private long[,] CreateTimelineGrid(string[] input)
    {
        var result = new long[input.Length, input[0].Length];
        result[0, input[0].IndexOf('S')] = 1;
        return result;
    }
    private long ProcessTimelines(long[,] beamGrid, string[] input)
    {
        for (var i = 1; i < input.Length; i++)
        {
            for (var j = 0; j < input[0].Length; j++)
            {
                //are we a splitter?
                if (input[i][j] == '^')
                {
                    //split
                    beamGrid[i, j - 1] += beamGrid[i-1,j];
                    beamGrid[i, j + 1] += beamGrid[i - 1, j];
                }
                else
                {
                    //no split
                    beamGrid[i, j] += beamGrid[i - 1, j];
                }
            }
        }

        var n = input.Length;
        var count = 0L;
        for (var i = 0; i < input[0].Length; i++)
        {
            count += beamGrid[n - 1, i];
        }
        return count;
    }

    public override long Part2(string[] input)
    {
        var timelineGrid = CreateTimelineGrid(input);
        return ProcessTimelines(timelineGrid, input);
    }
}