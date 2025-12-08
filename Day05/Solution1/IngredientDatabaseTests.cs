using Xunit;

namespace Solution1;

public class IngredientDatabaseTests
{
    // TEST LIST - PART 1
    // 1. Parse fresh ranges from input
    // 2. Parse available IDs from input
    // 3. Check if an ID is fresh (falls in any range)
    // 4. Count fresh ingredients from example - should return 3

    [Fact]
    public void CountFreshIngredients_Example_Returns3()
    {
        // Arrange
        var input = new[]
        {
            "3-5",
            "10-14",
            "16-20",
            "12-18",
            "",
            "1",
            "5",
            "8",
            "11",
            "17",
            "32"
        };

        // Act
        int result = IngredientDatabase.CountFreshIngredients(input);

        // Assert
        Assert.Equal(3, result); // IDs 5, 11, and 17 are fresh
    }

    [Fact]
    public void IsFresh_IdInRange_ReturnsTrue()
    {
        // Arrange
        var ranges = new[] { (3L, 5L), (10L, 14L) };

        // Act & Assert
        Assert.True(IngredientDatabase.IsFresh(5L, ranges)); // In range 3-5
        Assert.True(IngredientDatabase.IsFresh(11L, ranges)); // In range 10-14
    }

    [Fact]
    public void IsFresh_IdNotInRange_ReturnsFalse()
    {
        // Arrange
        var ranges = new[] { (3L, 5L), (10L, 14L) };

        // Act & Assert
        Assert.False(IngredientDatabase.IsFresh(1L, ranges)); // Not in any range
        Assert.False(IngredientDatabase.IsFresh(8L, ranges)); // Between ranges
    }
}
