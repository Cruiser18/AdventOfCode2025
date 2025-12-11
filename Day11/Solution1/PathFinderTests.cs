using Xunit;

namespace Solution1;

public class PathFinderTests
{
    [Fact]
    public void ParseDeviceLine_SingleOutput_ReturnsDeviceAndOutputs()
    {
        // Arrange
        string line = "bbb: ddd";
        
        // Act
        var (device, outputs) = PathFinder.ParseDeviceLine(line);
        
        // Assert
        Assert.Equal("bbb", device);
        Assert.Single(outputs);
        Assert.Equal("ddd", outputs[0]);
    }

    [Fact]
    public void ParseDeviceLine_MultipleOutputs_ReturnsDeviceAndAllOutputs()
    {
        // Arrange
        string line = "bbb: ddd eee";
        
        // Act
        var (device, outputs) = PathFinder.ParseDeviceLine(line);
        
        // Assert
        Assert.Equal("bbb", device);
        Assert.Equal(2, outputs.Count);
        Assert.Equal("ddd", outputs[0]);
        Assert.Equal("eee", outputs[1]);
    }

    [Fact]
    public void BuildGraph_MultipleDevices_CreatesGraphStructure()
    {
        // Arrange
        var lines = new[]
        {
            "you: bbb ccc",
            "bbb: ddd",
            "ccc: eee",
            "ddd: out",
            "eee: out"
        };
        
        // Act
        var graph = PathFinder.BuildGraph(lines);
        
        // Assert
        Assert.Equal(5, graph.Count);
        Assert.Contains("you", graph.Keys);
        Assert.Contains("bbb", graph.Keys);
        Assert.Equal(2, graph["you"].Count);
        Assert.Contains("bbb", graph["you"]);
        Assert.Contains("ccc", graph["you"]);
        Assert.Single(graph["bbb"]);
        Assert.Equal("ddd", graph["bbb"][0]);
    }

    [Fact]
    public void CountPaths_ExampleGraph_Returns5()
    {
        // Arrange
        var lines = new[]
        {
            "aaa: you hhh",
            "you: bbb ccc",
            "bbb: ddd eee",
            "ccc: ddd eee fff",
            "ddd: ggg",
            "eee: out",
            "fff: out",
            "ggg: out",
            "hhh: ccc fff iii",
            "iii: out"
        };
        var graph = PathFinder.BuildGraph(lines);
        
        // Act
        int pathCount = PathFinder.CountPaths(graph, "you", "out");
        
        // Assert
        Assert.Equal(5, pathCount);
    }

    // TODO: Test edge cases
}
