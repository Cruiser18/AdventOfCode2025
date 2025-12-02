namespace Solution2;

public class InvalidIdFinderAdvanced
{
    public static bool IsInvalidId(long id)
    {
        string idStr = id.ToString();
        int length = idStr.Length;
        
        // Try all possible pattern lengths from 1 to length/2
        for (int patternLength = 1; patternLength <= length / 2; patternLength++)
        {
            // Check if length is divisible by patternLength (i.e., repeats evenly)
            if (length % patternLength == 0)
            {
                string pattern = idStr.Substring(0, patternLength);
                bool isRepeated = true;
                
                // Check if the entire string is this pattern repeated
                for (int i = patternLength; i < length; i += patternLength)
                {
                    if (idStr.Substring(i, patternLength) != pattern)
                    {
                        isRepeated = false;
                        break;
                    }
                }
                
                if (isRepeated)
                    return true;
            }
        }
        
        return false;
    }

    public static (long start, long end) ParseRange(string range)
    {
        string[] parts = range.Split('-');
        return (long.Parse(parts[0]), long.Parse(parts[1]));
    }

    public static List<long> FindInvalidIdsInRange(long start, long end)
    {
        var invalidIds = new List<long>();
        for (long i = start; i <= end; i++)
        {
            if (IsInvalidId(i))
            {
                invalidIds.Add(i);
            }
        }
        return invalidIds;
    }

    public static long SumInvalidIds(string input)
    {
        return input.Split(',')
            .SelectMany(range =>
            {
                var (start, end) = ParseRange(range.Trim());
                return FindInvalidIdsInRange(start, end);
            })
            .Sum();
    }
}
