using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPongDelegate
{
    void StateChanged(State prev, State next);
    void CollisionDetected(CollisionDetection[] colliders, CollisionDetection collider, int index);
    void Shooting(double confidence);
}
