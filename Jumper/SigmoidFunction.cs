public class SigmoidFunction : IActivationFunction
{
    public double Activate(double x) => 1.0 / (1.0 + Math.Exp(-x));
    public double Derivative(double output) => output * (1.0 - output);
}