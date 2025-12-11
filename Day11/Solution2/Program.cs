using Solution2;

try
{
    string inputPath = @"c:\mjolner-code\AdventOfCode2025\Day11\input.txt";
    string[] lines = File.ReadAllLines(inputPath);
    
    Console.WriteLine($"Read {lines.Length} lines");
    
    var graph = PathFinder.BuildGraph(lines);
    Console.WriteLine($"Built graph with {graph.Count} nodes");
    
    var requiredNodes = new HashSet<string> { "dac", "fft" };
    Console.WriteLine("Starting path counting...");
    
    long pathCount = PathFinder.CountPathsWithRequiredNodes(graph, "svr", "out", requiredNodes);
    
    Console.WriteLine($"Answer: {pathCount}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine($"Stack: {ex.StackTrace}");
}
