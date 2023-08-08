using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttack : ObjectPickup
{
    public float movementDistance = 1f;
    public float speed = 0.5f;
    private bool movingLeft;
    private float leftEdge;
    private float rightEdge;
    private Vector3 curSpeed;
    private void Start()
    {
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
        curSpeed = new Vector3(speed, 0, 0);
    }

    protected override void Update()
    {
        if (movingLeft)
        {
            if (transform.position.x > leftEdge)
            {
                transform.position -= curSpeed * Time.deltaTime;
            }
            else
            {
                movingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightEdge)
            {
                transform.position +=  curSpeed * Time.deltaTime;
            }
            else
            {
                movingLeft = true;
            }
        }
    }
}
