using Solution1;

// Read input file from parent directory
string inputPath = @"c:\mjolner-code\AdventOfCode2025\Day03\input.txt";
string[] banks = File.ReadAllLines(inputPath);

// Part 1: Calculate total joltage with 2 batteries
int totalJoltagePart1 = BatteryBank.CalculateTotalJoltage(banks);
Console.WriteLine($"Part 1 - Total Joltage (2 batteries): {totalJoltagePart1}");

// Part 2: Calculate total joltage with 12 batteries
long totalJoltagePart2 = BatteryBank.CalculateTotalJoltage12(banks);
Console.WriteLine($"Part 2 - Total Joltage (12 batteries): {totalJoltagePart2}");
