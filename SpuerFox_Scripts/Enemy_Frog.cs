using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : Enemy
{
    private Rigidbody2D rb;
    

    public Collider2D coll;
    public LayerMask ground;
    public Transform Leftpoint, Rightpoint;
    private float leftx, rightx;
    
    public float speed,jumpForce;
    private bool faceLeft=true;
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        
        leftx = Leftpoint.position.x;
        rightx = Rightpoint.position.x;
        Destroy(Leftpoint.gameObject);
        Destroy(Rightpoint.gameObject);
        
    }
    void Update()
    {
        SwitchAnim();        
    }
    void MoveMent()
    {
        if (faceLeft)
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("Jumping", true);
                rb.velocity = new Vector2(-speed, jumpForce);
            }            
            if (transform.position.x < leftx)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                faceLeft = false;                
            }
        }
        else
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("Jumping", true);
                rb.velocity = new Vector2(speed, jumpForce);
            }            
            if (transform.position.x > rightx)
            {
                transform.localScale = new Vector3(1, 1, 1);
                faceLeft = true;                
            }
        }
    }
    void SwitchAnim()
    {
        if (anim.GetBool("Jumping"))
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("Jumping", false);
                anim.SetBool("Falling", true);
            }
        }
        if (anim.GetBool("Falling") && coll.IsTouchingLayers(ground))
        {
            anim.SetBool("Falling", false);
        }
    }
    
}
