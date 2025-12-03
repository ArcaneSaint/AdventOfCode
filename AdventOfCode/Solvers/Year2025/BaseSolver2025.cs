namespace AdventOfCode.Solvers.Year2025;

public abstract class BaseSolver2025<T>(int day, T part1Test = default, T part2Test = default) : BaseSolver2025<T,T>(day, part1Test, part2Test)
{
}

public abstract class BaseSolver2025<T1,T2>(int day, T1 part1Test = default, T2 part2Test = default) : BaseSolver<T1,T2>(day, 2025, part1Test, part2Test)
{
}
