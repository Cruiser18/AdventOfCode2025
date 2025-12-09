using Xunit;

namespace Solution1;

public class RedTileTests
{
    // TEST LIST
    // 1. Parse red tile coordinates from input
    // 2. Calculate area of rectangle between two points
    // 3. Find largest rectangle area from all pairs
    // 4. Example with 8 red tiles should return area 50
    // 5. Part 2: Find largest rectangle using only red/green tiles (inside polygon) - should return 24

    [Fact]
    public void CalculateArea_TwoPoints_ReturnsRectangleArea()
    {
        // Arrange
        var point1 = (x: 2, y: 5);
        var point2 = (x: 11, y: 1);

        // Act
        long area = RedTile.CalculateArea(point1, point2);

        // Assert
        Assert.Equal(50L, area); // width=10 (11-2+1), height=5 (5-1+1), area=50
    }

    [Fact]
    public void FindLargestRectangle_ExampleInput_Returns50()
    {
        // Arrange
        var input = new[]
        {
            "7,1",
            "11,1",
            "11,7",
            "9,7",
            "9,5",
            "2,5",
            "2,3",
            "7,3"
        };

        // Act
        long largestArea = RedTile.FindLargestRectangle(input);

        // Assert
        Assert.Equal(50L, largestArea);
    }

    [Fact]
    public void CalculateArea_SmallRectangle_Returns6()
    {
        // Arrange - rectangle between 7,3 and 2,3
        var point1 = (x: 7, y: 3);
        var point2 = (x: 2, y: 3);

        // Act
        long area = RedTile.CalculateArea(point1, point2);

        // Assert
        Assert.Equal(6L, area); // width=6, height=1, area=6
    }

    [Fact]
    public void FindLargestRectangleInPolygon_ExampleInput_Returns24()
    {
        // Arrange
        var input = new[]
        {
            "7,1",
            "11,1",
            "11,7",
            "9,7",
            "9,5",
            "2,5",
            "2,3",
            "7,3"
        };

        // Act
        long largestArea = RedTile.FindLargestRectangleInPolygon(input);

        // Assert
        Assert.Equal(24L, largestArea); // Largest rectangle within the polygon
    }
}
