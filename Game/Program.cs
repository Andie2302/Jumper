using Jumper;


Console.WriteLine("--- KI Jumper Training ---");

const int populationSize = 1000;
const int generations = 200;

var population = new List<SimpleNetwork>();
for (var i = 0; i < populationSize; i++)
{
    population.Add(new SimpleNetwork([3, 32,32,32, 1]));}

for (int gen = 0; gen < generations; gen++)
{
    var results = population
        .Select(net => new { Network = net, Fitness = EvolutionManager.Evaluate(net) })
        .OrderByDescending(x => x.Fitness)
        .ToList();

    Console.WriteLine($"Gen {gen}: Beste Fitness = {results[0].Fitness:F0}");

    var champion = results[0].Network;

    List<SimpleNetwork> nextGen = new List<SimpleNetwork>();
    nextGen.Add(champion);

    while (nextGen.Count < populationSize)
    {
        SimpleNetwork child = champion.Clone();
        child.Mutate(0.1);
        nextGen.Add(child);
    }

    population = nextGen;
}

Console.WriteLine("Training beendet. Drücke eine Taste zum Testen des Champions...");
Console.ReadKey();

Console.WriteLine("Training beendet. Drücke eine Taste, um den Champion spielen zu sehen...");
Console.ReadKey();

PlayVisual(population.OrderByDescending(n => EvolutionManager.Evaluate(n)).First());

Console.WriteLine("Drücke eine Taste, um das Spiel zu beenden...");
Console.ReadKey();
return;

void PlayVisual(SimpleNetwork net)
{
    var game = new JumperGame();
    Console.CursorVisible = false;

    while (!game.IsDead)
    {
        Console.Clear();

        var output = net.Predict([game.ObstacleX, game.PlayerY, game.Speed / 0.5]);        bool wantToJump = output[0] > 0.5;
        Console.WriteLine($"\nScore: {game.Score} | Input-Wert: {output[0]:F4} | Drückt: {(wantToJump ? "JUMP!" : "     ")}");
        game.Update(wantToJump);

        var line = new string('_', 25).ToCharArray();

        int obsPos = (int)(game.ObstacleX * 20) + 2; 
        if (obsPos >= 0 && obsPos < line.Length) line[obsPos] = '#';

        if (game.PlayerY > 0.1)
        {
            Console.WriteLine("\n  O");
            Console.WriteLine("  " + new string(line));
        }
        else
        {
            line[2] = 'O';
            Console.WriteLine("\n"); 
            Console.WriteLine("  " + new string(line));
        }

        Console.WriteLine($"\nScore: {game.Score} | Speed: {game.Speed:F3}");
        
        Thread.Sleep(30);
    }

    Console.SetCursorPosition(0, 5);
    Console.WriteLine("GAME OVER! Die KI ist gegen ein Hindernis gekracht.");
    Console.CursorVisible = true;
}
