using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    PongIq pongIq;
    public int index;
    public double value;

    private void Start()
    {
        pongIq = GetComponentInParent<PongIq>();
        if (index < 0) { value = -1.0; }
        else { value = 1.0; }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //pongIq.CollisionDetected(index, value);
    }
}
