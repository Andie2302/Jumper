namespace SimpleNeuronTest.basic;

public class Neuron
{
    public double[] Weights { get; }
    public double Bias { get; set; }
    public double LastOutput { get; set; }

    public Neuron(int inputCount, Random rng)
    {
        Weights = new double[inputCount];
        for (var i = 0; i < inputCount; i++)
            Weights[i] = rng.NextDouble() * 2 - 1;
        Bias = rng.NextDouble() * 2 - 1;
    }

    public double CalculateSum(double[] inputs)
    {
        var sum = Bias;
        for (var i = 0; i < Weights.Length; i++)
            sum += inputs[i] * Weights[i];
        return sum;
    }

    public void UpdateWeights(double delta, double learningRate, double[] inputs)
    {
        for (var i = 0; i < Weights.Length; i++)
            Weights[i] += learningRate * delta * inputs[i];
        Bias += learningRate * delta;
    }
}