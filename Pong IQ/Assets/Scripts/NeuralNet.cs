using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;

public class NeuralNet
{
    int inputSize, hiddenSize, outputSize;
    float learningRate = 0.1f;
    LayerWeights L1, L2;
    Layer input, hidden, output;

    public NeuralNet(int inputSize, int outputSize)
    {
        this.inputSize = inputSize;
        this.hiddenSize = inputSize / outputSize;
        this.outputSize = outputSize;

        this.L1 = new LayerWeights(inputSize, hiddenSize);
        this.L2 = new LayerWeights(hiddenSize, outputSize);

        this.input = new Layer(inputSize);
        this.hidden = new Layer(hiddenSize);
        this.output = new Layer(outputSize);
    }

    public float predict(Vector<float> x)
    {
        return 0f;
    }

    public void train(float target)
    {

    }

    private void backProp(float target)
    {

    }
}
