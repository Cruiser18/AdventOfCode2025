using Solution1;

// Read input file from parent directory
string inputPath = @"c:\mjolner-code\AdventOfCode2025\Day04\input.txt";
string[] grid = File.ReadAllLines(inputPath);

// Part 1: Count accessible rolls
int accessibleCount = ForkliftAccess.CountAccessibleRolls(grid);
Console.WriteLine($"Part 1 - Accessible rolls: {accessibleCount}");

// Part 2: Count total removable rolls
int totalRemovable = ForkliftAccess.CountTotalRemovableRolls(grid);
Console.WriteLine($"Part 2 - Total removable rolls: {totalRemovable}");
