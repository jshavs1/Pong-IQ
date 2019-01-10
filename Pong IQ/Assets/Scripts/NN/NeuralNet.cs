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
        /*
        inputLayer = new InputLayer(x.Length, x);
        Vector<double> o1 = L1.multiply(inputLayer.input);

        hiddenLayer = new InputLayer(o1.Count, o1);
        Vector<double> a1 = hiddenLayer.L1Activation;
        //Debug.Log(o1);
        //Debug.Log(a1);

        Vector<double> o2 = L2.multiply(a1);
        outputLayer = new InputLayer(o2.Count, o2);

        return outputLayer.L2Activation.ToArray();
        */

        Matrix<double> m = CreateMatrix.Dense<double>(1, inputSize, x);
        return this.predict(m).Row(0).ToArray();
    }

    public Matrix<double> predict(Matrix<double> X)
    {

        return softmax(weightedActivation(activate(weightedInput(X))));
        /*
        Matrix<double> y = CreateMatrix.Dense<double>(X.RowCount, outputSize);
        for (int i = 0; i < X.RowCount; i++)
        {
            double[] yhat = predict(X.Row(i).ToArray());
            y.SetRow(i, yhat);
        }
        return y;
        */
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
        int passes = 1000;
        for (int i = 0; i < passes; i++)
        {
            Matrix<double> y = predict(X);
            //Debug.Log(y);
            //Debug.Log(y);
            Matrix<double> z = weightedInput(X);
            Matrix<double> a = activate(z);

            Matrix<double> e2 = y - Y;
            Matrix<double> L2 = a.Transpose() * e2;

            //Debug.Log(e2);

            Matrix<double> e1 = (1 - z.PointwiseTanh().PointwisePower(2.0)).PointwiseMultiply(e2 * this.L2.weights.Transpose());
            Matrix<double> L1 = X.Transpose() * e1;

            //Debug.Log(e1);

            this.L1.adjustWeights(L1, e1.ColumnSums());
            this.L2.adjustWeights(L2, e2.ColumnSums());

            //Debug.Log(this.L1.weights);
            //Debug.Log("Pass: " + i);
        }

        Debug.Log(this.ToString());

        /*
        Vector<double> t = CreateVector.Dense<double>(target);
        Vector<double> a = outputLayer.L2Activation;
        
        Vector<double> e2 = (a - t) / a.Count;
        Matrix<double> d2 = hiddenLayer.L1Activation.ToColumnMatrix() * e2.ToRowMatrix();


        Matrix<double> e1 = hiddenLayer.L1Derivative.ToColumnMatrix().PointwiseMultiply((e2.ToRowMatrix() * L2.weights).Transpose());
        Matrix<double> d1 = inputLayer.input.ToColumnMatrix() * e1.Transpose();

        L2.adjustWeights(d2.Transpose(), e2);
        L1.adjustWeights(d1.Transpose(), e1.Column(0));
        */
        //Debug.Log("Target: " + t[0] + ", " + t[1] + "\nOutput: " + a[0] + ", " + a[1] + "\nbias: " + L1.bias + ", " + L2.bias);
    }

    public override string ToString()
    {
        return L1.weights.ToString() + "\n" + L2.weights.ToString();
    }
}
