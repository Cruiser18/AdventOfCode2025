using Solution1;

string[] input = File.ReadAllLines("../input.txt");

// Part 1: Indicator lights (GF(2) system)
int result1 = IndicatorLight.SolveTotalMinimumPresses(input);
Console.WriteLine($"Part 1 - Total minimum button presses for lights: {result1}");

// Part 2: Joltage counters (Integer linear system)
try
{
    long total = 0;
    for (int i = 0; i < input.Length; i++)
    {
        long result = IndicatorLight.SolveMinimumPressesForJoltage(input[i]);
        if (result == long.MaxValue)
        {
            Console.WriteLine($"Warning: Machine {i + 1} returned no solution within search bounds");
            Console.WriteLine($"  Machine: {input[i].Substring(0, Math.Min(80, input[i].Length))}...");
        }
        else
        {
            total += result;
        }
    }
    Console.WriteLine($"Part 2 - Total minimum button presses for joltage: {total}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error in Part 2: {ex.Message}");
}
