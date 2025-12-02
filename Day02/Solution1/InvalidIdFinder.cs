namespace Solution1;

public class InvalidIdFinder
{
    public static bool IsInvalidId(long id)
    {
        string idStr = id.ToString();

        // Must be even length to be repeated twice
        if (idStr.Length % 2 != 0)
            return false;

        int halfLength = idStr.Length / 2;
        return idStr.Substring(0, halfLength) == idStr.Substring(halfLength);
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
