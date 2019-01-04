using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;
using System;

public class Tanh : IActivation
{
    public Vector<double> activation(Vector<double> input)
    {
        Func<double, double> map = z => (Math.Exp(z) - Math.Exp(-z)) / (Math.Exp(z) + Math.Exp(-z));
        return input.Map<double>(map, Zeros.Include);
    }

    public Vector<double> derivative(Vector<double> input)
    {
        return 1 - this.activation(input).PointwisePower(2.0);
    }
}
