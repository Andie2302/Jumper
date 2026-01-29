namespace Jumper;

public class EvolutionManager
{
    public static double Evaluate(SimpleNetwork net)
    {
        var game = new JumperGame();
        var frames = 0;

        while (!game.IsDead && frames < 2000)
        {
            var output = net.Predict([game.ObstacleX, game.PlayerY, game.Speed]);
            game.Update(output[0] > 0.5);
            frames++;
        }

        return game.Score * 100 + frames;
    }
}
