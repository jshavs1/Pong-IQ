using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject ball;
    public float maxForce = 5f;
    public float minForce = 3f;
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

    public IEnumerator Shoot(float force, float rotation)
    {
        yield return null;
        ResetBall();
        activeBall.GetComponent<Ball>().ignoreCollision = false;
        this.transform.rotation = Quaternion.identity;
        this.transform.eulerAngles = new Vector3(0f, rotation * 50f - 25f);

        activeBall.GetComponent<Rigidbody>().AddForce((transform.forward + Vector3.up * 2f) * Mathf.Lerp(minForce, maxForce, force), ForceMode.Impulse);
    }
}
