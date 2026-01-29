
namespace Jumper;


public class JumperGame
{
    public double ObstacleX { get; private set; } = 1.0;
    public double PlayerY { get; private set; } = 0.0;
    // HIER: Die fehlende Speed-Eigenschaft hinzufügen
    public double Speed { get; set; } = 0.03; 
    public bool IsDead { get; private set; } = false;
    public int Score { get; private set; } = 0;

    private double _jumpVelocity = 0.0;
    private Random _rng = new Random();

    public void Update(bool wantToJump)
    {
        if (IsDead) return;

        // Sprung-Physik
        if (wantToJump && PlayerY <= 0.01) _jumpVelocity = 0.12;
        
        PlayerY += _jumpVelocity;
        _jumpVelocity -= 0.008;

        if (PlayerY < 0) { PlayerY = 0; _jumpVelocity = 0; }

        // Benutze die variable Geschwindigkeit beim Bewegen
        ObstacleX -= Speed;

        // Kollision
        if (ObstacleX < 0.1 && ObstacleX > 0.0 && PlayerY < 0.2) IsDead = true;

        // Reset & Speed-Variation
        if (ObstacleX < -0.1) 
        { 
            ObstacleX = 1.0; 
            Score++;
            // Hier wird der Speed für das nächste Hindernis zufällig gesetzt
            Speed = 0.02 + (_rng.NextDouble() * 0.03);
        }
    }
}