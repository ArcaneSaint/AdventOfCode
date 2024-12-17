namespace AdventOfCode.Interfaces;

public interface ISolver<T>
{
    public T Part1(string[] input);
    public T Part2(string[] input);

    abstract string? AdditionalInfo { get; }
}

