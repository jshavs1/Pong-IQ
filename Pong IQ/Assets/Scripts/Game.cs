using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    public int id, remainingShots = 12;
    public NeuralNet nn;
    public double[] weights, L1bias, L2bias;
    public bool[] remainingCups;
    public float h;

    public Game(NeuralNet nn, double[] weights, double[] L1bias, double[] L2bias)
    {
        h = 0f;
        remainingCups = new bool[10];
        this.weights = weights;
        this.L1bias = L1bias;
        this.L2bias = L2bias;
        this.nn = nn;
    }

    public void heuristic(int cup, float score)
    {
        if (cup != -1) { return; }
        bool b = remainingCups[cup];
        h += b ? -score : score;
    }

    public float fitness
    {
        get
        {
            return h;
        }
    }

}
