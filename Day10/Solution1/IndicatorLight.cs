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
        
        // Reset machine-specific cache
        currentMachineButtons = "";
        
        // Use recursive approach based on Part 1 logic
        return SolveJoltageRecursive(targetJoltage, buttons);
    }

    public static long SolveTotalMinimumPressesForJoltage(string[] input)
    {
        return input.Sum(machine => SolveMinimumPressesForJoltage(machine));
    }

    // Cache needs to be per-machine since button configurations differ
    private static Dictionary<string, Dictionary<string, long>> joltageCachePerMachine = new Dictionary<string, Dictionary<string, long>>();
    private static string currentMachineButtons = "";

    private static long SolveJoltageRecursive(int[] target, List<int[]> buttons)
    {
        // Base case: all zeros
        if (target.All(x => x == 0))
        {
            return 0;
        }

        // Check if any target is negative
        if (target.Any(x => x < 0))
        {
            return long.MaxValue; // Impossible
        }

        // Create cache key based on buttons and target
        if (string.IsNullOrEmpty(currentMachineButtons))
        {
            currentMachineButtons = string.Join(";", buttons.Select(b => string.Join(",", b)));
        }
        
        if (!joltageCachePerMachine.ContainsKey(currentMachineButtons))
        {
            joltageCachePerMachine[currentMachineButtons] = new Dictionary<string, long>();
        }
        
        var cache = joltageCachePerMachine[currentMachineButtons];
        string targetKey = string.Join(",", target);
        if (cache.TryGetValue(targetKey, out long cached))
        {
            return cached;
        }

        // Compute the parity pattern we need (odd=1, even=0)
        int[] parityPattern = target.Select(x => x % 2).ToArray();

        // Use Part 1 GF(2) solver to find all button combinations that give this parity
        var validPatterns = FindAllButtonPatterns(parityPattern, buttons);

        if (validPatterns.Count == 0)
        {
            // No valid patterns found - this configuration is impossible
            cache[targetKey] = long.MaxValue;
            return long.MaxValue;
        }

        long minPresses = long.MaxValue;

        foreach (var pattern in validPatterns)
        {
            // Apply this pattern (press each button in the pattern once)
            int[] newTarget = (int[])target.Clone();

            int buttonsPressed = pattern.Count;
            foreach (int buttonIdx in pattern)
            {
                for (int i = 0; i < target.Length; i++)
                {
                    newTarget[i] -= buttons[buttonIdx][i];
                }
            }

            // Check if all are non-negative and even
            bool valid = true;
            for (int i = 0; i < newTarget.Length; i++)
            {
                if (newTarget[i] < 0 || newTarget[i] % 2 != 0)
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
            {
                // Divide by 2 and recurse
                int[] halved = newTarget.Select(x => x / 2).ToArray();
                long recursiveResult = SolveJoltageRecursive(halved, buttons);

                if (recursiveResult != long.MaxValue)
                {
                    long totalPresses = buttonsPressed + 2 * recursiveResult;
                    minPresses = Math.Min(minPresses, totalPresses);
                }
            }
        }

        cache[targetKey] = minPresses;
        return minPresses;
    }

    private static List<List<int>> FindAllButtonPatterns(int[] targetParity, List<int[]> buttons)
    {
        // Find all combinations of buttons (each pressed 0 or 1 times) that give targetParity
        int numButtons = buttons.Count;
        int numCounters = targetParity.Length;
        var validPatterns = new List<List<int>>();

        // Try all 2^numButtons combinations
        for (int mask = 0; mask < (1 << numButtons); mask++)
        {
            int[] result = new int[numCounters];
            var pattern = new List<int>();

            for (int b = 0; b < numButtons; b++)
            {
                if ((mask & (1 << b)) != 0)
                {
                    pattern.Add(b);
                    for (int i = 0; i < numCounters; i++)
                    {
                        result[i] ^= buttons[b][i]; // XOR for GF(2)
                    }
                }
            }

            // Check if this gives the target parity
            bool matches = true;
            for (int i = 0; i < numCounters; i++)
            {
                if (result[i] != targetParity[i])
                {
                    matches = false;
                    break;
                }
            }

            if (matches)
            {
                validPatterns.Add(pattern);
            }
        }

        return validPatterns;
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

    /* OLD ILP APPROACH - REPLACED WITH RECURSIVE PARITY METHOD
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
        int sumTarget = target.Sum();

        // For very complex cases with many free variables, use randomized search
        if (freeVars.Count > 7)
        {
            // Start with all zeros
            long[] testVars = new long[freeVars.Count];
            long sum = EvaluateSolution(testVars);
            if (sum < minSum) minSum = sum;

            // Randomized search with much more probes
            Random rnd = new Random(42);
            for (int probe = 0; probe < 2000000; probe++)
            {
                for (int i = 0; i < freeVars.Count; i++)
                {
                    // Try different strategies for different probes
                    if (probe < 500000)
                    {
                        testVars[i] = rnd.Next(0, Math.Min(maxTarget, 300));
                    }
                    else if (probe < 1000000)
                    {
                        testVars[i] = rnd.Next(0, Math.Min(sumTarget, 600));
                    }
                    else
                    {
                        testVars[i] = rnd.Next(0, 1000);
                    }
                }
                
                sum = EvaluateSolution(testVars);
                if (sum < minSum) minSum = sum;
            }

            // Hill climbing from best found so far
            for (int iter = 0; iter < 5000; iter++)
            {
                bool improved = false;
                for (int i = 0; i < freeVars.Count; i++)
                {
                    // Try ±1
                    for (int delta = -1; delta <= 1; delta++)
                    {
                        if (delta == 0) continue;
                        long oldVal = testVars[i];
                        testVars[i] = Math.Max(0, oldVal + delta);
                        
                        sum = EvaluateSolution(testVars);
                        if (sum < minSum && sum != long.MaxValue)
                        {
                            minSum = sum;
                            improved = true;
                        }
                        else
                        {
                            testVars[i] = oldVal;
                        }
                    }
                }
                if (!improved) break;
            }
        }
        else
        {
            // Branch and bound search for reasonable-sized problems
            void BranchAndBoundSearch(int idx, long[] values, long currentPartialSum)
            {
                if (idx == freeVars.Count)
                {
                    long sum = EvaluateSolution(values);
                    if (sum < minSum) minSum = sum;
                    return;
                }

                // Calculate upper bound for this free variable based on constraints
                long upperBound = Math.Max(maxTarget * 2, sumTarget);
                
                // For the first few free variables, use even larger bounds
                if (idx < 2)
                {
                    upperBound = Math.Min(upperBound * 2, 1000);
                }
                
                // Try to find a tighter bound by looking at the constraints
                for (int row = 0; row < currentRow; row++)
                {
                    int pivotCol = pivotCols[row];
                    if (pivotCol == -1) continue;
                    
                    // Get coefficient of current free variable in this equation
                    double coeff = aug[row, freeVars[idx]];
                    if (Math.Abs(coeff) < 0.001) continue;
                    
                    // Calculate what the RHS would be with current assignments
                    double rhs = aug[row, numButtons];
                    for (int i = 0; i < idx; i++)
                    {
                        rhs -= aug[row, freeVars[i]] * values[i];
                    }
                    
                    // If coeff is positive, increasing this free var could help
                    if (coeff > 0.001)
                    {
                        long bound = (long)Math.Ceiling((rhs + maxTarget) / coeff);
                        if (bound > 0 && bound < upperBound)
                        {
                            upperBound = bound;
                        }
                    }
                }
                
                // Ensure minimum exploration
                upperBound = Math.Max(upperBound, 50);

                for (long v = 0; v <= upperBound; v++)
                {
                    // Prune if adding even the minimum possible from remaining variables exceeds current best
                    long estimatedMin = currentPartialSum + v;
                    if (estimatedMin >= minSum) break;

                    values[idx] = v;
                    BranchAndBoundSearch(idx + 1, values, currentPartialSum + v);
                    
                    // Additional pruning
                    if (minSum < long.MaxValue && v > 0)
                    {
                        long recentBest = EvaluateSolution(values);
                        if (recentBest == long.MaxValue && v > 10)
                        {
                            break;
                        }
                    }
                }
            }

            BranchAndBoundSearch(0, new long[freeVars.Count], 0);
        }

        return minSum;
    }
    */
}
