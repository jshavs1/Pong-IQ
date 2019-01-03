using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;

public class Layer
{
    readonly Vector<double> input;
    readonly double[] rawInput;
    readonly int size;

    public Layer(int size)
    {
        this.size = size;
        input = CreateVector.Dense<double>(size, 0.0);
    }

    public Layer(int size, double[] input)
    {
        if (size != input.Length) { Debug.LogError("Layer: Input size and size parameter do not match"); }

        this.size = size;
        this.rawInput = input;
        this.input = CreateVector.Dense<double>(input);
    }

    public void Set(int index, double value)
    {
        input[index] = value;
    }

    public double Get(int index)
    {
        return input[index];
    }
}
