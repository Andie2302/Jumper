
namespace Jumper;

public class JumperGame
{
    public double ObstacleX { get; private set; } = 1.0;
    public double PlayerY { get; private set; } = 0.0;
    public bool IsDead { get; private set; } = false;
    public int Score { get; private set; } = 0;

    private double _jumpVelocity = 0.0;

    public void Update(bool wantToJump)
    {
        if (IsDead) return;

        // Sprung-Physik
        if (wantToJump && PlayerY <= 0.01) _jumpVelocity = 0.12;
        
        PlayerY += _jumpVelocity;
        _jumpVelocity -= 0.008; // Schwerkraft

        if (PlayerY < 0) { PlayerY = 0; _jumpVelocity = 0; }

        // Hindernis-Bewegung
        ObstacleX -= 0.03;

        // Kollision
        if (ObstacleX < 0.1 && ObstacleX > 0.0 && PlayerY < 0.2) IsDead = true;

        if (ObstacleX < -0.1) { ObstacleX = 1.0; Score++; }
    }
}