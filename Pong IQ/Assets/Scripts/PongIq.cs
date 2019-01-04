using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PongIq : MonoBehaviour
{
    public int inputNodes, outputNodes, trainingIterations;
    public Cannon cannon;
    private bool[] remainingCups;
    private int remainingIterations;
    private NeuralNet nn;

    private float force, rotation;

    // Start is called before the first frame update
    void Start()
    {
        nn = new NeuralNet(inputNodes, outputNodes);
        remainingCups = new bool[10];
        remainingIterations = trainingIterations;
        Train();
    }

    void Train()
    {
        if (remainingIterations == 0) { Debug.Log("Training Finished."); }

        double[] input = GetInput();

        nn.predict(input);
        cannon.Shoot((float)input[0], (float)input[1]);
        
        remainingIterations--;
    }

    double[] GetInput()
    {
        double[] d = new double[inputNodes];
        force = UnityEngine.Random.value;
        rotation = UnityEngine.Random.value;

        d[0] = (double) force;
        d[1] = (double) rotation;
        for (int i = 0; i < remainingCups.Length; i++)
        {
            d[i + 2] = remainingCups[i] ? 0.0 : 1.0;
        }

        return d;
    }

    public void CollisionDetected(int index, double value)
    {
        nn.train(new double[] { value });
        if (value == 1.0 && index != -1) { remainingCups[index] = true; }
    }
}
