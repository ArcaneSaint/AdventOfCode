
namespace AdventOfCode.Solvers.Year2024;

internal class Day22Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2024<long>(22, part1Test, part2Test)
{
    private static Random rng = new Random();
    public class ElfNumberGenerator(long secret)
    {
        private int iteration = 0;
        public int Id { get; private set; } = rng.Next();
        public int Bananas { get; private set; }
        public int Change { get; private set; }
        // public List<int> ChangeSequence { get; private set; } = new();
        public Dictionary<long, int> TotalPerSequence { get; set; } = new();


        private int d1;
        private int d2;
        private int d3;
        private int d4;


        public long Next()
        {
            ++iteration;
            var r1 = secret * 64;
            secret = Mix(r1);
            secret = Prune(secret);
            var r2 = secret / 32;
            secret = Mix(r2);
            secret = Prune(secret);

            var r3 = secret * 2048;
            secret = Mix(r3);
            secret = Prune(secret);

            var nextBanana = (int)(secret % 10);
            Change = nextBanana - Bananas;
            (d1, d2, d3, d4) = (d2, d3, d4, Change);
            Bananas = nextBanana;
            UpdateSequence();

            return secret;
        }

        private void UpdateSequence()
        {
            if (iteration < 4)
            {
                return;
            }

            var key = d1 * 1000000
                    + d2 * 10000
                    + d3 * 100
                    + d4 * 1;

            if (!TotalPerSequence.ContainsKey(key))
            {
                TotalPerSequence[key] = Bananas;
            }
        }

        private long Prune(long secret)
        {
            return secret % 16777216;
        }

        private long Mix(long value)
        {
            return secret ^ value;
        }
    }

    public override long Part1(string[] input)
    {
        var result = 0L;
        foreach (var line in input)
        {
            var eng = new ElfNumberGenerator(line.ToLong());
            for (int i = 0; i < 1999; ++i)
            {
                eng.Next();
            }
            result += eng.Next();
        }

        return result;
    }


    public Dictionary<long, int> TotalPerSequence { get; set; } = new();
    public List<long> SequencesPerGenerator { get; set; } = new();

    public override long Part2(string[] input)
    {
        var generators = new List<ElfNumberGenerator>();
        foreach (var line in input)
        {
            generators.Add(new ElfNumberGenerator(line.ToLong()));
        }

      

        //now, skip the first 3, because we cannot do anything in the first three
        for (int i = 0; i < 2000; ++i)
        {
            foreach (var eng in generators)
            {
                eng.Next();
            }
        }

        var dictionaries = generators.Select(x => x.TotalPerSequence).ToList();
        var result = dictionaries.SelectMany(d => d)
            .GroupBy(kvp => kvp.Key, 
            (key, kvps) => new { Key = key, Value = kvps.Sum(kvp => kvp.Value) })
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);


        var max = result.OrderByDescending(x => x.Value).First();


        return max.Value;
    }
}
