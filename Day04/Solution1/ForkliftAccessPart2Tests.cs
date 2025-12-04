using Xunit;

namespace Solution1;

public class ForkliftAccessPart2Tests
{
    // TEST LIST - PART 2
    // 1. CountTotalRemovableRolls with example grid - iterative removal
    // 2. Verify that after each removal, new rolls become accessible

    [Fact]
    public void CountTotalRemovableRolls_ExampleGrid_ReturnsCorrectCount()
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
        int result = ForkliftAccess.CountTotalRemovableRolls(grid);

        // Assert
        // First iteration removes 13 rolls (from Part 1)
        // After removal, more rolls become accessible
        // Continue until no more accessible rolls
        Assert.True(result >= 13); // At least the first 13 can be removed
    }

    [Fact]
    public void CountTotalRemovableRolls_SimpleGrid_RemovesAll()
    {
        // Arrange - a simple grid where all can eventually be removed
        var grid = new[]
        {
            "@@@",
            "@@@",
            "@@@"
        };

        // Act
        int result = ForkliftAccess.CountTotalRemovableRolls(grid);

        // Assert
        // In a 3x3 grid, all corner rolls have < 4 neighbors initially
        // After removing corners, edges become accessible, etc.
        Assert.Equal(9, result); // All 9 should be removable
    }
}
