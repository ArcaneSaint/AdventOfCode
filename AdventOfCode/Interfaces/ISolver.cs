namespace AdventOfCode.Interfaces;

public interface ISolver
{
    public long Part1(string[] input);
    public long Part2(string[] input);

    abstract string? AdditionalInfo { get; }
}

