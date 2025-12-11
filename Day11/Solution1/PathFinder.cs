namespace Solution1;

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

    public static int CountPaths(Dictionary<string, List<string>> graph, string start, string end)
    {
        if (start == end)
            return 1;
        
        if (!graph.ContainsKey(start))
            return 0;
        
        return graph[start].Sum(next => CountPaths(graph, next, end));
    }
}
