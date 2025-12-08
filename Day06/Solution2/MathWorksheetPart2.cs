namespace Solution2;

public class Problem
{
    public int[] Numbers { get; set; } = Array.Empty<int>();
    public char Operator { get; set; }
}

public class MathWorksheetPart2
{
    public List<Problem> Problems { get; private set; } = new();

    public void ParseWorksheet(string[] lines)
    {
        if (lines.Length == 0) return;

        // Find the maximum line length to handle all columns
        int maxLength = lines.Max(l => l.Length);

        // We'll collect all column groups, then reverse them for right-to-left processing
        List<(int startCol, int endCol)> columnRanges = new();

        // First pass: identify all column ranges (problem boundaries)
        int col = 0;
        while (col < maxLength)
        {
            // Skip separator columns (all spaces)
            bool isAllSpaces = true;
            for (int row = 0; row < lines.Length; row++)
            {
                if (col < lines[row].Length && lines[row][col] != ' ')
                {
                    isAllSpaces = false;
                    break;
                }
            }

            if (isAllSpaces)
            {
                col++;
                continue;
            }

            // Found start of a column group
            int startCol = col;

            // Expand to include all connected non-space characters
            while (col < maxLength)
            {
                bool hasContent = false;
                for (int row = 0; row < lines.Length; row++)
                {
                    if (col < lines[row].Length && lines[row][col] != ' ')
                    {
                        hasContent = true;
                        break;
                    }
                }

                if (!hasContent) break;
                col++;
            }

            columnRanges.Add((startCol, col));
        }

        // Process problems right-to-left
        columnRanges.Reverse();

        foreach (var (startCol, endCol) in columnRanges)
        {
            // For each problem, read digit columns right-to-left
            // Each digit column forms one number by reading top-to-bottom
            List<int> numbers = new();
            char? operatorChar = null;

            // Read columns right-to-left within this problem
            for (int c = endCol - 1; c >= startCol; c--)
            {
                // Read this column top-to-bottom to form a number (or find operator)
                string digitColumn = "";
                
                for (int row = 0; row < lines.Length - 1; row++)  // Exclude operator row
                {
                    if (c < lines[row].Length && lines[row][c] != ' ')
                    {
                        digitColumn += lines[row][c];
                    }
                }

                // If this column has digits, it forms a number
                if (!string.IsNullOrEmpty(digitColumn))
                {
                    numbers.Add(int.Parse(digitColumn));
                }
            }

            // Get operator from last row
            for (int c = startCol; c < endCol; c++)
            {
                if (c < lines[lines.Length - 1].Length && lines[lines.Length - 1][c] != ' ')
                {
                    operatorChar = lines[lines.Length - 1][c];
                    break;
                }
            }

            if (operatorChar.HasValue && numbers.Count > 0)
            {
                Problems.Add(new Problem
                {
                    Numbers = numbers.ToArray(),
                    Operator = operatorChar.Value
                });
            }
        }
    }

    public static long CalculateProblem(Problem problem)
    {
        if (problem.Numbers.Length == 0) return 0;

        long result = problem.Numbers[0];

        for (int i = 1; i < problem.Numbers.Length; i++)
        {
            if (problem.Operator == '+')
            {
                result += problem.Numbers[i];
            }
            else if (problem.Operator == '*')
            {
                result *= problem.Numbers[i];
            }
        }

        return result;
    }

    public long GetGrandTotal()
    {
        return Problems.Sum(p => CalculateProblem(p));
    }
}
