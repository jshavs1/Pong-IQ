using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using System;

public class NeuralNet
{
    int inputSize, hiddenSize, outputSize;
    LayerWeights L1, L2;
    InputLayer inputLayer, hiddenLayer, outputLayer, softmaxLayer;

    public NeuralNet(int inputSize, int hiddenSize, int outputSize)
    {
        this.inputSize = inputSize;
        this.hiddenSize = hiddenSize;
        this.outputSize = outputSize;

        this.L1 = new LayerWeights(inputSize, hiddenSize);
        this.L2 = new LayerWeights(hiddenSize, outputSize);
    }

    public double[] predict(double[] x)
    {
        inputLayer = new InputLayer(x.Length, x);
        Vector<double> o1 = L1.multiply(inputLayer.input);

        hiddenLayer = new InputLayer(o1.Count, o1);
        Vector<double> a1 = hiddenLayer.L1Activation;

        Vector<double> o2 = L2.multiply(a1);
        outputLayer = new InputLayer(o2.Count, o2);

        // Softmax
        Vector<double> s = CreateVector.Dense<double>(o2.Count + 1);
        s.SetSubVector(0, o2.Count, o2);
        s[o2.Count] = o2.Sum();
        softmaxLayer = new InputLayer(o2.Count + 1, s);

        outputLayer = softmaxLayer;

        return outputLayer.L2Activation.ToArray();
    }

    public void train(double[] target)
    {
        Vector<double> t = CreateVector.Dense<double>(target);
        Vector<double> a = outputLayer.L2Activation;

        Vector<double> e2 = (a - t);
        Matrix<double> d2 = e2.ToColumnMatrix() * hiddenLayer.L1Activation.ToRowMatrix();


        Vector<double> e1 = hiddenLayer.L1Derivative.PointwiseMultiply(L2.weights.Transpose() * e2);
        Matrix<double> d1 = e1.ToColumnMatrix() * inputLayer.input.ToRowMatrix();

        //Debug.Log(d1);

        L2.adjustWeights(d2, e2);
        L1.adjustWeights(d1, e1);

        //Debug.Log("Target: " + t[0] + "\nOutput: " + a[0]);
    }

    public override string ToString()
    {
        return L1.weights.ToString();// + "\n" + L2.weights.ToString();
    }
}
