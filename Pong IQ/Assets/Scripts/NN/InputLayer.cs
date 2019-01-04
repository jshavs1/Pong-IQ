using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;

public class InputLayer
{
    public readonly Vector<double> input;
    public static IActivation activationFunction = new Tanh();
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

    public Vector<double> activation
    {
        get
        {
            return activationFunction.activation(input);
        }
    }

    public Vector<double> derivative
    {
        get
        {
            return activationFunction.derivative(input);
        }
    }
}
