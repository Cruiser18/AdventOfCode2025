namespace Solution1;

public class TachyonManifold
{
    private readonly string[] _grid;
    public (int row, int col) StartPosition { get; private set; }

    public TachyonManifold(string[] grid)
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

    public int CountBeamSplits()
    {
        int splitCount = 0;
        Queue<(int row, int col)> beams = new();
        HashSet<(int row, int col)> processedBeams = new();
        HashSet<(int row, int col)> hitSplitters = new();

        // Start with initial beam at S position
        beams.Enqueue(StartPosition);

        while (beams.Count > 0)
        {
            var (startRow, col) = beams.Dequeue();

            // Skip if we've already processed a beam from this exact position
            if (!processedBeams.Add((startRow, col)))
            {
                continue;
            }

            // Skip if beam is outside the grid horizontally
            if (col < 0 || (startRow < _grid.Length && col >= _grid[startRow].Length))
            {
                continue;
            }

            // Move beam downward until it hits a splitter or exits
            for (int r = startRow + 1; r < _grid.Length; r++)
            {
                // Check if column is within bounds
                if (col < 0 || col >= _grid[r].Length)
                {
                    break;
                }

                if (_grid[r][col] == '^')
                {
                    // Hit a splitter! Only count if we haven't hit this splitter before
                    if (hitSplitters.Add((r, col)))
                    {
                        splitCount++;
                    }

                    // Create two new beams: one to the left, one to the right
                    // These beams start at the splitter row and continue downward
                    beams.Enqueue((r, col - 1));
                    beams.Enqueue((r, col + 1));
                    break;
                }
                // Otherwise it's empty space '.' or 'S', continue moving down
            }
        }

        return splitCount;
    }
}
