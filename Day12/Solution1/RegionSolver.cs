namespace Solution1;

public class RegionSolver
{
    private readonly int width;
    private readonly int height;
    private readonly bool[,] grid;

    public RegionSolver(int width, int height)
    {
        this.width = width;
        this.height = height;
        this.grid = new bool[height, width];
    }

    public bool CanPlaceShape(List<(int row, int col)> shape, int startRow, int startCol)
    {
        foreach (var (row, col) in shape)
        {
            int actualRow = startRow + row;
            int actualCol = startCol + col;
            
            // Check bounds
            if (actualRow < 0 || actualRow >= height || actualCol < 0 || actualCol >= width)
            {
                return false;
            }
            
            // Check if cell is already occupied
            if (grid[actualRow, actualCol])
            {
                return false;
            }
        }
        
        return true;
    }

    public void PlaceShape(List<(int row, int col)> shape, int startRow, int startCol)
    {
        foreach (var (row, col) in shape)
        {
            grid[startRow + row, startCol + col] = true;
        }
    }

    public void RemoveShape(List<(int row, int col)> shape, int startRow, int startCol)
    {
        foreach (var (row, col) in shape)
        {
            grid[startRow + row, startCol + col] = false;
        }
    }

    public static bool Solve(Dictionary<int, List<(int row, int col)>> shapes, Region region)
    {
        // Quick check: if total cells needed exceeds region size, impossible
        int totalCellsNeeded = region.ShapeIds.Sum(id => shapes[id].Count);
        int regionCells = region.Width * region.Height;
        if (totalCellsNeeded > regionCells)
        {
            return false; // Not enough space for all shapes
        }
        
        var solver = new RegionSolver(region.Width, region.Height);
        
        // Precompute all orientations for each shape type
        var shapeOrientations = new Dictionary<int, List<List<(int row, int col)>>>();
        foreach (var (shapeId, shape) in shapes)
        {
            shapeOrientations[shapeId] = ShapeParser.GenerateOrientations(shape);
        }
        
        return solver.SolveRecursive(region.ShapeIds, 0, shapes, shapeOrientations);
    }

    public string GetGridVisualization()
    {
        var sb = new System.Text.StringBuilder();
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                sb.Append(grid[row, col] ? '#' : '.');
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    private bool SolveRecursive(
        List<int> shapeIds, 
        int shapeIndex, 
        Dictionary<int, List<(int row, int col)>> shapes,
        Dictionary<int, List<List<(int row, int col)>>> shapeOrientations)
    {
        // Base case: all shapes placed successfully
        if (shapeIndex >= shapeIds.Count)
        {
            return true;
        }
        
        int shapeId = shapeIds[shapeIndex];
        var orientations = shapeOrientations[shapeId];
        
        // Try placing this shape in each possible position and orientation
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                foreach (var orientation in orientations)
                {
                    if (CanPlaceShape(orientation, row, col))
                    {
                        PlaceShape(orientation, row, col);
                        
                        if (SolveRecursive(shapeIds, shapeIndex + 1, shapes, shapeOrientations))
                        {
                            return true;
                        }
                        
                        RemoveShape(orientation, row, col);
                    }
                }
            }
        }
        
        return false;
    }
}
