using Solution2;

// Read input file
var inputPath = Path.Combine("..", "input.txt");
var instructions = File.ReadAllLines(inputPath);

// Process the safe dial with advanced counting (method 0x434C49434B)
var dial = new SafeDialAdvanced();

foreach (var instruction in instructions)
{
    var (direction, distance) = SafeDialAdvanced.ParseInstruction(instruction);
    dial.Rotate(direction, distance);
}

Console.WriteLine($"The password (method 0x434C49434B) is: {dial.ZeroCount}");
