

using Jumper;

Console.WriteLine("Starte KI-Evolution für RunnerGame...");

// Initial-Population (1<<4 = 16 Neuronen im Hidden Layer)
var popSize = 20;
var population = Enumerable.Range(0, popSize)
    .Select(_ => new SimpleNetwork([2, 16, 1]))
    .ToList();

for (var gen = 0; gen < 100; gen++)
{
    // 1. Alle bewerten
    var results = population
        .Select(n => new { Net = n, Fitness = EvolutionManager.Evaluate(n) })
        .OrderByDescending(x => x.Fitness)
        .ToList();

    Console.WriteLine($"Generation {gen} | Beste Fitness: {results[0].Fitness}");

    // 2. Den Champion klonen und mutieren
    var champion = results[0].Net;
    population.Clear();
    population.Add(champion); // Der Beste bleibt erhalten

    for (var i = 1; i < popSize; i++)
    {
        var offspring = champion.Clone();
        offspring.Mutate(0.1); // 10% Mutationschance
        population.Add(offspring);
    }
}