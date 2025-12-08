namespace Solution2;

public class QuantumTachyonManifold
{
    private readonly string[] _grid;
    private readonly Dictionary<(int row, int col), long> _memo = new();
    public (int row, int col) StartPosition { get; private set; }

    public QuantumTachyonManifold(string[] grid)
    {
        _grid = grid;
        FindStartPosition();
    }

    private void FindStartPosition()
    {
        for (int row = 0; row < _grid.Length; row++)
        {
            for (int col = 0; col < _grid[row].Length; col++)
            {
                if (_grid[row][col] == 'S')
                {
                    StartPosition = (row, col);
                    return;
                }
            }
        }
    }

    public long CountTimelines()
    {
        // Use DFS to explore all possible paths (timelines)
        // Each path represents one timeline
        return CountPathsFromPosition(StartPosition.row, StartPosition.col);
    }

    private long CountPathsFromPosition(int startRow, int col)
    {
        // Check memoization cache
        if (_memo.TryGetValue((startRow, col), out long cachedResult))
        {
            return cachedResult;
        }

        // Check if position is out of bounds
        if (col < 0 || (startRow < _grid.Length && col >= _grid[startRow].Length))
        {
            return 0; // Path exits horizontally, doesn't count
        }

        // Move downward until we hit a splitter or exit the manifold
        for (int row = startRow + 1; row < _grid.Length; row++)
        {
            // Check if column is within bounds
            if (col < 0 || col >= _grid[row].Length)
            {
                // Particle exits the manifold - this is one complete timeline
                _memo[(startRow, col)] = 1;
                return 1;
            }

            if (_grid[row][col] == '^')
            {
                // Hit a splitter! The particle splits into two timelines
                // Count paths going left and paths going right
                long leftPaths = CountPathsFromPosition(row, col - 1);
                long rightPaths = CountPathsFromPosition(row, col + 1);
                long result = leftPaths + rightPaths;
                _memo[(startRow, col)] = result;
                return result;
            }
        }

        // If we reach here, particle exited through the bottom
        _memo[(startRow, col)] = 1;
        return 1;
    }
}
