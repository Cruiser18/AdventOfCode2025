using Solution2;

string[] lines = File.ReadAllLines("../input.txt");

var manifold = new QuantumTachyonManifold(lines);
long timelines = manifold.CountTimelines();

Console.WriteLine($"Total timelines: {timelines}");
