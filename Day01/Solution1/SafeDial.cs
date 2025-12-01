namespace Solution1;

public class SafeDial
{
    public int CurrentPosition { get; private set; }
    public int ZeroCount { get; private set; }

    public SafeDial()
    {
        CurrentPosition = 50;
        ZeroCount = 0;
    }

    public void Rotate(char direction, int distance)
    {
        if (direction == 'R')
        {
            CurrentPosition = (CurrentPosition + distance) % 100;
        }
        else if (direction == 'L')
        {
            CurrentPosition = (CurrentPosition - distance + 100 * ((distance / 100) + 1)) % 100;
        }

        if (CurrentPosition == 0)
        {
            ZeroCount++;
        }
    }

    public static (char direction, int distance) ParseInstruction(string instruction)
    {
        char direction = instruction[0];
        int distance = int.Parse(instruction.Substring(1));
        return (direction, distance);
    }
}
