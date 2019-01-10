using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

public class Softmax : IActivation
{
    public Vector<double> activation(Vector<double> input)
    {
        return input / input.Sum();
    }

    public Vector<double> derivative(Vector<double> input)
    {
        return CreateVector.Dense<double>(input.Count, 1.0);
    }
}
