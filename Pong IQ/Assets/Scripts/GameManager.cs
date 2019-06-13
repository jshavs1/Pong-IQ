using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, IPongDelegate
{
    public Text text;
    public Material idle, hit, miss;

    private CollisionDetection[] colliders;
    private double confidence;

    public void CollisionDetected(CollisionDetection[] colliders, CollisionDetection collider, int index)
    {
        if (index == -1) { return; }

        foreach (CollisionDetection col in colliders)
        {
            if (col.index != -1)
                col.gameObject.GetComponent<Renderer>().material = miss;
        }
        collider.gameObject.GetComponent<Renderer>().material = hit;
    }

    public void Shooting(double confidence)
    {
        this.confidence = confidence;
    }

    public void StateChanged(State prev, State next)
    {
        switch (next)
        {
            case State.Idle:
                text.text = "Idle";
                break;
            case State.Shooting:
                text.text = "Shooting (Confidence: " + confidence + ")";
                ResetMaterials();
                break;
            case State.Training:
                text.text = "Training...";
                break;
        }
    }

    private void ResetMaterials()
    {
        foreach(CollisionDetection col in colliders)
        {
            if (col.index != -1)
            {
                col.GetComponent<Renderer>().material = idle;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        colliders = FindObjectsOfType<CollisionDetection>();
    }
}
