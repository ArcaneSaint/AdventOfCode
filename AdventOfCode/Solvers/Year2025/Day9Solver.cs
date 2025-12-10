using System;
using System.Data;

namespace AdventOfCode.Solvers.Year2025;

internal class Day9Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2025<long>(9, part1Test, part2Test)
{
    private long Area((int X, int Y) point1, (int X, int Y) point2)
    {
        return (Math.Abs(point2.X - point1.X) + 1L)
            * (Math.Abs(point2.Y - point1.Y) + 1L);
    }
    private List<(int X, int Y)> ParseInput(string[] input)
    {
        var result = new List<(int X, int Y)>();

        foreach (var line in input)
        {
            var data = line.Split(',');
            result.Add((data[0].ToInt(), data[1].ToInt()));
        }

        return result;
    }
    public override long Part1(string[] input)
    {
        var allPoints = ParseInput(input);
        int id = 0;

        Dictionary<double, ((int, int), (int, int))> pointsGroupedByArea = new();
        for (var i = 0; i < allPoints.Count; ++i)
        {
            for (var j = i + 1; j < allPoints.Count; ++j)
            {
                var area = Area(allPoints[i], allPoints[j]);
                pointsGroupedByArea[area] = ((allPoints[i], allPoints[j]));
            }
        }

        return (long)pointsGroupedByArea.Keys.Max();
    }

    public override long Part2(string[] input)
    {
        var allPoints = ParseInput(input);
        var edges = PointsToEdges(allPoints);


        int id = 0;


        var maxX = allPoints.Select(x => x.X).Max();
        var maxY = allPoints.Select(x => x.Y).Max();

        long maxArea = 0;



        for (var i = 0; i < allPoints.Count; ++i)
        {
            for (var j = i + 1; j < allPoints.Count; ++j)
            {
                var p1 = allPoints[i];
                var p2 = allPoints[j];


                if (Area(p1, p2) == 30)
                {

                }
                var startX = Math.Min(p1.X, p2.X);
                var startY = Math.Min(p1.Y, p2.Y);
                var endX = Math.Max(p1.X, p2.X);
                var endY = Math.Max(p1.Y, p2.Y);

                if (!(HasIntersection((startX, startY), (endX, startY), edges)
                    || HasIntersection((startX, startY), (startX, endY), edges)
                    || HasIntersection((endX, startY), (endX, endY), edges)
                    || HasIntersection((startX, endY), (endX, endY), edges)))
                {
                    var area = Area(p1, p2);
                    if (area > maxArea)
                    {
                        maxArea = area;
                    }
                }
            }
        }


        return maxArea;

    }

    private bool HasIntersection((int X, int Y) point1, (int X, int Y) point2, List<((int X, int Y) Point1, (int X, int Y) Point2)> edges)
    {
        var line = PointsToEdge(point1, point2);
        if (line == ((2, 3), (9, 3)))
        {

        }
        if (!PointIsWithinBoundaries(point1, edges) || !PointIsWithinBoundaries(point2, edges))
        {
            return true;
        }

        var isVertical = line.Point1.X == line.Point2.X;

        if (isVertical)
        {
            var collide = edges.Where(edge =>
                        edge != line && edge.Point1.Y == edge.Point2.Y
                        && ((edge.Point1.X < line.Point1.X
                        && edge.Point2.X > line.Point1.X)

                        && (edge.Point1.Y > line.Point1.Y
                        && edge.Point1.Y < line.Point2.Y))
                       ).ToList();

            return collide.Any();
        }
        else
        {
            var collide = edges.Where(edge =>
                        edge != line && edge.Point1.X == edge.Point2.X
                        && ((edge.Point1.Y < line.Point1.Y
                        && edge.Point2.Y > line.Point1.Y)

                        && (edge.Point1.X > line.Point1.X
                        && edge.Point1.X < line.Point2.X))

           ).ToList();

            return collide.Any();
        }


    }
    private bool PointIsWithinBoundaries((int X, int Y) point, List<((int X, int Y) Point1, (int X, int Y) Point2)> edges)
    {
        var collides = edges.Where(edge =>
            edge.Point1.X <= point.X && edge.Point1.X != edge.Point2.X
            && edge.Point1.Y <= point.Y
            && edge.Point2.Y >= point.Y
        ).ToList();


        return collides.Count % 2 != 0;
    }

    private List<((int X, int Y) Point1, (int X, int Y) Point2)> PointsToEdges(List<(int X, int Y)> allPoints)
    {
        var result = new List<((int X, int Y) Point1, (int X, int Y) Point2)>();
        for (int i = 0; i < allPoints.Count - 1; i++)
        {
            result.Add(PointsToEdge(allPoints[i], allPoints[i + 1]));
        }
        result.Add(PointsToEdge(allPoints.Last(), allPoints.First()));
        return result;
    }
    private ((int X, int Y) Point1, (int X, int Y) Point2) PointsToEdge((int X, int Y) point1, (int X, int Y) point2)
    {
        if (point1.X > point2.X || point1.Y > point2.Y)
        {
            return (point2, point1);
        }
        return (point1, point2);
    }
}