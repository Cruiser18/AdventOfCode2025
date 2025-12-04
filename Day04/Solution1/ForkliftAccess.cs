namespace Solution1;

public class ForkliftAccess
{
    public static int CountAccessibleRolls(string[] grid)
    {
        int count = 0;
        
        for (int row = 0; row < grid.Length; row++)
        {
            for (int col = 0; col < grid[row].Length; col++)
            {
                // Check if current position is a roll (@)
                if (grid[row][col] == '@')
                {
                    // Count adjacent rolls
                    int adjacentRolls = CountAdjacentRolls(grid, row, col);
                    
                    // Accessible if fewer than 4 adjacent rolls
                    if (adjacentRolls < 4)
                    {
                        count++;
                    }
                }
            }
        }
        
        return count;
    }
    
    private static int CountAdjacentRolls(string[] grid, int row, int col)
    {
        int count = 0;
        
        // Check all 8 adjacent positions (including diagonals)
        for (int dr = -1; dr <= 1; dr++)
        {
            for (int dc = -1; dc <= 1; dc++)
            {
                // Skip the center position (the roll itself)
                if (dr == 0 && dc == 0)
                    continue;
                
                int newRow = row + dr;
                int newCol = col + dc;
                
                // Check bounds
                if (newRow >= 0 && newRow < grid.Length &&
                    newCol >= 0 && newCol < grid[newRow].Length)
                {
                    if (grid[newRow][newCol] == '@')
                    {
                        count++;
                    }
                }
            }
        }
        
        return count;
    }
    
    public static int CountTotalRemovableRolls(string[] grid)
    {
        // Create a mutable copy of the grid
        char[][] mutableGrid = grid.Select(row => row.ToCharArray()).ToArray();
        int totalRemoved = 0;
        bool keepRemoving = true;
        
        while (keepRemoving)
        {
            // Find all accessible rolls in current state
            List<(int row, int col)> accessibleRolls = new List<(int, int)>();
            
            for (int row = 0; row < mutableGrid.Length; row++)
            {
                for (int col = 0; col < mutableGrid[row].Length; col++)
                {
                    if (mutableGrid[row][col] == '@')
                    {
                        int adjacentRolls = CountAdjacentRollsFromMutable(mutableGrid, row, col);
                        if (adjacentRolls < 4)
                        {
                            accessibleRolls.Add((row, col));
                        }
                    }
                }
            }
            
            // If no accessible rolls found, stop
            if (accessibleRolls.Count == 0)
            {
                keepRemoving = false;
            }
            else
            {
                // Remove all accessible rolls (replace with '.')
                foreach (var (row, col) in accessibleRolls)
                {
                    mutableGrid[row][col] = '.';
                }
                totalRemoved += accessibleRolls.Count;
            }
        }
        
        return totalRemoved;
    }
    
    private static int CountAdjacentRollsFromMutable(char[][] grid, int row, int col)
    {
        int count = 0;
        
        // Check all 8 adjacent positions (including diagonals)
        for (int dr = -1; dr <= 1; dr++)
        {
            for (int dc = -1; dc <= 1; dc++)
            {
                // Skip the center position
                if (dr == 0 && dc == 0)
                    continue;
                
                int newRow = row + dr;
                int newCol = col + dc;
                
                // Check bounds
                if (newRow >= 0 && newRow < grid.Length &&
                    newCol >= 0 && newCol < grid[newRow].Length)
                {
                    if (grid[newRow][newCol] == '@')
                    {
                        count++;
                    }
                }
            }
        }
        
        return count;
    }
}
