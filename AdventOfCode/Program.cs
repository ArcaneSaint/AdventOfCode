global using AdventOfCode.Helpers;
using System.Diagnostics;
using AdventOfCode.Solvers.Year2015;


// See https://aka.ms/new-console-template for more information
using Year2015 = AdventOfCode.Solvers.Year2015;
using Year2024 = AdventOfCode.Solvers.Year2024;




//new Year2015.Day1Solver(-3, 1).Solve();
//new Year2015.Day2Solver(58 + 43, 34 + 14).Solve();
//new Year2015.Day3Solver(2, 11).Solve();
//new Year2015.Day4Solver(609043, 6742839).Solve(); //slow
//new Year2015.Day5Solver(2, 2).Solve();
//new Year2015.Day6Solver(1000000 - 1000 - 4, 1000000 + 2000 - 4).Solve();
//new Year2015.Day7Solver(65079, 456).Solve();
//new Year2015.Day8Solver(12, 456).Solve();





Stopwatch watch = Stopwatch.StartNew();

new Year2024.Day1Solver(11, 31).Solve();
new Year2024.Day2Solver(2, 4).Solve();
new Year2024.Day3Solver(161, 48).Solve();
new Year2024.Day4Solver(18, 9).Solve();
new Year2024.Day5Solver(143, 123).Solve();
new Year2024.Day6Solver(41, 6).Solve();
new Year2024.Day7Solver(3749, 11387).Solve();
new Year2024.Day8Solver(14, 34).Solve();

watch.Stop();
using (new ColorOutputter(ConsoleColor.Cyan))
{
    Console.WriteLine($"Total time elapsed: {watch.Elapsed}");
}