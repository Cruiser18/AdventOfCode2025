using Xunit;

namespace Solution1;

public class BatteryBankTests
{
    // TEST LIST - BASE FUNCTIONALITY ONLY
    [Fact]
    public void FindMaxJoltage_SimpleBank_ReturnsMaxTwoDigitCombo()
    {
        // Arrange & Act
        int result = BatteryBank.FindMaxJoltage("543");

        // Assert
        Assert.Equal(54, result); // 5 and 4 make 54
    }

    [Fact]
    public void FindMaxJoltage_Example1_Returns98()
    {
        // Arrange & Act
        int result = BatteryBank.FindMaxJoltage("987654321111111");

        // Assert
        Assert.Equal(98, result); // 9 and 8 make 98
    }

    [Fact]
    public void FindMaxJoltage_Example2_Returns89()
    {
        // Arrange & Act
        int result = BatteryBank.FindMaxJoltage("811111111111119");

        // Assert
        Assert.Equal(89, result); // 8 and 9 make 89
    }

    [Fact]
    public void FindMaxJoltage_Example3_Returns78()
    {
        // Arrange & Act
        int result = BatteryBank.FindMaxJoltage("234234234234278");

        // Assert
        Assert.Equal(78, result); // 7 and 8 make 78
    }

    [Fact]
    public void FindMaxJoltage_Example4_Returns92()
    {
        // Arrange & Act
        int result = BatteryBank.FindMaxJoltage("818181911112111");

        // Assert
        Assert.Equal(92, result); // 9 and 2 make 92
    }

    [Fact]
    public void CalculateTotalJoltage_MultipleBanks_ReturnsSumOfMaximums()
    {
        // Arrange
        var banks = new[] { "543", "987" };

        // Act
        int result = BatteryBank.CalculateTotalJoltage(banks);

        // Assert
        Assert.Equal(152, result); // 54 + 98 = 152
    }

    [Fact]
    public void CalculateTotalJoltage_ExampleInput_Returns357()
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
        int result = BatteryBank.CalculateTotalJoltage(banks);

        // Assert
        Assert.Equal(357, result); // 98 + 89 + 78 + 92 = 357
    }
}
