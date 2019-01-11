using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject ball;
    private GameObject activeBall;


    public void Start()
    {
        activeBall = GameObject.Instantiate(ball);
        activeBall.GetComponent<Ball>().cannon = this;
        activeBall.GetComponent<Ball>().pongIq = GetComponentInParent<PongIq>();
    }

    public void ResetBall()
    {
        activeBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
        activeBall.transform.position = this.transform.position;
    }

    public IEnumerator Shoot(float force, float rotation, float maxForce, float minForce, float maxRotation)
    {
        yield return null;
        ResetBall();
        float adjForce = Mathf.Lerp(minForce, maxForce, force), adjRotation = Mathf.Lerp(-maxRotation, maxRotation, rotation);
        activeBall.GetComponent<Ball>().ignoreCollision = false;
        this.transform.rotation = Quaternion.identity;
        this.transform.eulerAngles = new Vector3(0f, adjRotation);

        activeBall.GetComponent<Rigidbody>().AddForce((transform.forward + Vector3.up * 2f) * adjForce, ForceMode.Impulse);
    }
}
