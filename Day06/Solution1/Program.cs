using Solution1;

string[] lines = File.ReadAllLines("../input.txt");

var worksheet = new MathWorksheet();
worksheet.ParseWorksheet(lines);

long grandTotal = worksheet.GetGrandTotal();

Console.WriteLine($"Grand Total: {grandTotal}");
