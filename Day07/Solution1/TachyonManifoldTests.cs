using Xunit;

namespace Solution1;

public class TachyonManifoldTests
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
        var manifold = new TachyonManifold(lines);

        // Act
        var (row, col) = manifold.StartPosition;

        // Assert
        Assert.Equal(0, row);
        Assert.Equal(7, col);
    }

    [Fact]
    public void SimulateBeam_NoSplitters_NoSplits()
    {
        // Arrange
        string[] lines = new[]
        {
            "...S...",
            ".......",
            "......."
        };
        var manifold = new TachyonManifold(lines);

        // Act
        int splits = manifold.CountBeamSplits();

        // Assert
        Assert.Equal(0, splits);
    }

    [Fact]
    public void SimulateBeam_OneSplitter_OneSplit()
    {
        // Arrange
        string[] lines = new[]
        {
            "...S...",
            ".......",
            "...^..."
        };
        var manifold = new TachyonManifold(lines);

        // Act
        int splits = manifold.CountBeamSplits();

        // Assert
        Assert.Equal(1, splits);
    }

    [Fact]
    public void SimulateBeam_TwoSplittersInLine_OneSplit()
    {
        // Arrange
        string[] lines = new[]
        {
            "...S...",
            ".......",
            "...^...",
            ".......",
            "...^..."
        };
        var manifold = new TachyonManifold(lines);

        // Act
        int splits = manifold.CountBeamSplits();

        // Assert
        // Beam hits first splitter at (2,3), creates beams at (2,2) and (2,4)
        // Those beams don't hit any more splitters (only col 3 has splitters)
        Assert.Equal(1, splits);
    }

    [Fact]
    public void SimulateBeam_SplitterCreatesBeamsThatHitMoreSplitters()
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
        var manifold = new TachyonManifold(lines);

        // Act
        int splits = manifold.CountBeamSplits();

        // Assert
        // First split at row 2, creates 2 beams
        // Those 2 beams each hit splitters at row 4
        Assert.Equal(3, splits);
    }

    [Fact]
    public void SimulateBeam_BeamsMergeInSameColumn()
    {
        // Arrange - Two splitters that will create beams going to same column
        string[] lines = new[]
        {
            ".......S.......",
            "...............",
            ".......^.......",
            "...............",
            "......^.^......",
            "...............",
            ".......^......."
        };
        var manifold = new TachyonManifold(lines);

        // Act
        int splits = manifold.CountBeamSplits();

        // Assert
        // First split at (2,7) creates beams at (2,6) and (2,8)
        // Beam at (2,6) hits splitter at (4,6) -> splits
        // Beam at (2,8) hits splitter at (4,8) -> splits
        // Now we have beams at (4,5), (4,7) [middle], (4,9)
        // Middle beam (4,7) continues and hits splitter at (6,7) -> splits
        Assert.Equal(4, splits);
    }

    [Fact]
    public void SimulateBeam_FullTestInput_Returns21Splits()
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
        var manifold = new TachyonManifold(lines);

        // Act
        int splits = manifold.CountBeamSplits();

        // Assert
        Assert.Equal(21, splits);
    }

    [Fact]
    public void SimulateBeam_BeamExitsManifold_NoSplit()
    {
        // Arrange
        string[] lines = new[]
        {
            "...S...",
            ".......",
            "..^.^.."
        };
        var manifold = new TachyonManifold(lines);

        // Act
        int splits = manifold.CountBeamSplits();

        // Assert
        // Beam starts at (0,3) goes down
        // At row 2, col 3 is '.', not '^', so beam just exits
        Assert.Equal(0, splits);
    }
}
