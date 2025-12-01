using Solution1;

// Read input file
var inputPath = Path.Combine("..", "input.txt");
var instructions = File.ReadAllLines(inputPath);

// Process the safe dial
var dial = new SafeDial();

foreach (var instruction in instructions)
{
    var (direction, distance) = SafeDial.ParseInstruction(instruction);
    dial.Rotate(direction, distance);
}

Console.WriteLine($"The password is: {dial.ZeroCount}");
