using Solution1;

string inputPath = @"c:\mjolner-code\AdventOfCode2025\Day11\input.txt";
string[] lines = File.ReadAllLines(inputPath);

var graph = PathFinder.BuildGraph(lines);
int pathCount = PathFinder.CountPaths(graph, "you", "out");

Console.WriteLine($"Answer: {pathCount}");
