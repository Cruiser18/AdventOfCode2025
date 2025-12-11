using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Solution1;

public class IndicatorLight
{
    public static int SolveMinimumPresses(string machine)
    {
        // Parse the machine configuration
        var (targetLights, buttons) = ParseMachine(machine);
        
        // Solve using Gaussian elimination over GF(2)
        return SolveGF2(targetLights, buttons);
    }

    public static int SolveTotalMinimumPresses(string[] input)
    {
        return input.Sum(machine => SolveMinimumPresses(machine));
    }

    private static (bool[] target, List<bool[]> buttons) ParseMachine(string machine)
    {
        // Extract lights diagram [.##.]
        var lightsMatch = Regex.Match(machine, @"\[([.#]+)\]");
        string lightsStr = lightsMatch.Groups[1].Value;
        bool[] target = lightsStr.Select(c => c == '#').ToArray();

        // Extract all button configurations (x,y,z)
        var buttonMatches = Regex.Matches(machine, @"\(([0-9,]+)\)");
        var buttons = new List<bool[]>();

        foreach (Match match in buttonMatches)
        {
            var indices = match.Groups[1].Value.Split(',').Select(int.Parse).ToArray();
            var button = new bool[target.Length];
            foreach (int idx in indices)
            {
                button[idx] = true;
            }
            buttons.Add(button);
        }

        return (target, buttons);
    }

    private static int SolveGF2(bool[] target, List<bool[]> buttons)
    {
        int numLights = target.Length;
        int numButtons = buttons.Count;

        // Create augmented matrix [A | b] where A is button effects, b is target state
        // Each row represents a light, each column represents a button
        bool[,] matrix = new bool[numLights, numButtons + 1];

        for (int light = 0; light < numLights; light++)
        {
            for (int button = 0; button < numButtons; button++)
            {
                matrix[light, button] = buttons[button][light];
            }
            matrix[light, numButtons] = target[light]; // augmented column
        }

        // Gaussian elimination over GF(2)
        int[] pivotCol = new int[numLights];
        for (int i = 0; i < numLights; i++) pivotCol[i] = -1;

        int row = 0;
        for (int col = 0; col < numButtons && row < numLights; col++)
        {
            // Find pivot
            int pivotRow = -1;
            for (int r = row; r < numLights; r++)
            {
                if (matrix[r, col])
                {
                    pivotRow = r;
                    break;
                }
            }

            if (pivotRow == -1) continue; // No pivot in this column

            // Swap rows
            if (pivotRow != row)
            {
                for (int c = 0; c <= numButtons; c++)
                {
                    bool temp = matrix[row, c];
                    matrix[row, c] = matrix[pivotRow, c];
                    matrix[pivotRow, c] = temp;
                }
            }

            pivotCol[row] = col;

            // Eliminate
            for (int r = 0; r < numLights; r++)
            {
                if (r != row && matrix[r, col])
                {
                    for (int c = 0; c <= numButtons; c++)
                    {
                        matrix[r, c] ^= matrix[row, c]; // XOR in GF(2)
                    }
                }
            }

            row++;
        }

        // Check for inconsistency (no solution)
        for (int r = row; r < numLights; r++)
        {
            if (matrix[r, numButtons])
            {
                return -1; // No solution
            }
        }

        // Identify pivot and free variables
        bool[] isPivot = new bool[numButtons];
        for (int r = 0; r < row; r++)
        {
            if (pivotCol[r] != -1)
            {
                isPivot[pivotCol[r]] = true;
            }
        }

        List<int> freeVars = new List<int>();
        for (int i = 0; i < numButtons; i++)
        {
            if (!isPivot[i])
            {
                freeVars.Add(i);
            }
        }

        // Enumerate all possible assignments to free variables
        int minPresses = int.MaxValue;
        int numFreeVars = freeVars.Count;
        int numCombinations = 1 << numFreeVars; // 2^numFreeVars

        for (int combo = 0; combo < numCombinations; combo++)
        {
            bool[] solution = new bool[numButtons];

            // Set free variables according to this combination
            for (int i = 0; i < numFreeVars; i++)
            {
                solution[freeVars[i]] = ((combo >> i) & 1) == 1;
            }

            // Back substitute to determine pivot variables
            for (int r = row - 1; r >= 0; r--)
            {
                int col = pivotCol[r];
                if (col != -1)
                {
                    bool value = matrix[r, numButtons];
                    for (int c = 0; c < numButtons; c++)
                    {
                        if (c != col && matrix[r, c])
                        {
                            value ^= solution[c];
                        }
                    }
                    solution[col] = value;
                }
            }

            // Count button presses for this solution
            int presses = solution.Count(pressed => pressed);
            minPresses = Math.Min(minPresses, presses);
        }

        return minPresses;
    }

    public static long SolveMinimumPressesForJoltage(string machine)
    {
        // Parse the machine configuration for joltage
        var (targetJoltage, buttons) = ParseMachineForJoltage(machine);
        
        // Solve using integer linear programming
        return SolveIntegerLP(targetJoltage, buttons);
    }

    public static long SolveTotalMinimumPressesForJoltage(string[] input)
    {
        return input.Sum(machine => SolveMinimumPressesForJoltage(machine));
    }

    private static (int[] target, List<int[]> buttons) ParseMachineForJoltage(string machine)
    {
        // Extract joltage requirements {3,5,4,7}
        var joltageMatch = Regex.Match(machine, @"\{([0-9,]+)\}");
        var target = joltageMatch.Groups[1].Value.Split(',').Select(int.Parse).ToArray();

        // Extract all button configurations (x,y,z)
        var buttonMatches = Regex.Matches(machine, @"\(([0-9,]+)\)");
        var buttons = new List<int[]>();

        foreach (Match match in buttonMatches)
        {
            var indices = match.Groups[1].Value.Split(',').Select(int.Parse).ToArray();
            var button = new int[target.Length];
            foreach (int idx in indices)
            {
                button[idx] = 1;
            }
            buttons.Add(button);
        }

        return (target, buttons);
    }

    private static long SolveIntegerLP(int[] target, List<int[]> buttons)
    {
        int numCounters = target.Length;
        int numButtons = buttons.Count;

        // Build the coefficient matrix
        double[,] A = new double[numCounters, numButtons];
        for (int i = 0; i < numCounters; i++)
        {
            for (int j = 0; j < numButtons; j++)
            {
                A[i, j] = buttons[j][i];
            }
        }

        // Create augmented matrix
        double[,] aug = new double[numCounters, numButtons + 1];
        for (int i = 0; i < numCounters; i++)
        {
            for (int j = 0; j < numButtons; j++)
            {
                aug[i, j] = A[i, j];
            }
            aug[i, numButtons] = target[i];
        }

        // Gaussian elimination with partial pivoting
        int[] pivotCols = new int[numCounters];
        for (int i = 0; i < numCounters; i++) pivotCols[i] = -1;

        int currentRow = 0;
        for (int col = 0; col < numButtons && currentRow < numCounters; col++)
        {
            // Find best pivot
            int bestRow = -1;
            double bestValue = 0;
            for (int row = currentRow; row < numCounters; row++)
            {
                if (Math.Abs(aug[row, col]) > bestValue)
                {
                    bestValue = Math.Abs(aug[row, col]);
                    bestRow = row;
                }
            }

            if (bestRow == -1) continue;

            // Swap rows
            for (int c = 0; c <= numButtons; c++)
            {
                double temp = aug[currentRow, c];
                aug[currentRow, c] = aug[bestRow, c];
                aug[bestRow, c] = temp;
            }

            pivotCols[currentRow] = col;

            // Scale row
            double scale = aug[currentRow, col];
            for (int c = 0; c <= numButtons; c++)
            {
                aug[currentRow, c] /= scale;
            }

            // Eliminate
            for (int row = 0; row < numCounters; row++)
            {
                if (row != currentRow)
                {
                    double factor = aug[row, col];
                    for (int c = 0; c <= numButtons; c++)
                    {
                        aug[row, c] -= factor * aug[currentRow, c];
                    }
                }
            }

            currentRow++;
        }

        // Find free variables
        bool[] isFree = new bool[numButtons];
        for (int i = 0; i < numButtons; i++) isFree[i] = true;
        for (int i = 0; i < currentRow; i++)
        {
            if (pivotCols[i] != -1)
            {
                isFree[pivotCols[i]] = false;
            }
        }

        List<int> freeVars = new List<int>();
        for (int i = 0; i < numButtons; i++)
        {
            if (isFree[i]) freeVars.Add(i);
        }

        // Helper to evaluate a solution
        long EvaluateSolution(long[] freeVarValues)
        {
            double[] solution = new double[numButtons];
            for (int i = 0; i < freeVars.Count; i++)
            {
                solution[freeVars[i]] = freeVarValues[i];
            }

            for (int row = 0; row < currentRow; row++)
            {
                int pivotCol = pivotCols[row];
                if (pivotCol != -1)
                {
                    double val = aug[row, numButtons];
                    for (int col = 0; col < numButtons; col++)
                    {
                        if (col != pivotCol)
                        {
                            val -= aug[row, col] * solution[col];
                        }
                    }
                    solution[pivotCol] = val;
                }
            }

            // Check validity
            for (int i = 0; i < numButtons; i++)
            {
                if (solution[i] < -0.001 || Math.Abs(solution[i] - Math.Round(solution[i])) > 0.001)
                {
                    return long.MaxValue; // Invalid
                }
            }

            return solution.Select(x => (long)Math.Round(x)).Sum();
        }

        if (freeVars.Count == 0)
        {
            // No free variables, compute unique solution
            return EvaluateSolution(new long[0]);
        }

        // Use a smarter search strategy based on number of free variables
        long minSum = long.MaxValue;
        int maxTarget = target.Max();

        if (freeVars.Count <= 3)
        {
            // Exhaustive search for small number of free variables
            int searchDepth = freeVars.Count == 1 ? Math.Min(maxTarget * 2, 500) :
                             (freeVars.Count == 2 ? Math.Min(maxTarget, 250) :
                              Math.Min(maxTarget / 2, 150));

            void SearchExhaustive(int idx, long[] values)
            {
                if (idx == freeVars.Count)
                {
                    long sum = EvaluateSolution(values);
                    if (sum < minSum) minSum = sum;
                    return;
                }

                for (long v = 0; v <= searchDepth; v++)
                {
                    values[idx] = v;
                    
                    // Prune if partial sum already exceeds best
                    if (values.Take(idx + 1).Sum() < minSum)
                    {
                        SearchExhaustive(idx + 1, values);
                    }
                }
            }

            SearchExhaustive(0, new long[freeVars.Count]);
        }
        else
        {
            // For many free variables, use greedy + local search
            long[] bestFreeVars = new long[freeVars.Count];
            
            // Start with all zeros
            long currentBest = EvaluateSolution(bestFreeVars);
            if (currentBest < minSum) minSum = currentBest;

            // Try incrementing each free variable individually and take the best
            int maxIterations = Math.Min(100, maxTarget);
            for (int iter = 0; iter < maxIterations; iter++)
            {
                bool improved = false;
                
                for (int i = 0; i < freeVars.Count; i++)
                {
                    // Try incrementing this free variable
                    bestFreeVars[i]++;
                    long newSum = EvaluateSolution(bestFreeVars);
                    
                    if (newSum < minSum && newSum != long.MaxValue)
                    {
                        minSum = newSum;
                        improved = true;
                    }
                    else
                    {
                        // Revert if no improvement
                        bestFreeVars[i]--;
                    }
                }
                
                if (!improved) break; // Local optimum reached
            }

            // Also try some random probing with wider range
            Random rnd = new Random(42);
            int probes = Math.Min(10000, maxTarget * 10);
            for (int probe = 0; probe < probes; probe++)
            {
                long[] testVars = new long[freeVars.Count];
                for (int i = 0; i < freeVars.Count; i++)
                {
                    testVars[i] = rnd.Next(0, Math.Min(maxTarget, 500));
                }
                
                long sum = EvaluateSolution(testVars);
                if (sum < minSum) minSum = sum;
            }
        }

        return minSum;
    }
}
