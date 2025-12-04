using Xunit;

namespace Solution1;

public class ForkliftAccessTests
{
    // TEST LIST - PART 1
    // 1. CountAccessibleRolls with example grid returns 13
    // 2. IsAccessible checks if a roll has fewer than 4 adjacent rolls
    // 3. Edge cases: corners, edges, and center positions

    [Fact]
    public void CountAccessibleRolls_ExampleGrid_Returns13()
    {
        // Arrange
        var grid = new[]
        {
            "..@@.@@@@.",
            "@@@.@.@.@@",
            "@@@@@.@.@@",
            "@.@@@@..@.",
            "@@.@@@@.@@",
            ".@@@@@@@.@",
            ".@.@.@.@@@",
            "@.@@@.@@@@",
            ".@@@@@@@@.",
            "@.@.@@@.@."
        };

        // Act
        int result = ForkliftAccess.CountAccessibleRolls(grid);

        // Assert
        Assert.Equal(13, result);
    }
}
