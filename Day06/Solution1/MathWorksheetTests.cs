using Xunit;

namespace Solution1;

public class MathWorksheetTests
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
        var worksheet = new MathWorksheet();

        // Act
        worksheet.ParseWorksheet(lines);

        // Assert
        Assert.Equal(4, worksheet.Problems.Count);
    }

    [Fact]
    public void ParseWorksheet_FirstProblem_IsMultiplication()
    {
        // Arrange
        string[] lines = new[]
        {
            "123 328  51 64 ",
            " 45 64  387 23 ",
            "  6 98  215 314",
            "*   +   *   +  "
        };
        var worksheet = new MathWorksheet();

        // Act
        worksheet.ParseWorksheet(lines);

        // Assert
        var problem = worksheet.Problems[0];
        Assert.Equal('*', problem.Operator);
        Assert.Equal(new[] { 123, 45, 6 }, problem.Numbers);
    }

    [Fact]
    public void ParseWorksheet_SecondProblem_IsAddition()
    {
        // Arrange
        string[] lines = new[]
        {
            "123 328  51 64 ",
            " 45 64  387 23 ",
            "  6 98  215 314",
            "*   +   *   +  "
        };
        var worksheet = new MathWorksheet();

        // Act
        worksheet.ParseWorksheet(lines);

        // Assert
        var problem = worksheet.Problems[1];
        Assert.Equal('+', problem.Operator);
        Assert.Equal(new[] { 328, 64, 98 }, problem.Numbers);
    }

    [Fact]
    public void CalculateProblem_Multiplication_ReturnsCorrectAnswer()
    {
        // Arrange
        var problem = new Problem
        {
            Numbers = new[] { 123, 45, 6 },
            Operator = '*'
        };

        // Act
        long result = MathWorksheet.CalculateProblem(problem);

        // Assert
        Assert.Equal(33210, result);
    }

    [Fact]
    public void CalculateProblem_Addition_ReturnsCorrectAnswer()
    {
        // Arrange
        var problem = new Problem
        {
            Numbers = new[] { 328, 64, 98 },
            Operator = '+'
        };

        // Act
        long result = MathWorksheet.CalculateProblem(problem);

        // Assert
        Assert.Equal(490, result);
    }

    [Fact]
    public void GetGrandTotal_WithTestInput_Returns4277556()
    {
        // Arrange
        string[] lines = new[]
        {
            "123 328  51 64 ",
            " 45 64  387 23 ",
            "  6 98  215 314",
            "*   +   *   +  "
        };
        var worksheet = new MathWorksheet();
        worksheet.ParseWorksheet(lines);

        // Act
        long grandTotal = worksheet.GetGrandTotal();

        // Assert
        Assert.Equal(4277556, grandTotal);
    }

    [Fact]
    public void ParseWorksheet_ThirdProblem_CorrectNumbers()
    {
        // Arrange
        string[] lines = new[]
        {
            "123 328  51 64 ",
            " 45 64  387 23 ",
            "  6 98  215 314",
            "*   +   *   +  "
        };
        var worksheet = new MathWorksheet();

        // Act
        worksheet.ParseWorksheet(lines);

        // Assert
        var problem = worksheet.Problems[2];
        Assert.Equal('*', problem.Operator);
        Assert.Equal(new[] { 51, 387, 215 }, problem.Numbers);
    }

    [Fact]
    public void CalculateProblem_ThirdProblem_Returns4243455()
    {
        // Arrange
        var problem = new Problem
        {
            Numbers = new[] { 51, 387, 215 },
            Operator = '*'
        };

        // Act
        long result = MathWorksheet.CalculateProblem(problem);

        // Assert
        Assert.Equal(4243455, result);
    }

    [Fact]
    public void ParseWorksheet_EmptyColumn_SeparatesProblems()
    {
        // Arrange - testing column separation
        string[] lines = new[]
        {
            "123  456",
            " 45  789",
            "*   +"
        };
        var worksheet = new MathWorksheet();

        // Act
        worksheet.ParseWorksheet(lines);

        // Assert
        Assert.Equal(2, worksheet.Problems.Count);
    }
}
