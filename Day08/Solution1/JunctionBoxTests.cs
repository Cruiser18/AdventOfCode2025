using Xunit;

namespace Solution1;

public class JunctionBoxTests
{
    // TEST LIST
    // 1. Parse junction box coordinates from input
    // 2. Calculate distance between two junction boxes
    // 3. Connect boxes and form circuits (Union-Find)
    // 4. After 10 connections in example, result is 40 (5 * 4 * 2)
    // 5. Part 2: Find last connection to unite all circuits (216 * 117 = 25272)

    [Fact]
    public void CalculateResult_Example10Connections_Returns40()
    {
        // Arrange
        var input = new[]
        {
            "162,817,812",
            "57,618,57",
            "906,360,560",
            "592,479,940",
            "352,342,300",
            "466,668,158",
            "542,29,236",
            "431,825,988",
            "739,650,466",
            "52,470,668",
            "216,146,977",
            "819,987,18",
            "117,168,530",
            "805,96,715",
            "346,949,466",
            "970,615,88",
            "941,993,340",
            "862,61,35",
            "984,92,344",
            "425,690,689"
        };

        // Act
        int result = JunctionBox.CalculateResult(input, 10);

        // Assert
        Assert.Equal(40, result); // 5 * 4 * 2 (three largest circuits)
    }

    [Fact]
    public void CalculateDistance_TwoPoints_ReturnsEuclideanDistance()
    {
        // Arrange
        var box1 = new JunctionBox.Point3D(0, 0, 0);
        var box2 = new JunctionBox.Point3D(3, 4, 0);

        // Act
        double distance = JunctionBox.CalculateDistance(box1, box2);

        // Assert
        Assert.Equal(5.0, distance, 2); // sqrt(9 + 16) = 5
    }

    [Fact]
    public void FindLastConnection_ExampleInput_Returns25272()
    {
        // Arrange
        var input = new[]
        {
            "162,817,812",
            "57,618,57",
            "906,360,560",
            "592,479,940",
            "352,342,300",
            "466,668,158",
            "542,29,236",
            "431,825,988",
            "739,650,466",
            "52,470,668",
            "216,146,977",
            "819,987,18",
            "117,168,530",
            "805,96,715",
            "346,949,466",
            "970,615,88",
            "941,993,340",
            "862,61,35",
            "984,92,344",
            "425,690,689"
        };

        // Act
        int result = JunctionBox.FindLastConnectionProduct(input);

        // Assert
        Assert.Equal(25272, result); // 216 * 117 (X coordinates of last connection)
    }
}
