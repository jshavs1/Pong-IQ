using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

public class Softmax : IActivation
{
    public Vector<double> activation(Vector<double> input)
    {
        Vector<double> result = input / input[input.Count];
        return result.SubVector(0, input.Count - 1);
    }

    public Vector<double> derivative(Vector<double> input)
    {
        return null;
    }
}
