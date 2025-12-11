using Xunit;

namespace Solution2;

public class PathFinderTests
{
    [Fact]
    public void CountPathsWithRequiredNodes_ExampleGraph_Returns2()
    {
        // Arrange
        var lines = new[]
        {
            "svr: aaa bbb",
            "aaa: fft",
            "fft: ccc",
            "bbb: tty",
            "tty: ccc",
            "ccc: ddd eee",
            "ddd: hub",
            "hub: fff",
            "eee: dac",
            "dac: fff",
            "fff: ggg hhh",
            "ggg: out",
            "hhh: out"
        };
        var graph = PathFinder.BuildGraph(lines);
        var requiredNodes = new HashSet<string> { "dac", "fft" };
        
        // Act
        long pathCount = PathFinder.CountPathsWithRequiredNodes(graph, "svr", "out", requiredNodes);
        
        // Assert
        Assert.Equal(2L, pathCount);
    }
}
