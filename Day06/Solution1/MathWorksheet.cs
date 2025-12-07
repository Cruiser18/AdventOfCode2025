namespace Solution1;

public class Problem
{
    public int[] Numbers { get; set; } = Array.Empty<int>();
    public char Operator { get; set; }
}

public class MathWorksheet
{
    public List<Problem> Problems { get; private set; } = new();

    public void ParseWorksheet(string[] lines)
    {
        if (lines.Length == 0) return;

        // Find the maximum line length to handle all columns
        int maxLength = lines.Max(l => l.Length);

        // Process each column
        for (int col = 0; col < maxLength; col++)
        {
            // Check if this column is all spaces (separator column)
            bool isAllSpaces = true;
            for (int row = 0; row < lines.Length; row++)
            {
                if (col < lines[row].Length && lines[row][col] != ' ')
                {
                    isAllSpaces = false;
                    break;
                }
            }

            if (isAllSpaces) continue;

            // This column has content, extract the problem
            List<string> columnChars = new();
            for (int row = 0; row < lines.Length; row++)
            {
                if (col < lines[row].Length)
                {
                    columnChars.Add(lines[row][col].ToString());
                }
            }

            // Find where this column's problem starts and ends
            int startCol = col;
            
            // Expand to include all connected non-space characters horizontally
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

            // Extract the full column range [startCol, col)
            List<string> numbers = new();
            char? operatorChar = null;

            for (int row = 0; row < lines.Length; row++)
            {
                string segment = "";
                for (int c = startCol; c < col && c < lines[row].Length; c++)
                {
                    segment += lines[row][c];
                }
                
                segment = segment.Trim();
                if (string.IsNullOrEmpty(segment)) continue;

                // Last row should be the operator
                if (row == lines.Length - 1)
                {
                    operatorChar = segment[0];
                }
                else
                {
                    numbers.Add(segment);
                }
            }

            if (operatorChar.HasValue && numbers.Count > 0)
            {
                Problems.Add(new Problem
                {
                    Numbers = numbers.Select(int.Parse).ToArray(),
                    Operator = operatorChar.Value
                });
            }

            col--; // Adjust because the loop will increment
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
