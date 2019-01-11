using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MathNet.Numerics.LinearAlgebra;

public class PongIq : MonoBehaviour
{
    public int inputNodes, hiddenNodes, outputNodes, trainingSize, remainingIterations;
    public Cannon cannon;
    public bool[] remainingCups;
    private NeuralNet nn;
    private bool training;
    public float maxForce = 5f;
    public float minForce = 3f;
    public float maxRotation = 50f;
    private float force, rotation;
    private double lastValue = 0.0;

    private Matrix<double> X, Y;
    private double[] lastInput;
    private double scalar = 1.0;

    // Start is called before the first frame update
    void Start()
    {
        nn = new NeuralNet(inputNodes, hiddenNodes, outputNodes);
        X = CreateMatrix.Dense<double>(trainingSize, inputNodes);
        Y = CreateMatrix.Dense<double>(trainingSize, outputNodes);

        remainingCups = new bool[10];
        remainingIterations = trainingSize;
        training = true;
        Train();
        Time.timeScale = 10.0f;

        Debug.Log(nn.ToString());
    }

    private void Update()
    {
        if (training) { return; }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(TrainedShot());
        }
    }

    void Train()
    {
        lastInput = GetInput();

        StartCoroutine(cannon.Shoot((float)(lastInput[0] / scalar), (float)(lastInput[1] / scalar), maxForce, minForce, maxRotation));
        //remainingIterations--;
    }

    void TrainingFinished()
    {
        Time.timeScale = 1.0f;
        Debug.Log(Y);
        nn.train(X, Y);
        training = false;
        Debug.Log("TrainingFinished");
    }

    IEnumerator TrainedShot()
    {
        double[] input = null;
        double[] results = new double[outputNodes];
        while(results[0] < 0.5)
        {
            yield return null;
            input = GetInput();
            results = nn.predict(input);

            Debug.Log(results[0] + ", " + results[1]);
        }
        StartCoroutine(cannon.Shoot((float)(lastInput[0] / scalar), (float)(lastInput[1] / scalar), maxForce, minForce, maxRotation));
    }

    double[] GetInput()
    {
        double[] d = new double[inputNodes];
        force = UnityEngine.Random.value;
        rotation = UnityEngine.Random.value;

        d[0] = (double) force * scalar;
        d[1] = (double) rotation * scalar;
        d[2] = (double) Math.Sqrt(Math.Pow(d[0] - Mathf.Lerp(minForce, maxForce, 0.5f), 2.0) + Math.Pow(d[1] - Mathf.Lerp(-maxRotation, maxRotation, 0.5f), 2.0));

        /*
        for (int i = 0; i < remainingCups.Length; i++)
        {
            d[i + 2] = remainingCups[i] ? 0.0 : 1.0;
        }
        */
        return d;
    }

    public void CollisionDetected(int index, double value)
    {
        double[] result = new double[] { (value == 1.0 ? 1.0 : 0.0), (value == 1.0 ? 0.0 : 1.0) };
        if (training)
        {
            if (value == lastValue) { Train(); return; }
            lastValue = value;
            remainingIterations--;

            X.SetRow(remainingIterations, lastInput);
            Y.SetRow(remainingIterations, result);

            if (remainingIterations == 0)
            {
                TrainingFinished();
            }
            if (remainingIterations > -1)
            {
                Train();
            }
            else if (index != -1) { remainingCups[index] = true; }
        }
    }
}
