namespace Solution2;

public class SafeDialAdvanced
{
    public int CurrentPosition { get; private set; }
    public int ZeroCount { get; private set; }

    public SafeDialAdvanced()
    {
        CurrentPosition = 50;
        ZeroCount = 0;
    }

    public void Rotate(char direction, int distance)
    {
        // Calculate how many times we pass through or land on 0
        // Using an optimized calculation instead of looping through each click
        int zerosEncountered = 0;
        
        if (direction == 'R')
        {
            // Moving right: calculate how many times we hit position 0
            // We hit 0 when: (CurrentPosition + n) % 100 == 0
            // This means: CurrentPosition + n = 100k for some integer k >= 1
            // So: n = 100k - CurrentPosition
            // First hit: n = 100 - CurrentPosition (if within distance)
            // Subsequent hits: every 100 clicks after that
            
            int firstHit = 100 - CurrentPosition;
            if (firstHit <= distance)
            {
                // We hit 0 at least once
                zerosEncountered = 1 + (distance - firstHit) / 100;
            }
            
            CurrentPosition = (CurrentPosition + distance) % 100;
        }
        else if (direction == 'L')
        {
            // Moving left: calculate how many times we hit position 0
            // We hit 0 when: (CurrentPosition - n) % 100 == 0
            // This means: CurrentPosition - n = 100k for some integer k (could be 0 or negative)
            // First hit going left: n = CurrentPosition (lands on 0)
            // Subsequent hits: every 100 clicks after that
            
            int firstHit = CurrentPosition;
            if (firstHit == 0)
            {
                // Starting at 0, next hit is after 100 clicks
                firstHit = 100;
            }
            
            if (firstHit <= distance)
            {
                // We hit 0 at least once
                zerosEncountered = 1 + (distance - firstHit) / 100;
            }
            
            CurrentPosition = ((CurrentPosition - distance) % 100 + 100) % 100;
        }

        ZeroCount += zerosEncountered;
    }

    public static (char direction, int distance) ParseInstruction(string instruction)
    {
        char direction = instruction[0];
        int distance = int.Parse(instruction.Substring(1));
        return (direction, distance);
    }
}
