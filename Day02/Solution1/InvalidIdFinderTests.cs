using Xunit;

namespace Solution1;

public class InvalidIdFinderTests
{
    // TEST LIST - BASE FUNCTIONALITY ONLY
    [Fact]
    public void IsInvalidId_SimpleRepeatedDigit_ReturnsTrue()
    {
        // Arrange & Act
        bool result = InvalidIdFinder.IsInvalidId(11);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsInvalidId_TwoDigitRepeatedSequence_ReturnsTrue()
    {
        // Arrange & Act
        bool result = InvalidIdFinder.IsInvalidId(6464);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsInvalidId_ThreeDigitRepeatedSequence_ReturnsTrue()
    {
        // Arrange & Act
        bool result = InvalidIdFinder.IsInvalidId(123123);

        // Assert
        Assert.True(result);
    }

    // 3. IsInvalidId recognizes three-digit repeated sequence (123123)
    
    [Fact]
    public void IsInvalidId_NonRepeatedSequence_ReturnsFalse()
    {
        // Arrange & Act
        bool result = InvalidIdFinder.IsInvalidId(123);

        // Assert
        Assert.False(result);
    }
    // 5. IsInvalidId returns false for leading zero numbers (0101)
    
    [Fact]
    public void ParseRange_SimpleRange_ReturnsStartAndEnd()
    {
        // Arrange & Act
        var (start, end) = InvalidIdFinder.ParseRange("11-22");

        // Assert
        Assert.Equal(11, start);
        Assert.Equal(22, end);
    }

    // 6. ParseRange extracts start and end from range string
    
    [Fact]
    public void FindInvalidIdsInRange_SimpleRange_FindsAllInvalid()
    {
        // Arrange & Act
        var invalidIds = InvalidIdFinder.FindInvalidIdsInRange(11, 22);

        // Assert
        Assert.Equal(2, invalidIds.Count);
        Assert.Contains(11L, invalidIds);
        Assert.Contains(22L, invalidIds);
    }

    // 7. FindInvalidIdsInRange finds invalid IDs in a simple range (11-22)
    
    [Fact]
    public void FindInvalidIdsInRange_LargerRange_FindsInvalidId()
    {
        // Arrange & Act
        var invalidIds = InvalidIdFinder.FindInvalidIdsInRange(95, 115);

        // Assert
        Assert.Single(invalidIds);
        Assert.Contains(99L, invalidIds);
    }

    // 8. FindInvalidIdsInRange finds invalid IDs in larger range (95-115)
    
    [Fact]
    public void SumInvalidIds_MultipleRanges_ReturnsSumOfAllInvalid()
    {
        // Arrange
        string input = "11-22,95-115";
        
        // Act
        long sum = InvalidIdFinder.SumInvalidIds(input);

        // Assert
        // 11-22 has: 11, 22 = 33
        // 95-115 has: 99 = 99
        // Total: 132
        Assert.Equal(132, sum);
    }

    // 9. SumInvalidIds sums all invalid IDs from multiple ranges
    
    [Fact]
    public void SumInvalidIds_TestInput_ReturnsExpectedSum()
    {
        // Arrange
        string input = File.ReadAllText(@"c:\mjolner-code\AdventOfCode2025\Day02\testinput.txt")
            .Replace("\n", "").Replace("\r", "");
        
        // Act
        long sum = InvalidIdFinder.SumInvalidIds(input);

        // Assert
        Assert.Equal(1227775554, sum);
    }
}
