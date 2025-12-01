using Xunit;

namespace Solution2.Tests;

public class SafeDialAdvancedTests
{
    [Fact]
    public void Dial_StartsAt50()
    {
        // Arrange & Act
        var dial = new SafeDialAdvanced();
        
        // Assert
        Assert.Equal(50, dial.CurrentPosition);
    }
    
    [Fact]
    public void CountZeros_StartsAtZero()
    {
        // Arrange & Act
        var dial = new SafeDialAdvanced();
        
        // Assert
        Assert.Equal(0, dial.ZeroCount);
    }
    
    [Fact]
    public void RotateRight_NoPassThroughZero_CountIsZero()
    {
        // Arrange
        var dial = new SafeDialAdvanced();
        
        // Act
        dial.Rotate('R', 10); // From 50 to 60, doesn't pass 0
        
        // Assert
        Assert.Equal(60, dial.CurrentPosition);
        Assert.Equal(0, dial.ZeroCount);
    }
    
    [Fact]
    public void RotateRight_LandsOnZero_CountIsOne()
    {
        // Arrange
        var dial = new SafeDialAdvanced();
        
        // Act
        dial.Rotate('R', 50); // From 50 to 0, passes through 0 once
        
        // Assert
        Assert.Equal(0, dial.CurrentPosition);
        Assert.Equal(1, dial.ZeroCount);
    }
    
    [Fact]
    public void RotateRight_PassesThroughZeroOnce_CountIsOne()
    {
        // Arrange
        var dial = new SafeDialAdvanced();
        
        // Act
        dial.Rotate('R', 60); // From 50 to 10, passes through 0 once
        
        // Assert
        Assert.Equal(10, dial.CurrentPosition);
        Assert.Equal(1, dial.ZeroCount);
    }
    
    [Fact]
    public void RotateRight_PassesThroughZeroMultipleTimes_CountsAll()
    {
        // Arrange
        var dial = new SafeDialAdvanced();
        
        // Act
        dial.Rotate('R', 1000); // From 50, passes through 0 ten times, ends at 50
        
        // Assert
        Assert.Equal(50, dial.CurrentPosition);
        Assert.Equal(10, dial.ZeroCount);
    }
    
    [Fact]
    public void RotateLeft_NoPassThroughZero_CountIsZero()
    {
        // Arrange
        var dial = new SafeDialAdvanced();
        
        // Act
        dial.Rotate('L', 10); // From 50 to 40, doesn't pass 0
        
        // Assert
        Assert.Equal(40, dial.CurrentPosition);
        Assert.Equal(0, dial.ZeroCount);
    }
    
    [Fact]
    public void RotateLeft_LandsOnZero_CountIsOne()
    {
        // Arrange
        var dial = new SafeDialAdvanced();
        
        // Act
        dial.Rotate('L', 50); // From 50 to 0, passes through 0 once
        
        // Assert
        Assert.Equal(0, dial.CurrentPosition);
        Assert.Equal(1, dial.ZeroCount);
    }
    
    [Fact]
    public void RotateLeft_PassesThroughZeroOnce_CountIsOne()
    {
        // Arrange
        var dial = new SafeDialAdvanced();
        
        // Act
        dial.Rotate('L', 60); // From 50 to 90, passes through 0 once
        
        // Assert
        Assert.Equal(90, dial.CurrentPosition);
        Assert.Equal(1, dial.ZeroCount);
    }
    
    [Fact]
    public void RotateLeft_PassesThroughZeroMultipleTimes_CountsAll()
    {
        // Arrange
        var dial = new SafeDialAdvanced();
        
        // Act
        dial.Rotate('L', 1000); // From 50, passes through 0 ten times, ends at 50
        
        // Assert
        Assert.Equal(50, dial.CurrentPosition);
        Assert.Equal(10, dial.ZeroCount);
    }
    
    [Fact]
    public void ExampleStep2_L68From50_PassesThroughZeroOnce()
    {
        // Arrange
        var dial = new SafeDialAdvanced();
        
        // Act
        dial.Rotate('L', 68); // From 50 to 82, passes through 0 once
        
        // Assert
        Assert.Equal(82, dial.CurrentPosition);
        Assert.Equal(1, dial.ZeroCount);
    }
    
    [Fact]
    public void ExampleStep3_L30From82_DoesNotPassThroughZero()
    {
        // Arrange
        var dial = new SafeDialAdvanced();
        dial.Rotate('L', 68); // Move to 82, count = 1
        
        // Act
        dial.Rotate('L', 30); // From 82 to 52, doesn't pass 0
        
        // Assert
        Assert.Equal(52, dial.CurrentPosition);
        Assert.Equal(1, dial.ZeroCount); // Still 1
    }
    
    [Fact]
    public void ExampleStep4_R48From52_LandsOnZero()
    {
        // Arrange
        var dial = new SafeDialAdvanced();
        dial.Rotate('L', 68); // Move to 82, count = 1
        dial.Rotate('L', 30); // Move to 52, count = 1
        
        // Act
        dial.Rotate('R', 48); // From 52 to 0
        
        // Assert
        Assert.Equal(0, dial.CurrentPosition);
        Assert.Equal(2, dial.ZeroCount); // Now 2
    }
    
    [Fact]
    public void ExampleStep6_R60From95_PassesThroughZeroOnce()
    {
        // Arrange
        var dial = new SafeDialAdvanced();
        dial.Rotate('L', 68); // To 82, count = 1
        dial.Rotate('L', 30); // To 52, count = 1
        dial.Rotate('R', 48); // To 0, count = 2
        dial.Rotate('L', 5);  // To 95, count = 2
        
        // Act
        dial.Rotate('R', 60); // From 95 to 55, passes through 0 once
        
        // Assert
        Assert.Equal(55, dial.CurrentPosition);
        Assert.Equal(3, dial.ZeroCount); // Now 3
    }
    
    [Fact]
    public void ExampleStep11_L82From14_PassesThroughZeroOnce()
    {
        // Arrange
        var dial = new SafeDialAdvanced();
        dial.Rotate('L', 68);  // To 82, count = 1
        dial.Rotate('L', 30);  // To 52, count = 1
        dial.Rotate('R', 48);  // To 0, count = 2
        dial.Rotate('L', 5);   // To 95, count = 2
        dial.Rotate('R', 60);  // To 55, count = 3
        dial.Rotate('L', 55);  // To 0, count = 4
        dial.Rotate('L', 1);   // To 99, count = 4
        dial.Rotate('L', 99);  // To 0, count = 5
        dial.Rotate('R', 14);  // To 14, count = 5
        
        // Act
        dial.Rotate('L', 82); // From 14 to 32, passes through 0 once
        
        // Assert
        Assert.Equal(32, dial.CurrentPosition);
        Assert.Equal(6, dial.ZeroCount); // Now 6
    }
    
    [Fact]
    public void ProcessInstructions_WithExampleInput_Returns6()
    {
        // Arrange
        var instructions = new[]
        {
            "L68", "L30", "R48", "L5", "R60", 
            "L55", "L1", "L99", "R14", "L82"
        };
        var dial = new SafeDialAdvanced();
        
        // Act
        foreach (var instruction in instructions)
        {
            var (direction, distance) = SafeDialAdvanced.ParseInstruction(instruction);
            dial.Rotate(direction, distance);
        }
        
        // Assert
        Assert.Equal(6, dial.ZeroCount);
    }
    
    [Fact]
    public void ProcessInstructions_FullExampleWalkthrough()
    {
        // Arrange
        var dial = new SafeDialAdvanced();
        
        // Act & Assert - Following example step by step
        Assert.Equal(50, dial.CurrentPosition); // Starts at 50
        Assert.Equal(0, dial.ZeroCount);
        
        dial.Rotate('L', 68); // To 82, passes 0 once
        Assert.Equal(82, dial.CurrentPosition);
        Assert.Equal(1, dial.ZeroCount);
        
        dial.Rotate('L', 30); // To 52, no pass
        Assert.Equal(52, dial.CurrentPosition);
        Assert.Equal(1, dial.ZeroCount);
        
        dial.Rotate('R', 48); // To 0
        Assert.Equal(0, dial.CurrentPosition);
        Assert.Equal(2, dial.ZeroCount);
        
        dial.Rotate('L', 5); // To 95, no pass
        Assert.Equal(95, dial.CurrentPosition);
        Assert.Equal(2, dial.ZeroCount);
        
        dial.Rotate('R', 60); // To 55, passes 0 once
        Assert.Equal(55, dial.CurrentPosition);
        Assert.Equal(3, dial.ZeroCount);
        
        dial.Rotate('L', 55); // To 0
        Assert.Equal(0, dial.CurrentPosition);
        Assert.Equal(4, dial.ZeroCount);
        
        dial.Rotate('L', 1); // To 99, no pass
        Assert.Equal(99, dial.CurrentPosition);
        Assert.Equal(4, dial.ZeroCount);
        
        dial.Rotate('L', 99); // To 0
        Assert.Equal(0, dial.CurrentPosition);
        Assert.Equal(5, dial.ZeroCount);
        
        dial.Rotate('R', 14); // To 14, no pass
        Assert.Equal(14, dial.CurrentPosition);
        Assert.Equal(5, dial.ZeroCount);
        
        dial.Rotate('L', 82); // To 32, passes 0 once
        Assert.Equal(32, dial.CurrentPosition);
        Assert.Equal(6, dial.ZeroCount);
    }
    
    [Fact]
    public void ParseInstruction_ParsesLeftRotation()
    {
        // Arrange & Act
        var (direction, distance) = SafeDialAdvanced.ParseInstruction("L68");
        
        // Assert
        Assert.Equal('L', direction);
        Assert.Equal(68, distance);
    }
    
    [Fact]
    public void ParseInstruction_ParsesRightRotation()
    {
        // Arrange & Act
        var (direction, distance) = SafeDialAdvanced.ParseInstruction("R48");
        
        // Assert
        Assert.Equal('R', direction);
        Assert.Equal(48, distance);
    }
}
