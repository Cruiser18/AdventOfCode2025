using Solution1;

// Read input file from Day02 directory
string inputPath = @"c:\mjolner-code\AdventOfCode2025\Day02\input.txt";

// Read all input as a single string (ranges are comma-separated, possibly multi-line)
string input = File.ReadAllText(inputPath).Replace("\n", "").Replace("\r", "");

// Calculate the sum of all invalid IDs
long result = InvalidIdFinder.SumInvalidIds(input);

Console.WriteLine($"Sum of all invalid IDs: {result}");
