using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MathNet.Numerics.LinearAlgebra;

public class LayerWeights
{
    Matrix<double> weights;
    int rows, columns;

    public LayerWeights(int rows, int columns)
    {
        weights = CreateMatrix.Dense<double>(rows, columns);
        Func<double, double> randomize = x => (double)UnityEngine.Random.value;
        weights.MapInplace(randomize, Zeros.Include);

        this.rows = rows;
        this.columns = columns;
    }

    public void adjustWeights(Matrix<double> d)
    {
        weights = weights - d;
    }
}
