using System;
using System.Collections.Generic;
using System.Linq;

namespace Solution1;

public class JunctionBox
{
    public record Point3D(int X, int Y, int Z);

    private class UnionFind
    {
        private readonly int[] parent;
        private readonly int[] size;

        public UnionFind(int n)
        {
            parent = new int[n];
            size = new int[n];
            for (int i = 0; i < n; i++)
            {
                parent[i] = i;
                size[i] = 1;
            }
        }

        public int Find(int x)
        {
            if (parent[x] != x)
            {
                parent[x] = Find(parent[x]); // Path compression
            }
            return parent[x];
        }

        public void Union(int x, int y)
        {
            int rootX = Find(x);
            int rootY = Find(y);

            if (rootX == rootY) return;

            // Union by size
            if (size[rootX] < size[rootY])
            {
                parent[rootX] = rootY;
                size[rootY] += size[rootX];
            }
            else
            {
                parent[rootY] = rootX;
                size[rootX] += size[rootY];
            }
        }

        public int GetSize(int x)
        {
            return size[Find(x)];
        }

        public IEnumerable<int> GetAllCircuitSizes()
        {
            var roots = new HashSet<int>();
            for (int i = 0; i < parent.Length; i++)
            {
                roots.Add(Find(i));
            }
            return roots.Select(root => size[root]);
        }

        public int GetNumberOfCircuits()
        {
            var roots = new HashSet<int>();
            for (int i = 0; i < parent.Length; i++)
            {
                roots.Add(Find(i));
            }
            return roots.Count;
        }
    }

    public static double CalculateDistance(Point3D a, Point3D b)
    {
        long dx = a.X - b.X;
        long dy = a.Y - b.Y;
        long dz = a.Z - b.Z;
        return Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }

    public static int CalculateResult(string[] input, int connectionsToMake)
    {
        // Parse junction boxes
        var boxes = input.Select(line =>
        {
            var parts = line.Split(',');
            return new Point3D(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
        }).ToList();

        int n = boxes.Count;

        // Calculate all pairwise distances
        var distances = new List<(int i, int j, double distance)>();
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                double distance = CalculateDistance(boxes[i], boxes[j]);
                distances.Add((i, j, distance));
            }
        }

        // Sort by distance (ascending)
        distances.Sort((a, b) => a.distance.CompareTo(b.distance));

        // Initialize Union-Find
        var uf = new UnionFind(n);

        // Make the specified number of connections
        int connectionsMade = 0;
        foreach (var (i, j, _) in distances)
        {
            if (connectionsMade >= connectionsToMake) break;
            uf.Union(i, j);
            connectionsMade++;
        }

        // Get the sizes of all circuits
        var circuitSizes = uf.GetAllCircuitSizes().OrderByDescending(s => s).ToList();

        // Multiply the three largest circuit sizes
        if (circuitSizes.Count >= 3)
        {
            return circuitSizes[0] * circuitSizes[1] * circuitSizes[2];
        }
        else if (circuitSizes.Count == 2)
        {
            return circuitSizes[0] * circuitSizes[1];
        }
        else if (circuitSizes.Count == 1)
        {
            return circuitSizes[0];
        }
        else
        {
            return 0;
        }
    }

    public static int FindLastConnectionProduct(string[] input)
    {
        // Parse junction boxes
        var boxes = input.Select(line =>
        {
            var parts = line.Split(',');
            return new Point3D(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
        }).ToList();

        int n = boxes.Count;

        // Calculate all pairwise distances
        var distances = new List<(int i, int j, double distance)>();
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                double distance = CalculateDistance(boxes[i], boxes[j]);
                distances.Add((i, j, distance));
            }
        }

        // Sort by distance (ascending)
        distances.Sort((a, b) => a.distance.CompareTo(b.distance));

        // Initialize Union-Find
        var uf = new UnionFind(n);

        // Keep connecting until all boxes are in one circuit
        foreach (var (i, j, _) in distances)
        {
            uf.Union(i, j);

            // Check if all boxes are now in one circuit
            if (uf.GetNumberOfCircuits() == 1)
            {
                // This is the last connection needed
                return boxes[i].X * boxes[j].X;
            }
        }

        return 0; // Should never reach here if input is valid
    }
}
