namespace Jumper;

public class EvolutionManager
{
    public static double Evaluate(SimpleNetwork net)
    {
        var game = new JumperGame();
        var frames = 0;
        double airTimePenalty = 0;

        while (!game.IsDead && frames < 2000)
        {
            var output = net.Predict([game.ObstacleX, game.PlayerY, game.Speed / 0.5]);
            bool wantToJump = output[0] > 0.5;

            // Bestrafung, wenn die KI springt, obwohl das Hindernis noch weit weg ist
            if (game.PlayerY > 0.01 && game.ObstacleX > 0.6) 
            {
                airTimePenalty += 0.5; 
            }

            game.Update(wantToJump);
            frames++;
        }

        // Die Strafe wird vom Gesamtwert abgezogen
        return (game.Score * 500) + frames - airTimePenalty;
    }
}
