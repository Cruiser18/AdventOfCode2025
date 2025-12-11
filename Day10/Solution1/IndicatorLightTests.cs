using Xunit;

namespace Solution1;

public class IndicatorLightTests
{
    // TEST LIST
    // 1. Parse machine configuration (lights, buttons, joltage)
    // 2. Solve for minimum button presses for a single machine
    // 3. Example: First machine requires 2 presses
    // 4. Example: Second machine requires 3 presses
    // 5. Example: Third machine requires 2 presses
    // 6. Sum of all three examples should be 7

    [Fact]
    public void SolveMinimumPresses_FirstExample_Returns2()
    {
        // Arrange
        string machine = "[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}";

        // Act
        int result = IndicatorLight.SolveMinimumPresses(machine);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void SolveMinimumPresses_SecondExample_Returns3()
    {
        // Arrange
        string machine = "[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}";

        // Act
        int result = IndicatorLight.SolveMinimumPresses(machine);

        // Assert
        Assert.Equal(3, result);
    }

    [Fact]
    public void SolveMinimumPresses_ThirdExample_Returns2()
    {
        // Arrange
        string machine = "[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}";

        // Act
        int result = IndicatorLight.SolveMinimumPresses(machine);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void SolveTotalMinimumPresses_AllExamples_Returns7()
    {
        // Arrange
        var input = new[]
        {
            "[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}",
            "[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}",
            "[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}"
        };

        // Act
        int result = IndicatorLight.SolveTotalMinimumPresses(input);

        // Assert
        Assert.Equal(7, result); // 2 + 3 + 2
    }

    // PART 2 - JOLTAGe CONFIGURATION

    [Fact]
    public void SolveMinimumPressesForJoltage_FirstExample_Returns10()
    {
        // Arrange
        string machine = "[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}";

        // Act
        long result = IndicatorLight.SolveMinimumPressesForJoltage(machine);

        // Assert
        Assert.Equal(10, result);
    }

    [Fact]
    public void SolveMinimumPressesForJoltage_SecondExample_Returns12()
    {
        // Arrange
        string machine = "[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}";

        // Act
        long result = IndicatorLight.SolveMinimumPressesForJoltage(machine);

        // Assert
        Assert.Equal(12, result);
    }

    [Fact]
    public void SolveMinimumPressesForJoltage_ThirdExample_Returns11()
    {
        // Arrange
        string machine = "[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}";

        // Act
        long result = IndicatorLight.SolveMinimumPressesForJoltage(machine);

        // Assert
        Assert.Equal(11, result);
    }

    [Fact]
    public void SolveTotalMinimumPressesForJoltage_AllExamples_Returns33()
    {
        // Arrange
        var input = new[]
        {
            "[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}",
            "[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}",
            "[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}"
        };

        // Act
        long result = IndicatorLight.SolveTotalMinimumPressesForJoltage(input);

        // Assert
        Assert.Equal(33, result); // 10 + 12 + 11
    }
}
