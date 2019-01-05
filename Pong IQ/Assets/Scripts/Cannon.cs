﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject ball;
    public float maxForce = 5f;
    public float minForce = 3f;

    public void Shoot(float force, float rotation)
    {
        GameObject newBall = GameObject.Instantiate(ball);
        newBall.transform.position = this.transform.position;

        this.transform.rotation = Quaternion.identity;
        this.transform.eulerAngles = new Vector3(0f, rotation * 50f - 25f);

        newBall.GetComponent<Rigidbody>().AddForce((transform.forward + Vector3.up * 2f) * Mathf.Lerp(minForce, maxForce, force), ForceMode.Impulse);
    }
}
