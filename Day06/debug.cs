string[] lines = new[]
{
    "123 328  51 64 ",
    " 45 64  387 23 ",
    "  6 98  215 314",
    "*   +   *   +  "
};

// Print with position markers
Console.WriteLine("012345678901234567890");
foreach (var line in lines)
{
    Console.WriteLine(line);
}

// Rightmost problem should be: 4 + 431 + 623
// Let's trace column by column from right to left
Console.WriteLine("\nAnalyzing rightmost problem:");
Console.WriteLine("Position 14: " + (lines[0].Length > 14 ? lines[0][14] : ' ') + ", " + (lines[1].Length > 14 ? lines[1][14] : ' ') + ", " + (lines[2].Length > 14 ? lines[2][14] : ' '));
Console.WriteLine("Position 13: " + lines[0][13] + ", " + lines[1][13] + ", " + lines[2][13]);
Console.WriteLine("Position 12: " + lines[0][12] + ", " + lines[1][12] + ", " + lines[2][12]);
