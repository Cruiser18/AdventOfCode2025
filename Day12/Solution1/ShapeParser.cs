namespace Solution1;

public record Region(int Width, int Height, List<int> ShapeIds);

public class ShapeParser
{
    public static List<(int row, int col)> ParseShape(string[] lines)
    {
        var coords = new List<(int row, int col)>();
        for (int row = 0; row < lines.Length; row++)
        {
            for (int col = 0; col < lines[row].Length; col++)
            {
                if (lines[row][col] == '#')
                {
                    coords.Add((row, col));
                }
            }
        }
        return coords;
    }

    public static List<List<(int row, int col)>> GenerateOrientations(List<(int row, int col)> shape)
    {
        var orientations = new HashSet<string>();
        var results = new List<List<(int row, int col)>>();

        // Generate all transformations: 4 rotations × 2 flips
        for (int rotation = 0; rotation < 4; rotation++)
        {
            foreach (bool flip in new[] { false, true })
            {
                var transformed = Transform(shape, rotation, flip);
                var normalized = Normalize(transformed);
                var key = GetShapeKey(normalized);

                if (orientations.Add(key))
                {
                    results.Add(normalized);
                }
            }
        }

        return results;
    }

    public static (Dictionary<int, List<(int row, int col)>> shapes, List<Region> regions) ParseInput(string[] lines)
    {
        var shapes = new Dictionary<int, List<(int row, int col)>>();
        var regions = new List<Region>();
        
        int i = 0;
        while (i < lines.Length)
        {
            var line = lines[i].Trim();
            
            // Skip empty lines
            if (string.IsNullOrEmpty(line))
            {
                i++;
                continue;
            }
            
            // Check if it's a region line (contains 'x' and ':')
            if (line.Contains('x') && line.Contains(':') && !line.EndsWith(':'))
            {
                regions.Add(ParseRegion(line));
                i++;
            }
            // Check if it's a shape header (ends with ':')
            else if (line.EndsWith(':'))
            {
                var shapeId = int.Parse(line.TrimEnd(':'));
                var shapeLines = new List<string>();
                i++;
                
                // Read shape lines until empty line or end
                while (i < lines.Length && !string.IsNullOrEmpty(lines[i].Trim()))
                {
                    shapeLines.Add(lines[i]);
                    i++;
                }
                
                shapes[shapeId] = ParseShape(shapeLines.ToArray());
            }
            else
            {
                i++;
            }
        }
        
        return (shapes, regions);
    }

    public static Region ParseRegion(string line)
    {
        // Format: "38x47: 30 28 36 34 25 27"
        // Means: 38x47 region with 30 of shape 0, 28 of shape 1, 36 of shape 2, etc.
        var parts = line.Split(':', StringSplitOptions.TrimEntries);
        var dimensions = parts[0].Split('x');
        var width = int.Parse(dimensions[0]);
        var height = int.Parse(dimensions[1]);
        
        var counts = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                             .Select(int.Parse)
                             .ToList();
        
        // Expand counts into actual shape IDs
        var shapeIds = new List<int>();
        for (int shapeId = 0; shapeId < counts.Count; shapeId++)
        {
            for (int count = 0; count < counts[shapeId]; count++)
            {
                shapeIds.Add(shapeId);
            }
        }
        
        return new Region(width, height, shapeIds);
    }

    private static List<(int row, int col)> Transform(List<(int row, int col)> shape, int rotation, bool flip)
    {
        var result = new List<(int row, int col)>(shape);

        // Apply flip (horizontal)
        if (flip)
        {
            result = result.Select(p => (p.row, -p.col)).ToList();
        }

        // Apply rotation (90 degrees counterclockwise per step)
        for (int i = 0; i < rotation; i++)
        {
            result = result.Select(p => (-p.col, p.row)).ToList();
        }

        return result;
    }

    private static List<(int row, int col)> Normalize(List<(int row, int col)> shape)
    {
        if (shape.Count == 0) return shape;

        var minRow = shape.Min(p => p.row);
        var minCol = shape.Min(p => p.col);

        return shape.Select(p => (row: p.row - minRow, col: p.col - minCol))
                    .OrderBy(p => p.row)
                    .ThenBy(p => p.col)
                    .ToList();
    }

    private static string GetShapeKey(List<(int row, int col)> shape)
    {
        return string.Join(";", shape.Select(p => $"{p.row},{p.col}"));
    }
}
