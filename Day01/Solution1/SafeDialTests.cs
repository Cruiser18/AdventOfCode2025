using Xunit;

namespace Solution1.Tests;

public class SafeDialTests
{
    [Fact]
    public void Dial_StartsAt50()
    {
        // Arrange & Act
        var dial = new SafeDial();
        
        // Assert
        Assert.Equal(50, dial.CurrentPosition);
    }
    
    [Fact]
    public void RotateRight_FromMiddle_MovesToHigherNumber()
    {
        // Arrange
        var dial = new SafeDial();
        
        // Act
        dial.Rotate('R', 8); // From 50
        
        // Assert
        Assert.Equal(58, dial.CurrentPosition);
    }
    
    [Fact]
    public void RotateLeft_FromMiddle_MovesToLowerNumber()
    {
        // Arrange
        var dial = new SafeDial();
        
        // Act
        dial.Rotate('L', 8); // From 50
        
        // Assert
        Assert.Equal(42, dial.CurrentPosition);
    }
    
    [Fact]
    public void RotateRight_From99_WrapsTo0()
    {
        // Arrange
        var dial = new SafeDial();
        dial.Rotate('R', 49); // Move to 99
        
        // Act
        dial.Rotate('R', 1); // Should wrap to 0
        
        // Assert
        Assert.Equal(0, dial.CurrentPosition);
    }
    
    [Fact]
    public void RotateLeft_From0_WrapsTo99()
    {
        // Arrange
        var dial = new SafeDial();
        dial.Rotate('L', 50); // Move to 0
        
        // Act
        dial.Rotate('L', 1); // Should wrap to 99
        
        // Assert
        Assert.Equal(99, dial.CurrentPosition);
    }
    
    [Fact]
    public void RotateLeft_From5By10_ResultsIn95()
    {
        // Arrange
        var dial = new SafeDial();
        dial.Rotate('L', 45); // Move to 5
        
        // Act
        dial.Rotate('L', 10);
        
        // Assert
        Assert.Equal(95, dial.CurrentPosition);
    }
    
    [Fact]
    public void RotateRight_From95By5_ResultsIn0()
    {
        // Arrange
        var dial = new SafeDial();
        dial.Rotate('R', 45); // Move to 95
        
        // Act
        dial.Rotate('R', 5);
        
        // Assert
        Assert.Equal(0, dial.CurrentPosition);
    }
    
    [Fact]
    public void CountZeros_StartsAtZero()
    {
        // Arrange & Act
        var dial = new SafeDial();
        
        // Assert
        Assert.Equal(0, dial.ZeroCount);
    }
    
    [Fact]
    public void CountZeros_IncrementsWhenLandingOn0()
    {
        // Arrange
        var dial = new SafeDial();
        
        // Act
        dial.Rotate('L', 50); // Lands on 0
        
        // Assert
        Assert.Equal(1, dial.ZeroCount);
    }
    
    [Fact]
    public void CountZeros_DoesNotIncrementWhenNotLandingOn0()
    {
        // Arrange
        var dial = new SafeDial();
        
        // Act
        dial.Rotate('L', 10); // Lands on 40
        
        // Assert
        Assert.Equal(0, dial.ZeroCount);
    }
    
    [Fact]
    public void ParseInstruction_ParsesLeftRotation()
    {
        // Arrange & Act
        var (direction, distance) = SafeDial.ParseInstruction("L68");
        
        // Assert
        Assert.Equal('L', direction);
        Assert.Equal(68, distance);
    }
    
    [Fact]
    public void ParseInstruction_ParsesRightRotation()
    {
        // Arrange & Act
        var (direction, distance) = SafeDial.ParseInstruction("R48");
        
        // Assert
        Assert.Equal('R', direction);
        Assert.Equal(48, distance);
    }
    
    [Theory]
    [InlineData("L68", 50, 82)]
    [InlineData("R48", 52, 0)]
    [InlineData("L5", 0, 95)]
    [InlineData("R60", 95, 55)]
    [InlineData("L55", 55, 0)]
    public void ExampleRotations_ProduceCorrectPositions(string instruction, int startPos, int expectedPos)
    {
        // Arrange
        var dial = new SafeDial();
        // Move dial to start position
        dial.Rotate('R', (startPos - 50 + 100) % 100);
        var (direction, distance) = SafeDial.ParseInstruction(instruction);
        
        // Act
        dial.Rotate(direction, distance);
        
        // Assert
        Assert.Equal(expectedPos, dial.CurrentPosition);
    }
    
    [Fact]
    public void ProcessInstructions_WithExampleInput_Returns3()
    {
        // Arrange
        var instructions = new[]
        {
            "L68", "L30", "R48", "L5", "R60", 
            "L55", "L1", "L99", "R14", "L82"
        };
        var dial = new SafeDial();
        
        // Act
        foreach (var instruction in instructions)
        {
            var (direction, distance) = SafeDial.ParseInstruction(instruction);
            dial.Rotate(direction, distance);
        }
        
        // Assert
        Assert.Equal(3, dial.ZeroCount);
    }
    
    [Fact]
    public void ProcessInstructions_TracksPositionsCorrectly()
    {
        // Arrange
        var dial = new SafeDial();
        
        // Act & Assert - Following example step by step
        Assert.Equal(50, dial.CurrentPosition); // Starts at 50
        
        dial.Rotate('L', 68);
        Assert.Equal(82, dial.CurrentPosition);
        Assert.Equal(0, dial.ZeroCount);
        
        dial.Rotate('L', 30);
        Assert.Equal(52, dial.CurrentPosition);
        Assert.Equal(0, dial.ZeroCount);
        
        dial.Rotate('R', 48);
        Assert.Equal(0, dial.CurrentPosition);
        Assert.Equal(1, dial.ZeroCount); // First zero
        
        dial.Rotate('L', 5);
        Assert.Equal(95, dial.CurrentPosition);
        Assert.Equal(1, dial.ZeroCount);
        
        dial.Rotate('R', 60);
        Assert.Equal(55, dial.CurrentPosition);
        Assert.Equal(1, dial.ZeroCount);
        
        dial.Rotate('L', 55);
        Assert.Equal(0, dial.CurrentPosition);
        Assert.Equal(2, dial.ZeroCount); // Second zero
        
        dial.Rotate('L', 1);
        Assert.Equal(99, dial.CurrentPosition);
        Assert.Equal(2, dial.ZeroCount);
        
        dial.Rotate('L', 99);
        Assert.Equal(0, dial.CurrentPosition);
        Assert.Equal(3, dial.ZeroCount); // Third zero
        
        dial.Rotate('R', 14);
        Assert.Equal(14, dial.CurrentPosition);
        Assert.Equal(3, dial.ZeroCount);
        
        dial.Rotate('L', 82);
        Assert.Equal(32, dial.CurrentPosition);
        Assert.Equal(3, dial.ZeroCount);
    }
}
