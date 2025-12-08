using Solution1;

// Read input file from parent directory
string inputPath = @"c:\mjolner-code\AdventOfCode2025\Day05\input.txt";
string[] input = File.ReadAllLines(inputPath);

// Part 1: Count fresh ingredients
int freshCount = IngredientDatabase.CountFreshIngredients(input);
Console.WriteLine($"Part 1 - Fresh ingredients: {freshCount}");

// Part 2: Count total fresh IDs covered by ranges
long totalFreshIds = IngredientDatabase.CountTotalFreshIds(input);
Console.WriteLine($"Part 2 - Total fresh IDs: {totalFreshIds}");
