using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;

public interface IActivation
{
    Vector<double> activation(Vector<double> input);
    Vector<double> derivative(Vector<double> input);
}
