using Xunit;

namespace Solution1;

public class BatteryBankPart2Tests
{
    // TEST LIST - PART 2
    [Fact]
    public void FindMaxJoltage12_Example1_Returns987654321111()
    {
        // Arrange & Act
        long result = BatteryBank.FindMaxJoltage12("987654321111111");

        // Assert
        Assert.Equal(987654321111L, result);
    }

    [Fact]
    public void FindMaxJoltage12_Example2_Returns811111111119()
    {
        // Arrange & Act
        long result = BatteryBank.FindMaxJoltage12("811111111111119");

        // Assert
        Assert.Equal(811111111119L, result);
    }

    [Fact]
    public void FindMaxJoltage12_Example3_Returns434234234278()
    {
        // Arrange & Act
        long result = BatteryBank.FindMaxJoltage12("234234234234278");

        // Assert
        Assert.Equal(434234234278L, result);
    }

    [Fact]
    public void FindMaxJoltage12_Example4_Returns888911112111()
    {
        // Arrange & Act
        long result = BatteryBank.FindMaxJoltage12("818181911112111");

        // Assert
        Assert.Equal(888911112111L, result);
    }

    [Fact]
    public void CalculateTotalJoltage12_ExampleInput_Returns3121910778619()
    {
        // Arrange
        var banks = new[] 
        { 
            "987654321111111",
            "811111111111119",
            "234234234234278",
            "818181911112111"
        };

        // Act
        long result = BatteryBank.CalculateTotalJoltage12(banks);

        // Assert
        Assert.Equal(3121910778619L, result); // 987654321111 + 811111111119 + 434234234278 + 888911112111
    }
}
