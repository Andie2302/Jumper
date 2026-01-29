
namespace Jumper;


public class JumperGame
{
    public double ObstacleX { get; private set; } = 1.0;
    public double PlayerY { get; private set; } = 0.0;
    public double Speed { get; set; } = 0.03; 
    public bool IsDead { get; private set; } = false;
    public int Score { get; private set; } = 0;

    private double _jumpVelocity = 0.0;
    private Random _rng = new Random();

    public void Update(bool wantToJump)
    {
        if (IsDead) return;

        // Sprung-Logik
        if (wantToJump && PlayerY <= 0.01) _jumpVelocity = 0.8;
    
        PlayerY += _jumpVelocity;
        _jumpVelocity -= 0.12;

        if (PlayerY < 0) { PlayerY = 0; _jumpVelocity = 0; }

        // Kollision VOR dem Bewegen berechnen
        double oldX = ObstacleX;
        ObstacleX -= Speed;

        // Prüfen, ob der Block den kritischen Bereich (0.1 bis 0.0) gekreuzt hat
        bool blockPassedPlayer = oldX >= 0.1 && ObstacleX <= 0.0;

        if (blockPassedPlayer && PlayerY < 0.2) 
        {
            IsDead = true;
        }
        else if (ObstacleX < -0.1) 
        {
            ObstacleX = 1.0; 
            Score++;
            // Deine neue Speed-Range
            Speed = 0.01 + (_rng.NextDouble() * 0.09); 
        }
    }
}