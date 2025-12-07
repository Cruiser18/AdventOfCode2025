using Xunit;

namespace Solution2;

public class MathWorksheetPart2Tests
{
    [Fact]
    public void ParseWorksheet_WithTestInput_ReturnsFourProblems()
    {
        // Arrange
        string[] lines = new[]
        {
            "123 328  51 64 ",
            " 45 64  387 23 ",
            "  6 98  215 314",
            "*   +   *   +  "
        };
        var worksheet = new MathWorksheetPart2();

        // Act
        worksheet.ParseWorksheet(lines);

        // Assert
        Assert.Equal(4, worksheet.Problems.Count);
    }

    [Fact]
    public void ParseWorksheet_RightmostProblem_ReadsRightToLeft()
    {
        // Arrange - Rightmost column reads as: 4 + 431 + 623 = 1058
        string[] lines = new[]
        {
            "123 328  51 64 ",
            " 45 64  387 23 ",
            "  6 98  215 314",
            "*   +   *   +  "
        };
        var worksheet = new MathWorksheetPart2();

        // Act
        worksheet.ParseWorksheet(lines);

        // Assert - rightmost is Problems[0] since we read right-to-left
        var problem = worksheet.Problems[0];
        Assert.Equal('+', problem.Operator);
        Assert.Equal(new[] { 4, 431, 623 }, problem.Numbers);
    }

    [Fact]
    public void ParseWorksheet_SecondFromRight_ReadsRightToLeft()
    {
        // Arrange - Second from right: 175 * 581 * 32 = 3253600
        string[] lines = new[]
        {
            "123 328  51 64 ",
            " 45 64  387 23 ",
            "  6 98  215 314",
            "*   +   *   +  "
        };
        var worksheet = new MathWorksheetPart2();

        // Act
        worksheet.ParseWorksheet(lines);

        // Assert
        var problem = worksheet.Problems[1];
        Assert.Equal('*', problem.Operator);
        Assert.Equal(new[] { 175, 581, 32 }, problem.Numbers);
    }

    [Fact]
    public void ParseWorksheet_ThirdFromRight_ReadsRightToLeft()
    {
        // Arrange - Third from right: 8 + 248 + 369 = 625
        string[] lines = new[]
        {
            "123 328  51 64 ",
            " 45 64  387 23 ",
            "  6 98  215 314",
            "*   +   *   +  "
        };
        var worksheet = new MathWorksheetPart2();

        // Act
        worksheet.ParseWorksheet(lines);

        // Assert
        var problem = worksheet.Problems[2];
        Assert.Equal('+', problem.Operator);
        Assert.Equal(new[] { 8, 248, 369 }, problem.Numbers);
    }

    [Fact]
    public void ParseWorksheet_LeftmostProblem_ReadsRightToLeft()
    {
        // Arrange - Leftmost: 356 * 24 * 1 = 8544
        string[] lines = new[]
        {
            "123 328  51 64 ",
            " 45 64  387 23 ",
            "  6 98  215 314",
            "*   +   *   +  "
        };
        var worksheet = new MathWorksheetPart2();

        // Act
        worksheet.ParseWorksheet(lines);

        // Assert
        var problem = worksheet.Problems[3];
        Assert.Equal('*', problem.Operator);
        Assert.Equal(new[] { 356, 24, 1 }, problem.Numbers);
    }

    [Fact]
    public void CalculateProblem_RightmostProblem_Returns1058()
    {
        // Arrange
        var problem = new Problem
        {
            Numbers = new[] { 4, 431, 623 },
            Operator = '+'
        };

        // Act
        long result = MathWorksheetPart2.CalculateProblem(problem);

        // Assert
        Assert.Equal(1058, result);
    }

    [Fact]
    public void CalculateProblem_SecondProblem_Returns3253600()
    {
        // Arrange
        var problem = new Problem
        {
            Numbers = new[] { 175, 581, 32 },
            Operator = '*'
        };

        // Act
        long result = MathWorksheetPart2.CalculateProblem(problem);

        // Assert
        Assert.Equal(3253600, result);
    }

    [Fact]
    public void CalculateProblem_LeftmostProblem_Returns8544()
    {
        // Arrange
        var problem = new Problem
        {
            Numbers = new[] { 356, 24, 1 },
            Operator = '*'
        };

        // Act
        long result = MathWorksheetPart2.CalculateProblem(problem);

        // Assert
        Assert.Equal(8544, result);
    }

    [Fact]
    public void GetGrandTotal_WithTestInput_Returns3263827()
    {
        // Arrange
        string[] lines = new[]
        {
            "123 328  51 64 ",
            " 45 64  387 23 ",
            "  6 98  215 314",
            "*   +   *   +  "
        };
        var worksheet = new MathWorksheetPart2();
        worksheet.ParseWorksheet(lines);

        // Act
        long grandTotal = worksheet.GetGrandTotal();

        // Assert
        Assert.Equal(3263827, grandTotal);
    }
}
