using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using System;

public class NeuralNet
{
    int inputSize, hiddenSize, outputSize;
    LayerWeights L1, L2;
    InputLayer inputLayer, hiddenLayer, outputLayer, softmaxLayer;

    public NeuralNet(int inputSize, int hiddenSize, int outputSize)
    {
        this.inputSize = inputSize;
        this.hiddenSize = hiddenSize;
        this.outputSize = outputSize;

        this.L1 = new LayerWeights(inputSize, hiddenSize);
        this.L2 = new LayerWeights(hiddenSize, outputSize);
    }

    public double[] predict(double[] x)
    {
        Matrix<double> m = CreateMatrix.Dense<double>(1, inputSize, x);
        return this.predict(m).Row(0).ToArray();
    }

    public Matrix<double> predict(Matrix<double> X)
    {
        return softmax(weightedActivation(activate(weightedInput(X))));
    }

    public Matrix<double> weightedInput(Matrix<double> X)
    {
        return X * L1.weights;
    }

    public Matrix<double> activate(Matrix<double> weighted)
    {
        return weighted.PointwiseTanh();
    }

    public Matrix<double> weightedActivation(Matrix<double> activation)
    {
        return activation * L2.weights;
    }

    public Matrix<double> softmax(Matrix<double> weightedActivation)
    {
        Matrix<double> result = CreateMatrix.Dense<double>(weightedActivation.RowCount, weightedActivation.ColumnCount);
        foreach (MathNet.Numerics.Tuple<int, Vector<double>> row in weightedActivation.PointwiseExp().EnumerateRowsIndexed())
        {
            result.SetRow(row.Item1, row.Item2 / row.Item2.Sum());
        }

        return result;
    }

    public void train(Matrix<double> X, Matrix<double> Y)
    {
        int passes = 10000;
        for (int i = 0; i < passes; i++)
        {
            Matrix<double> y = predict(X);
            //Debug.Log(y);
            Matrix<double> z = weightedInput(X);
            Matrix<double> a = activate(z);
            //Debug.Log(a);

            Matrix<double> e2 = y - Y;
            Matrix<double> L2 = a.Transpose() * e2;

            //Debug.Log(e2);

            Matrix<double> e1 = (1 - z.PointwiseTanh().PointwisePower(2.0)).PointwiseMultiply(e2 * this.L2.weights.Transpose());
            Matrix<double> L1 = X.Transpose() * e1;

            //Debug.Log(e1);

            this.L1.adjustWeights(L1, e1.ColumnSums() / e1.RowCount);
            this.L2.adjustWeights(L2, e2.ColumnSums() / e1.RowCount);

            //Debug.Log(this.L1.weights);
            if (i % 1000 == 0)
            {
                Debug.Log("Pass: " + i + ", Loss: " + loss(X, Y));
            }
            
        }
        Debug.Log(this.ToString());
    }

    private double loss(Matrix<double> X, Matrix<double> Y)
    {
        Matrix<double> y = predict(X);
        Vector<double> yv = CreateVector.Dense<double>(X.RowCount);

        foreach (MathNet.Numerics.Tuple<int, Vector<double>> row in y.EnumerateRowsIndexed())
        {
            yv[row.Item1] = Y.Row(row.Item1)[0] == 1.0 ? y.Row(row.Item1)[0] : y.Row(row.Item1)[1];  
        }

        Vector<double> correctLogs = -1.0 * yv.PointwiseLog();
        double loss = correctLogs.Sum();
        return (1.0 / (double)X.RowCount) * loss;
    }

    public override string ToString()
    {
        return L1.weights.ToString() + "\n" + L2.weights.ToString();
    }
}
