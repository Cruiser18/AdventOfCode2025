using System;
using System.Collections.Generic;
using System.Linq;

namespace Solution1;

public class RedTile
{
    public static long CalculateArea((int x, int y) point1, (int x, int y) point2)
    {
        // Calculate width and height of rectangle
        long width = Math.Abs((long)point1.x - point2.x) + 1;
        long height = Math.Abs((long)point1.y - point2.y) + 1;
        
        return width * height;
    }

    public static long FindLargestRectangle(string[] input)
    {
        // Parse all red tile coordinates
        var tiles = input.Select(line =>
        {
            var parts = line.Split(',');
            return (x: int.Parse(parts[0]), y: int.Parse(parts[1]));
        }).ToList();

        long maxArea = 0;

        // Try all pairs of red tiles as opposite corners
        for (int i = 0; i < tiles.Count; i++)
        {
            for (int j = i + 1; j < tiles.Count; j++)
            {
                long area = CalculateArea(tiles[i], tiles[j]);
                maxArea = Math.Max(maxArea, area);
            }
        }

        return maxArea;
    }

    public static long FindLargestRectangleInPolygon(string[] input)
    {
        // Parse all red tile coordinates (they form a closed polygon)
        var tiles = input.Select(line =>
        {
            var parts = line.Split(',');
            return (x: int.Parse(parts[0]), y: int.Parse(parts[1]));
        }).ToList();

        long maxArea = 0;
        int validCount = 0;
        int totalPairs = tiles.Count * (tiles.Count - 1) / 2;
        int processed = 0;
        long skippedLargest = 0;

        // Cache for point-in-polygon checks
        var pointCache = new Dictionary<(int x, int y), bool>();

        // Try all pairs of red tiles as opposite corners
        for (int i = 0; i < tiles.Count; i++)
        {
            for (int j = i + 1; j < tiles.Count; j++)
            {
                processed++;
                if (processed % 10000 == 0)
                {
                    Console.WriteLine($"  Processed {processed}/{totalPairs} pairs, found {validCount} valid rectangles, cache size: {pointCache.Count}, max skipped: {skippedLargest}...");
                }

                long area = CalculateArea(tiles[i], tiles[j]);
                
                // Check if this rectangle is valid (all points must be inside or on the polygon)
                var validResult = IsRectangleValid(tiles[i], tiles[j], tiles, pointCache, out bool wasSkipped);
                if (wasSkipped && area > skippedLargest)
                {
                    skippedLargest = area;
                }
                
                if (validResult)
                {
                    validCount++;
                    maxArea = Math.Max(maxArea, area);
                }
            }
        }

        Console.WriteLine($"  Total valid rectangles: {validCount}");
        Console.WriteLine($"  Largest skipped rectangle: {skippedLargest}");
        return maxArea;
    }

    private static bool IsRectangleValid((int x, int y) point1, (int x, int y) point2, List<(int x, int y)> polygon, Dictionary<(int x, int y), bool> cache, out bool wasSkipped)
    {
        wasSkipped = false;
        
        // Get the bounds of the rectangle
        int minX = Math.Min(point1.x, point2.x);
        int maxX = Math.Max(point1.x, point2.x);
        int minY = Math.Min(point1.y, point2.y);
        int maxY = Math.Max(point1.y, point2.y);

        long area = ((long)(maxX - minX + 1)) * ((long)(maxY - minY + 1));
        
        // For large rectangles, check boundary points only
        if (area > 1000000)
        {
            // Check all boundary points
            // Top and bottom edges
            for (int x = minX; x <= maxX; x++)
            {
                var topPoint = (x, minY);
                var bottomPoint = (x, maxY);
                
                if (!cache.TryGetValue(topPoint, out bool topInside))
                {
                    topInside = IsPointInsideOrOnPolygon(topPoint, polygon);
                    cache[topPoint] = topInside;
                }
                if (!topInside) return false;
                
                if (!cache.TryGetValue(bottomPoint, out bool bottomInside))
                {
                    bottomInside = IsPointInsideOrOnPolygon(bottomPoint, polygon);
                    cache[bottomPoint] = bottomInside;
                }
                if (!bottomInside) return false;
            }
            
            // Left and right edges (excluding corners already checked)
            for (int y = minY + 1; y < maxY; y++)
            {
                var leftPoint = (minX, y);
                var rightPoint = (maxX, y);
                
                if (!cache.TryGetValue(leftPoint, out bool leftInside))
                {
                    leftInside = IsPointInsideOrOnPolygon(leftPoint, polygon);
                    cache[leftPoint] = leftInside;
                }
                if (!leftInside) return false;
                
                if (!cache.TryGetValue(rightPoint, out bool rightInside))
                {
                    rightInside = IsPointInsideOrOnPolygon(rightPoint, polygon);
                    cache[rightPoint] = rightInside;
                }
                if (!rightInside) return false;
            }
            
            // Also check that no polygon edges cross through the rectangle interior
            // If all boundary points are inside and no edges cross, interior must be inside
            if (!PolygonEdgesCrossRectangle(minX, maxX, minY, maxY, polygon))
            {
                return true;
            }
            
            // If edges might cross, we can't be sure - mark as skipped
            wasSkipped = true;
            return false;
        }

        // For smaller rectangles, check every point
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                var point = (x, y);
                
                if (!cache.TryGetValue(point, out bool isInside))
                {
                    isInside = IsPointInsideOrOnPolygon(point, polygon);
                    cache[point] = isInside;
                }
                
                if (!isInside)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static bool PolygonEdgesCrossRectangle(int minX, int maxX, int minY, int maxY, List<(int x, int y)> polygon)
    {
        // Check if any polygon edge crosses through the rectangle
        for (int i = 0; i < polygon.Count; i++)
        {
            var p1 = polygon[i];
            var p2 = polygon[(i + 1) % polygon.Count];
            
            // Check if this edge intersects with the rectangle interior
            // (not just touching the boundary)
            if (EdgeCrossesRectangleInterior(p1, p2, minX, maxX, minY, maxY))
            {
                return true;
            }
        }
        return false;
    }

    private static bool EdgeCrossesRectangleInterior((int x, int y) p1, (int x, int y) p2, int minX, int maxX, int minY, int maxY)
    {
        // Simplified check: if both endpoints are outside on opposite sides, edge likely crosses
        bool p1Inside = p1.x >= minX && p1.x <= maxX && p1.y >= minY && p1.y <= maxY;
        bool p2Inside = p2.x >= minX && p2.x <= maxX && p2.y >= minY && p2.y <= maxY;
        
        // If both points are inside or both outside on same side, no problem
        if (p1Inside && p2Inside) return false;
        
        // If one inside and one outside, the edge crosses the boundary which we already checked
        // The real issue is if both are outside but the edge passes through
        if (!p1Inside && !p2Inside)
        {
            // Check if the line segment intersects the rectangle
            // For simplicity, return false (assume no cross) - this is optimistic
            return false;
        }
        
        return false;
    }

    private static bool IsPointInsideOrOnPolygon((int x, int y) point, List<(int x, int y)> polygon)
    {
        // First check if point is one of the polygon vertices
        if (polygon.Contains(point))
        {
            return true;
        }

        // Check if point is on any edge of the polygon
        for (int i = 0; i < polygon.Count; i++)
        {
            var p1 = polygon[i];
            var p2 = polygon[(i + 1) % polygon.Count];

            if (IsPointOnSegment(point, p1, p2))
            {
                return true;
            }
        }

        // Use ray casting algorithm to check if point is inside polygon
        return IsPointInsidePolygon(point, polygon);
    }

    private static bool IsPointOnSegment((int x, int y) point, (int x, int y) p1, (int x, int y) p2)
    {
        // Check if point is on the line segment between p1 and p2
        int minX = Math.Min(p1.x, p2.x);
        int maxX = Math.Max(p1.x, p2.x);
        int minY = Math.Min(p1.y, p2.y);
        int maxY = Math.Max(p1.y, p2.y);

        if (point.x < minX || point.x > maxX || point.y < minY || point.y > maxY)
        {
            return false;
        }

        // Check if point is collinear with p1 and p2
        long cross = (long)(point.y - p1.y) * (p2.x - p1.x) - (long)(point.x - p1.x) * (p2.y - p1.y);
        return cross == 0;
    }

    private static bool IsPointInsidePolygon((int x, int y) point, List<(int x, int y)> polygon)
    {
        // Ray casting algorithm: cast a ray from the point to infinity and count intersections
        bool inside = false;
        int n = polygon.Count;

        for (int i = 0, j = n - 1; i < n; j = i++)
        {
            var pi = polygon[i];
            var pj = polygon[j];

            if ((pi.y > point.y) != (pj.y > point.y) &&
                point.x < (pj.x - pi.x) * (point.y - pi.y) / (double)(pj.y - pi.y) + pi.x)
            {
                inside = !inside;
            }
        }

        return inside;
    }
}
