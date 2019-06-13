using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Data.Text;

public class PongIq : MonoBehaviour
{
    [SerializeField]
    private State _state;
    private State state
    {
        get { return _state; }
        set
        {
            del?.StateChanged(_state, value);
            this._state = value;
        }
    }

    public IPongDelegate del;

    public int inputNodes, hiddenNodes, outputNodes, trainingSize, remainingIterations;
    public Cannon cannon;
    public bool[] remainingCups;
    private NeuralNet nn;
    private bool training;
    public float maxForce = 5f;
    public float minForce = 0f;
    public float maxRotation = 50f;
    private float force, rotation;
    private double lastValue = 0.0;

    private Matrix<double> X, Y;
    private double[] lastInput;
    private double scalar = 1.0;

    // Start is called before the first frame update
    void Start()
    {
        del = FindObjectOfType<GameManager>();
        nn = new NeuralNet(inputNodes, hiddenNodes, outputNodes);
        X = CreateMatrix.Dense<double>(trainingSize, inputNodes);
        Y = CreateMatrix.Dense<double>(trainingSize, outputNodes);

        remainingCups = new bool[10];
        remainingIterations = trainingSize;
        training = true;
        Train();
        Time.timeScale = 20.0f;

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
        state = State.Training;

        lastInput = GetInput();

        StartCoroutine(cannon.Shoot((float)(lastInput[0] / scalar), (float)(lastInput[1] / scalar), maxForce, minForce, maxRotation));
        //remainingIterations--;
    }

    void TrainingFinished()
    {
        Time.timeScale = 1.0f;
        Debug.Log(Y);
        nn.train(X, Y);
        DelimitedWriter.Write<double>("X.csv", X, ",");
        DelimitedWriter.Write<double>("Y.csv", Y, ",");
        DelimitedWriter.Write<double>("predict.csv", nn.predict(X), ",");
        training = false;
        Debug.Log("TrainingFinished");

        state = State.Idle;
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
        Debug.Log("Input: "  + input[0] + ", " + input[1]);
        del?.Shooting(results[0]);
        StartCoroutine(cannon.Shoot((float)(input[0] / scalar), (float)(input[1] / scalar), maxForce, minForce, maxRotation));
        state = State.Shooting;
    }

    double[] GetInput()
    {
        double[] d = new double[inputNodes];
        force = UnityEngine.Random.value;
        rotation = UnityEngine.Random.value;

        d[0] = (double) force * scalar;
        d[1] = (double) rotation * scalar;
        d[2] = (double) Math.Sqrt(Math.Pow(d[0] - normalize(Mathf.Lerp(minForce, maxForce, 0.5f), minForce, maxForce), 2.0) + Math.Pow(d[1] - normalize(Mathf.Lerp(-maxRotation, maxRotation, 0.5f), -maxRotation, maxRotation), 2.0));

        /*
        for (int i = 0; i < remainingCups.Length; i++)
        {
            d[i + 2] = remainingCups[i] ? 0.0 : 1.0;
        }
        */
        return d;
    }

    private double normalize(double x, double min, double max)
    {
        return (x - min) / (max - min);
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
        else
        {
            CollisionDetection[] colliders = GetComponentsInChildren<CollisionDetection>();
            CollisionDetection collider = null;
            foreach (CollisionDetection col in GetComponentsInChildren<CollisionDetection>())
            {
                if (col.index == index)
                    collider = col;
            }
            del.CollisionDetected(colliders, collider, index);
            state = State.Idle;
        }
    }
}

public enum State
{
    Training,
    Idle,
    Shooting,
}
