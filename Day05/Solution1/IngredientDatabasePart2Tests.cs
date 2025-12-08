using Xunit;

namespace Solution1;

public class IngredientDatabasePart2Tests
{
    // TEST LIST - PART 2
    // 1. Count total fresh IDs covered by ranges (with overlaps)
    // 2. Merge overlapping ranges correctly

    [Fact]
    public void CountTotalFreshIds_Example_Returns14()
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
        long result = IngredientDatabase.CountTotalFreshIds(input);

        // Assert
        // Ranges: 3-5 (3 IDs), 10-14 (5 IDs), 16-20 (5 IDs), 12-18 (7 IDs)
        // After merging overlaps: 3-5 (3 IDs), 10-20 (11 IDs)
        // Total: 3 + 11 = 14
        Assert.Equal(14, result);
    }

    [Fact]
    public void MergeRanges_OverlappingRanges_MergesCorrectly()
    {
        // Arrange
        var ranges = new[] 
        { 
            (10L, 14L), 
            (12L, 18L), 
            (16L, 20L) 
        };

        // Act
        var merged = IngredientDatabase.MergeRanges(ranges);

        // Assert
        Assert.Single(merged); // Should merge into one range
        Assert.Equal(10L, merged[0].start);
        Assert.Equal(20L, merged[0].end);
    }

    [Fact]
    public void MergeRanges_NonOverlappingRanges_KeepsSeparate()
    {
        // Arrange
        var ranges = new[] 
        { 
            (3L, 5L), 
            (10L, 14L) 
        };

        // Act
        var merged = IngredientDatabase.MergeRanges(ranges);

        // Assert
        Assert.Equal(2, merged.Length); // Should stay as 2 ranges
    }
}
