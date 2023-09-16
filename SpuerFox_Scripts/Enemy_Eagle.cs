using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : Enemy
{
    private Rigidbody2D rb;
    public float speed;
    private bool isUp=true;
    public Transform toppoint, bottompoint;
    private float topY, bottomY;
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        topY = toppoint.position.y;
        bottomY = bottompoint.position.y;
        Destroy(toppoint.gameObject);
        Destroy(bottompoint.gameObject);
        
    }
    void Update()
    {
        MoveMent();


    }
    void MoveMent()
    {
        if (isUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);
            if (transform.position.y > topY)
            {
                isUp = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -speed);
            if (transform.position.y < bottomY)
            {
                isUp = true;
            }
        }
    }
}
