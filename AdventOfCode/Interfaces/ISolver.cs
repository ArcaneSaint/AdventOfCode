namespace AdventOfCode.Interfaces;

public interface ISolver<T1, T2>
{
    public T1 Part1(string[] input);
    public T2 Part2(string[] input);

    abstract string? AdditionalInfo { get; }
}

