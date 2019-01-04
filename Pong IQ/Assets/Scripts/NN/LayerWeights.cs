using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MathNet.Numerics.LinearAlgebra;

public class LayerWeights
{
    public Matrix<double> weights;
    public static double learningRate = 0.1;
    Vector<double> bias;
    int rows, columns;

    public LayerWeights(int rows, int columns)
    {
        weights = CreateMatrix.Dense<double>(rows, columns);
        Func<double, double> randomize = x => (double)UnityEngine.Random.value;
        weights.MapInplace(randomize, Zeros.Include);

        bias = CreateVector.Dense<double>(columns);
        bias.MapInplace(randomize, Zeros.Include);

        this.rows = rows;
        this.columns = columns;
    }

    public void adjustWeights(Matrix<double> d, Vector<double> e)
    {
        this.bias = bias - e * learningRate;
        this.weights = weights - d * learningRate;
    }

    public Vector<double> multiply(Vector<double> input)
    {
        return (weights.Transpose() * input) + bias;
    }
}
