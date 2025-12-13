using Xunit;

namespace Solution1;

public class RegionSolverTests
{
    [Fact]
    public void CanPlaceShape_EmptyGrid_ReturnsTrue()
    {
        // Arrange
        var solver = new RegionSolver(4, 4);
        var shape = new List<(int row, int col)>
        {
            (0, 0),
            (0, 1),
            (1, 0)
        };
        
        // Act
        var canPlace = solver.CanPlaceShape(shape, 0, 0);
        
        // Assert
        Assert.True(canPlace);
    }

    [Fact]
    public void CanPlaceShape_OutOfBounds_ReturnsFalse()
    {
        // Arrange
        var solver = new RegionSolver(4, 4);
        var shape = new List<(int row, int col)>
        {
            (0, 0),
            (0, 1),
            (0, 2),
            (0, 3),
            (0, 4) // This extends beyond width 4
        };
        
        // Act
        var canPlace = solver.CanPlaceShape(shape, 0, 0);
        
        // Assert
        Assert.False(canPlace);
    }

    [Fact]
    public void CanPlaceShape_Overlapping_ReturnsFalse()
    {
        // Arrange
        var solver = new RegionSolver(4, 4);
        var shape1 = new List<(int row, int col)> { (0, 0), (0, 1) };
        var shape2 = new List<(int row, int col)> { (0, 1), (0, 2) };
        
        solver.PlaceShape(shape1, 0, 0);
        
        // Act
        var canPlace = solver.CanPlaceShape(shape2, 0, 0);
        
        // Assert
        Assert.False(canPlace); // Overlaps at (0, 1)
    }

    [Fact]
    public void PlaceShape_ValidPosition_UpdatesGrid()
    {
        // Arrange
        var solver = new RegionSolver(4, 4);
        var shape = new List<(int row, int col)> { (0, 0), (0, 1), (1, 0) };
        
        // Act
        solver.PlaceShape(shape, 0, 0);
        
        // Assert
        Assert.False(solver.CanPlaceShape(shape, 0, 0)); // Should overlap with itself
    }

    [Fact]
    public void RemoveShape_PlacedShape_ClearsGrid()
    {
        // Arrange
        var solver = new RegionSolver(4, 4);
        var shape = new List<(int row, int col)> { (0, 0), (0, 1), (1, 0) };
        
        solver.PlaceShape(shape, 0, 0);
        
        // Act
        solver.RemoveShape(shape, 0, 0);
        
        // Assert
        Assert.True(solver.CanPlaceShape(shape, 0, 0)); // Should be placeable again
    }

    [Fact]
    public void Solve_SimpleRegion_ReturnsTrue()
    {
        // Arrange
        // 4x4 grid with 4 1x1 shapes
        var shapes = new Dictionary<int, List<(int row, int col)>>
        {
            [0] = new List<(int row, int col)> { (0, 0) }
        };
        var region = new Region(4, 4, new List<int> { 0, 0, 0, 0 });
        
        // Act
        var canSolve = RegionSolver.Solve(shapes, region);
        
        // Assert
        Assert.True(canSolve);
    }

    [Fact]
    public void Solve_ImpossibleRegion_ReturnsFalse()
    {
        // Arrange
        // 2x2 grid (4 cells) but need to fit 5 1x1 shapes
        var shapes = new Dictionary<int, List<(int row, int col)>>
        {
            [0] = new List<(int row, int col)> { (0, 0) }
        };
        var region = new Region(2, 2, new List<int> { 0, 0, 0, 0, 0 });
        
        // Act
        var canSolve = RegionSolver.Solve(shapes, region);
        
        // Assert
        Assert.False(canSolve);
    }

    [Fact]
    public void Solve_TestInputFirstRegion_ChecksIfPossible()
    {
        // Arrange - From test input: 4x4 with shapes 0,0,0,0,2,0
        // Note: 6 shapes × 7 cells = 42 cells, but region is only 4×4 = 16 cells
        // This should be impossible
        var shapes = new Dictionary<int, List<(int row, int col)>>
        {
            [0] = ShapeParser.ParseShape(new[] { "###", "##.", "##." }),
            [2] = ShapeParser.ParseShape(new[] { ".##", "###", "##." })
        };
        var region = new Region(4, 4, new List<int> { 0, 0, 0, 0, 2, 0 });
        
        // Act
        var canSolve = RegionSolver.Solve(shapes, region);
        
        // Assert - This is mathematically impossible
        Assert.False(canSolve);
    }
}
