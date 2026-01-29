using Jumper;


Console.WriteLine("--- KI Jumper Training ---");

int populationSize = 25;
int generations = 100;

// 1. Initial-Population erstellen
List<SimpleNetwork> population = new List<SimpleNetwork>();
for (int i = 0; i < populationSize; i++)
{
    // Topology: 2 Inputs (ObstacleX, PlayerY), 16 Hidden, 1 Output (Jump)
    population.Add(new SimpleNetwork([2, 32,32,32, 1]));
}

for (int gen = 0; gen < generations; gen++)
{
    // 2. Alle bewerten (Fitness berechnen)
    var results = population
        .Select(net => new { Network = net, Fitness = EvolutionManager.Evaluate(net) })
        .OrderByDescending(x => x.Fitness)
        .ToList();

    Console.WriteLine($"Gen {gen}: Beste Fitness = {results[0].Fitness:F0}");

    // 3. Den Champion auswählen
    SimpleNetwork champion = results[0].Network;

    // 4. Nächste Generation züchten
    List<SimpleNetwork> nextGen = new List<SimpleNetwork>();
    nextGen.Add(champion); // Der Champion kommt direkt weiter (Elitismus)

    while (nextGen.Count < populationSize)
    {
        // Kinder vom Champion erstellen
        SimpleNetwork child = champion.Clone();
        child.Mutate(0.1); // 10% Mutationsrate
        nextGen.Add(child);
    }

    population = nextGen;
}

Console.WriteLine("Training beendet. Drücke eine Taste zum Testen des Champions...");
Console.ReadKey();

// ... (Dein Training-Code von vorhin)

Console.WriteLine("Training beendet. Drücke eine Taste, um den Champion spielen zu sehen...");
Console.ReadKey();

// Den besten Champion aus der letzten Generation visualisieren
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
        
        // KI Entscheidung (Jetzt mit 3 Eingängen für Speed)
        var output = net.Predict([game.ObstacleX, game.PlayerY, game.Speed]);
        bool wantToJump = output[0] > 0.5;
        
        game.Update(wantToJump); //

        // Zeichnen der "Welt" (20 Zeichen breit)
        char[] line = new string('_', 25).ToCharArray();
        
        // Hindernis zeichnen (#)
        int obsPos = (int)(game.ObstacleX * 20) + 2; 
        if (obsPos >= 0 && obsPos < line.Length) line[obsPos] = '#';

        // Spieler zeichnen (O)
        // Wenn PlayerY > 0, zeichnen wir ihn eine Zeile höher
        if (game.PlayerY > 0.1)
        {
            Console.WriteLine("\n  O"); // Luft
            Console.WriteLine("  " + new string(line)); // Boden
        }
        else
        {
            line[2] = 'O'; // Spieler am Boden
            Console.WriteLine("\n"); 
            Console.WriteLine("  " + new string(line));
        }

        Console.WriteLine($"\nScore: {game.Score} | Speed: {game.Speed:F3}");
        
        Thread.Sleep(30); // Geschwindigkeit drosseln, damit wir zusehen können
    }

    Console.SetCursorPosition(0, 5);
    Console.WriteLine("GAME OVER! Die KI ist gegen ein Hindernis gekracht.");
    Console.CursorVisible = true;
}
