using Solution1;

string[] input = File.ReadAllLines("../input.txt");

// Part 1: Indicator lights (GF(2) system)
int result1 = IndicatorLight.SolveTotalMinimumPresses(input);
Console.WriteLine($"Part 1 - Total minimum button presses for lights: {result1}");

// Part 2: Joltage counters (Recursive parity-based system)
long result2 = IndicatorLight.SolveTotalMinimumPressesForJoltage(input);
Console.WriteLine($"Part 2 - Total minimum button presses for joltage: {result2}");
