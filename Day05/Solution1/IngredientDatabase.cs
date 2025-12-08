namespace Solution1;

public class IngredientDatabase
{
    public static int CountFreshIngredients(string[] input)
    {
        // Parse the input to get ranges and available IDs
        var (ranges, availableIds) = ParseInput(input);
        
        // Count how many available IDs are fresh
        int freshCount = 0;
        foreach (var id in availableIds)
        {
            if (IsFresh(id, ranges))
            {
                freshCount++;
            }
        }
        
        return freshCount;
    }
    
    public static bool IsFresh(long id, (long start, long end)[] ranges)
    {
        // Check if the ID falls within any of the ranges
        foreach (var (start, end) in ranges)
        {
            if (id >= start && id <= end)
            {
                return true;
            }
        }
        return false;
    }
    
    public static long CountTotalFreshIds(string[] input)
    {
        // Parse to get just the ranges (ignore available IDs)
        var (ranges, _) = ParseInput(input);
        
        // Merge overlapping ranges
        var mergedRanges = MergeRanges(ranges);
        
        // Count total IDs covered by merged ranges
        long totalCount = 0;
        foreach (var (start, end) in mergedRanges)
        {
            totalCount += (end - start + 1); // +1 because ranges are inclusive
        }
        
        return totalCount;
    }
    
    public static (long start, long end)[] MergeRanges((long start, long end)[] ranges)
    {
        if (ranges.Length == 0)
            return ranges;
        
        // Sort ranges by start position
        var sorted = ranges.OrderBy(r => r.start).ToArray();
        
        var merged = new List<(long start, long end)>();
        var current = sorted[0];
        
        for (int i = 1; i < sorted.Length; i++)
        {
            var next = sorted[i];
            
            // Check if ranges overlap or are adjacent
            if (next.start <= current.end + 1)
            {
                // Merge by extending the end of current range
                current = (current.start, Math.Max(current.end, next.end));
            }
            else
            {
                // No overlap, add current to result and start new range
                merged.Add(current);
                current = next;
            }
        }
        
        // Add the last range
        merged.Add(current);
        
        return merged.ToArray();
    }
    
    private static ((long start, long end)[] ranges, long[] availableIds) ParseInput(string[] input)
    {
        var ranges = new List<(long start, long end)>();
        var availableIds = new List<long>();
        bool parsingRanges = true;
        
        foreach (var line in input)
        {
            // Empty line separates ranges from available IDs
            if (string.IsNullOrWhiteSpace(line))
            {
                parsingRanges = false;
                continue;
            }
            
            if (parsingRanges)
            {
                // Parse range like "3-5"
                var parts = line.Split('-');
                long start = long.Parse(parts[0]);
                long end = long.Parse(parts[1]);
                ranges.Add((start, end));
            }
            else
            {
                // Parse available ID
                availableIds.Add(long.Parse(line));
            }
        }
        
        return (ranges.ToArray(), availableIds.ToArray());
    }
}
