namespace AdventOfCode.Solvers.Year2024;

public abstract class BaseSolver2024<T>(int day, T part1Test = default, T part2Test = default) : BaseSolver2024<T,T>(day, part1Test, part2Test)
{
}

public abstract class BaseSolver2024<T1,T2>(int day, T1 part1Test = default, T2 part2Test = default) : BaseSolver<T1,T2>(day, 2024, part1Test, part2Test)
{
}
