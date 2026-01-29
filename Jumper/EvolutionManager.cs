namespace Jumper;

public class EvolutionManager
{
    public static double Evaluate(SimpleNetwork net)
    {
        var game = new JumperGame();
        var frames = 0;
        
        // Die KI darf max. 2000 Frames spielen
        while (!game.IsDead && frames < 2000)
        {
            var output = net.Predict([game.ObstacleX, game.PlayerY]);
            game.Update(output[0] > 0.5);
            frames++;
        }
        
        // Fitness = Score + wie lange sie überlebt hat
        return game.Score * 100 + frames;
    }
}