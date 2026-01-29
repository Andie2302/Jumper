using SimpleNeuronTest.basic;

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
            // Architektur: Letzter Layer = Sigmoid, Hidden Layers = ReLU
            IActivationFunction func = (i == _layers.Length - 1) 
                ? new SigmoidFunction() 
                : new ReluFunction();
            
            _layers[i] = new Layer(topology[i + 1], topology[i], func, rng);
        }
    }

    // Diese Methode hat im Build gefehlt:
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
        // 1. Forward Pass (notwendig, um LastOutput in den Neuronen zu setzen)
        var actual = Predict(inputs);

        // 2. Backward Pass
        double[] currentErrorSignals = new double[targets.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            currentErrorSignals[i] = targets[i] - actual[i];
        }

        for (int i = _layers.Length - 1; i >= 0; i--)
        {
            var layer = _layers[i];
            double[] nextErrorSignals = new double[layer.Neurons[0].Weights.Length];
            double[] deltas = new double[layer.Neurons.Length];

            // Wir brauchen die Inputs für diesen Layer (entweder Netzwerk-Inputs oder Output vom Vorlayer)
            double[] layerInputs = (i == 0) ? inputs : _layers[i - 1].Neurons.Select(n => n.LastOutput).ToArray();

            for (int j = 0; j < layer.Neurons.Length; j++)
            {
                var neuron = layer.Neurons[j];
                double derivative = layer.GetDerivative(neuron.LastOutput);
                deltas[j] = currentErrorSignals[j] * derivative;

                for (int k = 0; k < neuron.Weights.Length; k++)
                {
                    nextErrorSignals[k] += neuron.Weights[k] * deltas[j];
                }
            }

            for (int j = 0; j < layer.Neurons.Length; j++)
            {
                layer.Neurons[j].UpdateWeights(deltas[j], learningRate, layerInputs);
            }

            currentErrorSignals = nextErrorSignals;
        }
    }
    
    public SimpleNetwork Clone()
    {
        // Wir nutzen die Topology des aktuellen Netzes
        int[] topology = new int[_layers.Length + 1];
    
        // Eingänge des ersten Layers
        topology[0] = _layers[0].Neurons[0].Weights.Length;
    
        // Ausgänge jedes Layers
        for (int i = 0; i < _layers.Length; i++)
        {
            topology[i + 1] = _layers[i].Neurons.Length;
        }

        var clone = new SimpleNetwork(topology);

        // Jetzt kopieren wir die exakten Gewichte und Biases
        for (int l = 0; l < _layers.Length; l++)
        {
            for (int n = 0; n < _layers[l].Neurons.Length; n++)
            {
                var originalNeuron = _layers[l].Neurons[n];
                var cloneNeuron = clone._layers[l].Neurons[n];

                Array.Copy(originalNeuron.Weights, cloneNeuron.Weights, originalNeuron.Weights.Length);
                cloneNeuron.Bias = originalNeuron.Bias;
            }
        }

        return clone;
    }
    
}