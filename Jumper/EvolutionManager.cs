namespace Jumper;

public static class EvolutionManager
{

    public static double Evaluate(SimpleNetwork net)
    {
        double totalFitness = 0;
        int runs = 3; // 3 Testläufe für mehr Stabilität

        for (int i = 0; i < runs; i++)
        {
            var game = new JumperGame();
            var frames = 0;
            double airTimePenalty = 0;

            while (!game.IsDead && frames < 2000)
            {
                // PlayerY / 3.0 für bessere Skalierung hinzugefügt
                var output = net.Predict([game.ObstacleX, game.PlayerY / 3.0, game.Speed / 0.5]);
                bool wantToJump = output[0] > 0.5;

                if (game.PlayerY > 0.01 && game.ObstacleX > 0.6) airTimePenalty += 0.5; 

                game.Update(wantToJump);
                frames++;
            }
            totalFitness += (game.Score * 500) + frames - airTimePenalty;
        }

        return totalFitness / runs;
    }
    
}
