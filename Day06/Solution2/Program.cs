using Solution2;

string[] lines = File.ReadAllLines("../input.txt");

var worksheet = new MathWorksheetPart2();
worksheet.ParseWorksheet(lines);

long grandTotal = worksheet.GetGrandTotal();

Console.WriteLine($"Grand Total (Part 2): {grandTotal}");
