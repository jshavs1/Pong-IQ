using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;
using System;

public class Tanh : IActivation
{
    public Vector<double> activation(Vector<double> input)
    {
        return input.PointwiseTanh();
    }

    public Vector<double> derivative(Vector<double> input)
    {
        return 1.0 - this.activation(input).PointwisePower(2.0);
    }
}
