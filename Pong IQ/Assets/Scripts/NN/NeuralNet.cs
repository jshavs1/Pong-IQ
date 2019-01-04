using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;

public class NeuralNet
{
    int inputSize, hiddenSize, outputSize;
    LayerWeights L1, L2;
    InputLayer inputLayer, hiddenLayer, outputLayer;

    public NeuralNet(int inputSize, int outputSize)
    {
        this.inputSize = inputSize;
        this.hiddenSize = inputSize / 2;
        this.outputSize = outputSize;

        this.L1 = new LayerWeights(inputSize, hiddenSize);
        this.L2 = new LayerWeights(hiddenSize, outputSize);
    }

    public double[] predict(double[] x)
    {
        inputLayer = new InputLayer(x.Length, x);
        Vector<double> o1 = L1.multiply(inputLayer.input);

        hiddenLayer = new InputLayer(o1.Count, o1);
        Vector<double> a1 = hiddenLayer.activation;

        Vector<double> o2 = L2.multiply(a1);
        outputLayer = new InputLayer(o2.Count, o2);

        return outputLayer.activation.ToArray();
    }

    public void train(double[] target)
    {
        Vector<double> t = CreateVector.Dense<double>(target);
        Vector<double> a = outputLayer.activation;

        Vector<double> e2 = (a - t).PointwiseMultiply(outputLayer.derivative);
        Matrix<double> d2 = L2.weights.Transpose();

        Debug.Log(d2);

        for (int i = 0; i < d2.ColumnCount; i++)
        {
            Vector<double> column = d2.Column(i);
            d2.SetColumn(i, column.PointwiseMultiply(e2));
        }

        Vector<double> e1 = hiddenLayer.derivative.PointwiseMultiply(e2 * L2.weights.Transpose());
        Matrix<double> d1 = inputLayer.input.ToColumnMatrix() * e1.ToRowMatrix();

        L2.adjustWeights(d2.Transpose(), e2);
        L1.adjustWeights(d1, e1);
    }
}
