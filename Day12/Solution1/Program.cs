using Solution1;

// Read input
var inputPath = @"c:\mjolner-code\AdventOfCode2025\Day12\input.txt";
var lines = File.ReadAllLines(inputPath);

// Parse shapes and regions
var (shapes, regions) = ShapeParser.ParseInput(lines);

Console.WriteLine($"Parsed {shapes.Count} shapes and {regions.Count} regions");
Console.WriteLine();

// Solve each region
int solvableCount = 0;
for (int i = 0; i < regions.Count; i++)
{
    var region = regions[i];
    Console.WriteLine($"Starting Region {i + 1}/{regions.Count}: {region.Width}x{region.Height} with {region.ShapeIds.Count} shapes...");
    
    var canSolve = RegionSolver.Solve(shapes, region);
    
    Console.WriteLine($"  Result: {(canSolve ? "YES" : "NO")}");
    
    if (canSolve)
    {
        solvableCount++;
    }
}

Console.WriteLine();
Console.WriteLine($"Answer: {solvableCount} regions can fit all required presents");
