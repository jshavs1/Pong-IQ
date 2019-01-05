using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;

public class InputLayer
{
    public readonly Vector<double> input;
    public static IActivation L1ActivationFunction = new Tanh(), L2ActivationFunction = new Softmax();
    readonly double[] rawInput;
    readonly int size;

    public InputLayer(int size)
    {
        this.size = size;
        input = CreateVector.Dense<double>(size, 0.0);
    }

    public InputLayer(int size, double[] input)
    {
        if (size != input.Length) { Debug.LogError("Layer: Input size and size parameter do not match"); }

        this.size = size;
        this.rawInput = input;
        this.input = CreateVector.Dense<double>(input);
    }
    public InputLayer(int size, Vector<double> input)
    {
        this.size = size;
        this.input = input;
        this.rawInput = input.AsArray();
    }

    public void Set(int index, double value)
    {
        input[index] = value;
    }

    public double Get(int index)
    {
        return input[index];
    }

    public Vector<double> L1Activation
    {
        get
        {
            return L1ActivationFunction.activation(input);
        }
    }

    public Vector<double> L1Derivative
    {
        get
        {
            return L1ActivationFunction.derivative(input);
        }
    }

    public Vector<double> L2Activation
    {
        get
        {
            return L2ActivationFunction.activation(input);
        }
    }

    public Vector<double> L2Derivative
    {
        get
        {
            return L2ActivationFunction.derivative(input);
        }
    }
}
