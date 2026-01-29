
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

        // 1. Sprung-Logik
        if (wantToJump && PlayerY <= 0.01) _jumpVelocity = 0.8;
    
        PlayerY += _jumpVelocity;
        _jumpVelocity -= 0.12;

        if (PlayerY < 0) { PlayerY = 0; _jumpVelocity = 0; }

        // 2. Bewegung und "Tunneling"-Check
        double oldX = ObstacleX;
        ObstacleX -= Speed;

        // Wir prüfen, ob der Block in diesem Frame von rechts nach links 
        // die Position des Spielers (ca. 0.05) passiert hat.
        bool passedPlayer = oldX >= 0.05 && ObstacleX <= 0.05;

        if (passedPlayer && PlayerY < 0.2) 
        {
            IsDead = true; // Jetzt erwischt ihn der Block garantiert!
        }
        else if (ObstacleX < -0.1) 
        {
            // Reset des Blocks
            ObstacleX = 1.0; 
            Score++;
            // Geschwindigkeit neu würfeln
            Speed = 0.01 + (_rng.NextDouble() * 0.5);
        }
    }
}