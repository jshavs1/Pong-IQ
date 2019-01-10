using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Cannon cannon;
    public PongIq pongIq;
    public bool ignoreCollision = false;
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (ignoreCollision) return;
        ignoreCollision = true;
        CollisionDetection cd = collision.contacts[0].otherCollider.gameObject.GetComponent<CollisionDetection>();
        pongIq.CollisionDetected(cd.index, cd.value);

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        cannon.ResetBall();
    }
}
