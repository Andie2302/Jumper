namespace SimpleNeuronTest.basic;

public class Layer
{
    public Neuron[] Neurons { get; }
    private readonly IActivationFunction _activation;

    public Layer(int neuronCount, int inputCountPerNeuron, IActivationFunction activation, Random rng)
    {
        _activation = activation;
        Neurons = new Neuron[neuronCount];
        for (var i = 0; i < neuronCount; i++)
            Neurons[i] = new Neuron(inputCountPerNeuron, rng);
    }

    public double[] Forward(double[] inputs)
    {
        var outputs = new double[Neurons.Length];
        for (var i = 0; i < Neurons.Length; i++)
        {
            var sum = Neurons[i].CalculateSum(inputs);
            outputs[i] = _activation.Activate(sum);
            Neurons[i].LastOutput = outputs[i];
        }
        return outputs;
    }

    public double GetDerivative(double output) => _activation.Derivative(output);
}