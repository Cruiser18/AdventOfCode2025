using Xunit;

namespace Solution2;

public class InvalidIdFinderAdvancedTests
{
    // TEST LIST - BASE FUNCTIONALITY ONLY
    [Fact]
    public void IsInvalidId_SimpleRepeatedDigit_ReturnsTrue()
    {
        // Arrange & Act
        bool result = InvalidIdFinderAdvanced.IsInvalidId(11);

        // Assert
        Assert.True(result);
    }

    // 2. IsInvalidId recognizes two-digit repeated twice (1212)
    // 3. IsInvalidId recognizes pattern repeated three times (123123123)
    // 4. IsInvalidId recognizes pattern repeated five times (1212121212)
    // 5. IsInvalidId recognizes pattern repeated seven times (1111111)
    
    [Fact]
    public void IsInvalidId_NonRepeatedSequence_ReturnsFalse()
    {
        // Arrange & Act
        bool result = InvalidIdFinderAdvanced.IsInvalidId(123);

        // Assert
        Assert.False(result);
    }

    // 6. IsInvalidId returns false for non-repeated sequences (123)
    
    [Fact]
    public void IsInvalidId_PatternRepeatedThreeTimes_ReturnsTrue()
    {
        // Arrange & Act
        bool result = InvalidIdFinderAdvanced.IsInvalidId(123123123);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void IsInvalidId_PatternRepeatedFiveTimes_ReturnsTrue()
    {
        // Arrange & Act
        bool result = InvalidIdFinderAdvanced.IsInvalidId(1212121212);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void IsInvalidId_PatternRepeatedSevenTimes_ReturnsTrue()
    {
        // Arrange & Act
        bool result = InvalidIdFinderAdvanced.IsInvalidId(1111111);

        // Assert
        Assert.True(result);
    }

    // 6. IsInvalidId returns false for non-repeated sequences (123)
    
    [Fact]
    public void SumInvalidIds_TestInput_ReturnsExpectedSum()
    {
        // Arrange
        string input = File.ReadAllText(@"c:\mjolner-code\AdventOfCode2025\Day02\testinput.txt")
            .Replace("\n", "").Replace("\r", "");
        
        // Act
        long sum = InvalidIdFinderAdvanced.SumInvalidIds(input);

        // Assert - Part 2 expected result
        Assert.Equal(4174379265, sum);
    }
    
    // 7. ParseRange extracts start and end from range string
    // 8. FindInvalidIdsInRange finds invalid IDs in range (95-115 should find 99 and 111)
    // 9. SumInvalidIds sums all invalid IDs from multiple ranges
}
