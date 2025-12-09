using Solution1;

// Test with example input first
string[] testInput = File.ReadAllLines("../testinput.txt");
long testResult = RedTile.FindLargestRectangle(testInput);
Console.WriteLine($"Test result: {testResult}");
Console.WriteLine();

// Part 1: Find largest rectangle area using red tiles as opposite corners
Console.WriteLine("Reading real input...");
string[] input = File.ReadAllLines("../input.txt");
Console.WriteLine($"Processing {input.Length} red tiles...");
long result = RedTile.FindLargestRectangle(input);
Console.WriteLine($"Largest rectangle area: {result}");

// Part 2: Find largest rectangle using only red/green tiles (within polygon)
Console.WriteLine();
Console.WriteLine("Part 2 - Finding largest rectangle within polygon...");
long result2 = RedTile.FindLargestRectangleInPolygon(input);
Console.WriteLine($"Largest rectangle area within polygon: {result2}");
