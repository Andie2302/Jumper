namespace Jumper;

public class ReluFunction : IActivationFunction
{
    public double Activate(double x) => Math.Max(0, x);
    public double Derivative(double output) => output > 0 ? 1 : 0;
}