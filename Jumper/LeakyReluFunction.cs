namespace Jumper;

public class LeakyReluFunction : IActivationFunction
{
    public double Activate(double x) => x > 0 ? x : x * 0.01;
    public double Derivative(double output) => output > 0 ? 1 : 0.01;
}