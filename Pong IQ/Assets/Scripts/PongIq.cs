using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PongIq : MonoBehaviour
{
    public int inputNodes, hiddenNodes, outputNodes, trainingIterations;
    public Cannon cannon;
    public bool[] remainingCups;
    public int remainingIterations;
    private NeuralNet nn;
    private bool training;

    private float force, rotation;

    // Start is called before the first frame update
    void Start()
    {
        nn = new NeuralNet(inputNodes, hiddenNodes, outputNodes);
        remainingCups = new bool[10];
        remainingIterations = trainingIterations;
        Train();
        Time.timeScale = 10.0f;
        Debug.Log(nn.ToString());
    }

    private void Update()
    {
        if (remainingIterations > 0) { return; }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(TrainedShot());
        }
    }

    void Train()
    {
        if (training) { return; }
        training = true;
        double[] input = GetInput();

        nn.predict(input);
        cannon.Shoot((float)input[0], (float)input[1]);

        remainingIterations--;

        if (remainingIterations == 0)
        {
            TrainingFinished();
        }
    }

    void TrainingFinished()
    {
        Time.timeScale = 1.0f;
        Debug.Log(nn.ToString());
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
            Debug.Log(results);
        }

        cannon.Shoot((float)input[0], (float)input[1]);
    }

    double[] GetInput()
    {
        double[] d = new double[inputNodes];
        force = UnityEngine.Random.value;
        rotation = UnityEngine.Random.value;

        d[0] = (double) force;
        d[1] = (double) rotation;

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
        training = false;
        nn.train(new double[] { value, 1.0 - value });
        if (remainingIterations > 0)
        {
            Train();
        }
        else
        {
            if (index != -1) { remainingCups[index] = true; }
        }
    }
}
