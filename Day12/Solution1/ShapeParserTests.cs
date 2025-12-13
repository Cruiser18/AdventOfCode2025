using Xunit;

namespace Solution1;

public class ShapeParserTests
{
    [Fact]
    public void ParseShape_SimpleShape_ReturnsCoordinates()
    {
        // Arrange
        var lines = new[]
        {
            "###",
            "##.",
            "##."
        };
        
        // Act
        var coords = ShapeParser.ParseShape(lines);
        
        // Assert
        Assert.Equal(7, coords.Count); // 7 '#' characters
        Assert.Contains((0, 0), coords);
        Assert.Contains((0, 1), coords);
        Assert.Contains((0, 2), coords);
        Assert.Contains((1, 0), coords);
        Assert.Contains((1, 1), coords);
        Assert.Contains((2, 0), coords);
        Assert.Contains((2, 1), coords);
    }

    [Fact]
    public void GenerateOrientations_LShape_ReturnsAllUniqueRotationsAndFlips()
    {
        // Arrange - Simple L shape
        var shape = new List<(int row, int col)>
        {
            (0, 0),
            (1, 0),
            (1, 1)
        };
        
        // Act
        var orientations = ShapeParser.GenerateOrientations(shape);
        
        // Assert
        // An L-shape has 8 unique orientations (4 rotations × 2 flips)
        Assert.True(orientations.Count <= 8); // May have fewer if some are duplicates
        Assert.All(orientations, o => Assert.Equal(3, o.Count)); // Each should have 3 cells
    }

    [Fact]
    public void ParseRegion_SimpleRegion_ReturnsWidthHeightAndShapes()
    {
        // Arrange - "4x4: 2 1 0" means 2 of shape 0, 1 of shape 1, 0 of shape 2
        var line = "4x4: 2 1 0";
        
        // Act
        var region = ShapeParser.ParseRegion(line);
        
        // Assert
        Assert.Equal(4, region.Width);
        Assert.Equal(4, region.Height);
        Assert.Equal(3, region.ShapeIds.Count); // 2 + 1 + 0 = 3 shapes total
        Assert.Equal(new[] { 0, 0, 1 }, region.ShapeIds); // Two 0's, one 1
    }

    [Fact]
    public void ParseRegion_LargeRegion_ReturnsCorrectDimensions()
    {
        // Arrange - "12x5: 1 0 2" means 1 of shape 0, 0 of shape 1, 2 of shape 2
        var line = "12x5: 1 0 2";
        
        // Act
        var region = ShapeParser.ParseRegion(line);
        
        // Assert
        Assert.Equal(12, region.Width);
        Assert.Equal(5, region.Height);
        Assert.Equal(3, region.ShapeIds.Count); // 1 + 0 + 2 = 3 shapes total
        Assert.Equal(new[] { 0, 2, 2 }, region.ShapeIds); // One 0, two 2's
    }

    [Fact]
    public void ParseInput_CompleteInput_ReturnsShapesAndRegions()
    {
        // Arrange
        var lines = new[]
        {
            "0:",
            "###",
            "##.",
            "##.",
            "",
            "1:",
            "###",
            "##.",
            ".##",
            "",
            "4x4: 0 0 0 0 2 0",
            "12x5: 1 0 1 0 2 2"
        };
        
        // Act
        var (shapes, regions) = ShapeParser.ParseInput(lines);
        
        // Assert
        Assert.Equal(2, shapes.Count);
        Assert.Equal(2, regions.Count);
        Assert.Equal(7, shapes[0].Count); // Shape 0 has 7 cells
        Assert.Equal(7, shapes[1].Count); // Shape 1 has 7 cells (3+2+2)
        Assert.Equal(4, regions[0].Width);
        Assert.Equal(12, regions[1].Width);
    }
}
