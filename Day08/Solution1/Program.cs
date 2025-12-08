using Solution1;

// Part 1: Connect 1000 closest pairs and find result
string[] realInput = File.ReadAllLines("../input.txt");
int result = JunctionBox.CalculateResult(realInput, 1000);
Console.WriteLine($"Part 1 - Result after 1000 connections: {result}");

// Part 2: Find the last connection that unifies all circuits
int lastConnectionProduct = JunctionBox.FindLastConnectionProduct(realInput);
Console.WriteLine($"Part 2 - Product of X coordinates of last connection: {lastConnectionProduct}");
