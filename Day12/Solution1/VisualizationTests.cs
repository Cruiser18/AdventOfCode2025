using Xunit;
using Xunit.Abstractions;

namespace Solution1;

public class VisualizationTests
{
    private readonly ITestOutputHelper output;

    public VisualizationTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void VisualizeFirstRegion_FromActualInput()
    {
        // Arrange - Read actual shapes from input
        var inputLines = File.ReadAllLines(@"c:\mjolner-code\AdventOfCode2025\Day12\input.txt");
        var (shapes, _) = ShapeParser.ParseInput(inputLines);

        // First region from actual input: "38x47: 30 28 36 34 25 27"
        var region = ShapeParser.ParseRegion("38x47: 30 28 36 34 25 27");
        
        output.WriteLine($"Region: {region.Width}x{region.Height}");
        output.WriteLine($"Total cells in region: {region.Width * region.Height}");
        output.WriteLine($"Shapes needed:");
        output.WriteLine($"  30 of shape 0 ({shapes[0].Count} cells each) = {30 * shapes[0].Count} cells");
        output.WriteLine($"  28 of shape 1 ({shapes[1].Count} cells each) = {28 * shapes[1].Count} cells");
        output.WriteLine($"  36 of shape 2 ({shapes[2].Count} cells each) = {36 * shapes[2].Count} cells");
        output.WriteLine($"  34 of shape 3 ({shapes[3].Count} cells each) = {34 * shapes[3].Count} cells");
        output.WriteLine($"  25 of shape 4 ({shapes[4].Count} cells each) = {25 * shapes[4].Count} cells");
        output.WriteLine($"  27 of shape 5 ({shapes[5].Count} cells each) = {27 * shapes[5].Count} cells");
        
        int totalCellsNeeded = region.ShapeIds.Sum(id => shapes[id].Count);
        output.WriteLine($"Total cells needed: {totalCellsNeeded}");
        output.WriteLine($"Difference: {region.Width * region.Height - totalCellsNeeded}");
        
        // Try to solve
        var solver = new VisualizingSolver(region.Width, region.Height);
        var canSolve = solver.Solve(shapes, region);
        
        output.WriteLine($"\nCan solve: {canSolve}");
        
        if (canSolve)
        {
            output.WriteLine("\nSolution visualization:");
            output.WriteLine(solver.GetVisualization());
        }
        else
        {
            output.WriteLine("\nNo solution found - region cannot fit all shapes");
            output.WriteLine("Partial grid (if any shapes were placed):");
            output.WriteLine(solver.GetVisualization());
        }
    }
}

public class VisualizingSolver
{
    private readonly int width;
    private readonly int height;
    private readonly int[,] grid; // 0 = empty, otherwise shape ID + 1

    public VisualizingSolver(int width, int height)
    {
        this.width = width;
        this.height = height;
        this.grid = new int[height, width];
    }

    public bool Solve(Dictionary<int, List<(int row, int col)>> shapes, Region region)
    {
        // Quick check: if total cells needed exceeds region size, impossible
        int totalCellsNeeded = region.ShapeIds.Sum(id => shapes[id].Count);
        int regionCells = region.Width * region.Height;
        if (totalCellsNeeded > regionCells)
        {
            return false; // Too many shapes to fit
        }
        
        // Precompute all orientations for each shape type
        var shapeOrientations = new Dictionary<int, List<List<(int row, int col)>>>();
        foreach (var (shapeId, shape) in shapes)
        {
            shapeOrientations[shapeId] = ShapeParser.GenerateOrientations(shape);
        }
        
        return SolveRecursive(region.ShapeIds, 0, shapes, shapeOrientations);
    }

    private bool CanPlaceShape(List<(int row, int col)> shape, int startRow, int startCol)
    {
        foreach (var (row, col) in shape)
        {
            int actualRow = startRow + row;
            int actualCol = startCol + col;
            
            if (actualRow < 0 || actualRow >= height || actualCol < 0 || actualCol >= width)
            {
                return false;
            }
            
            if (grid[actualRow, actualCol] != 0)
            {
                return false;
            }
        }
        
        return true;
    }

    private void PlaceShape(List<(int row, int col)> shape, int startRow, int startCol, int shapeId)
    {
        foreach (var (row, col) in shape)
        {
            grid[startRow + row, startCol + col] = shapeId + 1; // +1 so 0 remains empty
        }
    }

    private void RemoveShape(List<(int row, int col)> shape, int startRow, int startCol)
    {
        foreach (var (row, col) in shape)
        {
            grid[startRow + row, startCol + col] = 0;
        }
    }

    private bool SolveRecursive(
        List<int> shapeIds, 
        int shapeIndex, 
        Dictionary<int, List<(int row, int col)>> shapes,
        Dictionary<int, List<List<(int row, int col)>>> shapeOrientations)
    {
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
                        PlaceShape(orientation, row, col, shapeId);
                        
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

    public string GetVisualization()
    {
        var sb = new System.Text.StringBuilder();
        var colors = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                if (grid[row, col] == 0)
                {
                    sb.Append('.');
                }
                else
                {
                    int shapeId = grid[row, col] - 1;
                    sb.Append(colors[shapeId % colors.Length]);
                }
            }
            sb.AppendLine();
        }
        
        sb.AppendLine();
        sb.AppendLine("Legend:");
        for (int i = 0; i < 6; i++)
        {
            sb.AppendLine($"  {colors[i]} = Shape {i}");
        }
        
        return sb.ToString();
    }
}
