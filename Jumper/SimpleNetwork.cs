using SimpleNeuronTest.basic;

namespace Jumper;

public class SimpleNetwork
{
    private readonly Layer[] _layers;
    public IReadOnlyList<Layer> Layers => _layers;

    public SimpleNetwork(int[] topology, int seed = 42)
    {
        var rng = new Random(seed);
        _layers = new Layer[topology.Length - 1];

        for (var i = 0; i < _layers.Length; i++)
        {
            IActivationFunction func = (i == _layers.Length - 1) 
                ? new SigmoidFunction() 
                : new ReluFunction();
            
            _layers[i] = new Layer(topology[i + 1], topology[i], func, rng);
        }
    }

    public double[] Predict(double[] inputs)
    {
        var currentSignals = inputs;
        foreach (var layer in _layers)
        {
            currentSignals = layer.Forward(currentSignals);
        }
        return currentSignals;
    }

    public void Train(double[] inputs, double[] targets, double learningRate)
    {
        var actual = Predict(inputs);

        var currentErrorSignals = new double[targets.Length];
        for (var i = 0; i < targets.Length; i++)
        {
            currentErrorSignals[i] = targets[i] - actual[i];
        }

        for (var i = _layers.Length - 1; i >= 0; i--)
        {
            var layer = _layers[i];
            var nextErrorSignals = new double[layer.Neurons[0].Weights.Length];
            var deltas = new double[layer.Neurons.Length];

            var layerInputs = (i == 0) ? inputs : _layers[i - 1].Neurons.Select(n => n.LastOutput).ToArray();

            for (var j = 0; j < layer.Neurons.Length; j++)
            {
                var neuron = layer.Neurons[j];
                var derivative = layer.GetDerivative(neuron.LastOutput);
                deltas[j] = currentErrorSignals[j] * derivative;

                for (var k = 0; k < neuron.Weights.Length; k++)
                {
                    nextErrorSignals[k] += neuron.Weights[k] * deltas[j];
                }
            }

            for (var j = 0; j < layer.Neurons.Length; j++)
            {
                layer.Neurons[j].UpdateWeights(deltas[j], learningRate, layerInputs);
            }

            currentErrorSignals = nextErrorSignals;
        }
    }
    
    public SimpleNetwork Clone()
    {
        var topology = new int[_layers.Length + 1];

        topology[0] = _layers[0].Neurons[0].Weights.Length;

        for (var i = 0; i < _layers.Length; i++)
        {
            topology[i + 1] = _layers[i].Neurons.Length;
        }

        var clone = new SimpleNetwork(topology);

        for (var l = 0; l < _layers.Length; l++)
        {
            for (var n = 0; n < _layers[l].Neurons.Length; n++)
            {
                var originalNeuron = _layers[l].Neurons[n];
                var cloneNeuron = clone._layers[l].Neurons[n];

                Array.Copy(originalNeuron.Weights, cloneNeuron.Weights, originalNeuron.Weights.Length);
                cloneNeuron.Bias = originalNeuron.Bias;
            }
        }

        return clone;
    }
    
    
    public void Mutate(double rate, double amount = 0.1)
    {
        var rng = new Random();
        foreach (var layer in _layers)
        {
            foreach (var neuron in layer.Neurons)
            {
                for (var i = 0; i < neuron.Weights.Length; i++)
                {
                    if (rng.NextDouble() < rate)
                    {
                        neuron.Weights[i] += (rng.NextDouble() * 2 - 1) * amount;
                    }
                }

                if (rng.NextDouble() < rate)
                {
                    neuron.Bias += (rng.NextDouble() * 2 - 1) * amount;
                }
            }
        }
    }
    
}