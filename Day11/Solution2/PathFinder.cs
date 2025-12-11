namespace Solution2;

public class PathFinder
{
    public static (string device, List<string> outputs) ParseDeviceLine(string line)
    {
        var parts = line.Split(": ");
        return (parts[0], parts[1].Split(' ').ToList());
    }

    public static Dictionary<string, List<string>> BuildGraph(string[] lines)
    {
        return lines
            .Select(line => ParseDeviceLine(line))
            .ToDictionary(parsed => parsed.device, parsed => parsed.outputs);
    }

    public static long CountPathsWithRequiredNodes(Dictionary<string, List<string>> graph, string start, string end, HashSet<string> requiredNodes)
    {
        var memo = new Dictionary<(string, string), long>();
        return CountPathsHelper(graph, start, end, requiredNodes, new HashSet<string>(), memo);
    }

    private static long CountPathsHelper(Dictionary<string, List<string>> graph, string current, string end, HashSet<string> requiredNodes, HashSet<string> visitedRequired, Dictionary<(string, string), long> memo)
    {
        if (current == end)
        {
            // Check if all required nodes have been visited
            return requiredNodes.IsSubsetOf(visitedRequired) ? 1 : 0;
        }
        
        if (!graph.ContainsKey(current))
            return 0;
        
        // Create a key for memoization based on current node and which required nodes we've seen
        string visitedKey = string.Join(",", visitedRequired.OrderBy(x => x));
        var memoKey = (current, visitedKey);
        
        if (memo.ContainsKey(memoKey))
            return memo[memoKey];
        
        // Track required nodes we've visited
        var newVisitedRequired = new HashSet<string>(visitedRequired);
        if (requiredNodes.Contains(current))
            newVisitedRequired.Add(current);
        
        long count = graph[current].Sum(next => CountPathsHelper(graph, next, end, requiredNodes, newVisitedRequired, memo));
        
        memo[memoKey] = count;
        return count;
    }
}
