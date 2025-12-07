using Solution1;

string[] lines = File.ReadAllLines("../input.txt");

var manifold = new TachyonManifold(lines);
int splits = manifold.CountBeamSplits();

Console.WriteLine($"Total beam splits: {splits}");
