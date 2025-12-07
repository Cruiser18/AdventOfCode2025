using Xunit;

namespace Solution2;

public class QuantumTachyonManifoldTests
{
    [Fact]
    public void FindStartPosition_WithTestInput_ReturnsCorrectPosition()
    {
        // Arrange
        string[] lines = new[]
        {
            ".......S.......",
            "...............",
            ".......^......."
        };
        var manifold = new QuantumTachyonManifold(lines);

        // Act
        var (row, col) = manifold.StartPosition;

        // Assert
        Assert.Equal(0, row);
        Assert.Equal(7, col);
    }

    [Fact]
    public void CountTimelines_NoSplitters_OneTimeline()
    {
        // Arrange
        string[] lines = new[]
        {
            "...S...",
            ".......",
            "......."
        };
        var manifold = new QuantumTachyonManifold(lines);

        // Act
        long timelines = manifold.CountTimelines();

        // Assert
        Assert.Equal(1, timelines);
    }

    [Fact]
    public void CountTimelines_OneSplitter_TwoTimelines()
    {
        // Arrange
        string[] lines = new[]
        {
            "...S...",
            ".......",
            "...^..."
        };
        var manifold = new QuantumTachyonManifold(lines);

        // Act
        long timelines = manifold.CountTimelines();

        // Assert
        // Particle hits one splitter, creates 2 timelines (left and right)
        Assert.Equal(2, timelines);
    }

    [Fact]
    public void CountTimelines_TwoSplittersInSequence_FourTimelines()
    {
        // Arrange
        string[] lines = new[]
        {
            ".......S.......",
            "...............",
            ".......^.......",
            "...............",
            "......^.^......"
        };
        var manifold = new QuantumTachyonManifold(lines);

        // Act
        long timelines = manifold.CountTimelines();

        // Assert
        // First split: 2 timelines
        // Each hits another splitter: 2 * 2 = 4 timelines
        Assert.Equal(4, timelines);
    }

    [Fact]
    public void CountTimelines_FullTestInput_Returns40Timelines()
    {
        // Arrange
        string[] lines = new[]
        {
            ".......S.......",
            "...............",
            ".......^.......",
            "...............",
            "......^.^......",
            "...............",
            ".....^.^.^.....",
            "...............",
            "....^.^...^....",
            "...............",
            "...^.^...^.^...",
            "...............",
            "..^...^.....^..",
            "...............",
            ".^.^.^.^.^...^.",
            "..............."
        };
        var manifold = new QuantumTachyonManifold(lines);

        // Act
        long timelines = manifold.CountTimelines();

        // Assert
        Assert.Equal(40, timelines);
    }

    [Fact]
    public void CountTimelines_ParticleExitsWithoutHittingSplitter()
    {
        // Arrange
        string[] lines = new[]
        {
            "...S...",
            ".......",
            "..^.^.."
        };
        var manifold = new QuantumTachyonManifold(lines);

        // Act
        long timelines = manifold.CountTimelines();

        // Assert
        // Particle at column 3 doesn't hit any splitter
        Assert.Equal(1, timelines);
    }
}
