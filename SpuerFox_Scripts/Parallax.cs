using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cam;
    public float moveRate;
    public bool lockY;//Ëø¶¨YÖáÄ¬ÈÏ¹Ø±Õ

    private float startPointX,startPointY;

    void Start()
    {
        startPointX = transform.position.x;
        startPointY = transform.position.y;
    }

    
    void Update()
    {
        if (lockY)
        {
            transform.position = new Vector2(startPointX - 2 + cam.position.x * moveRate, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(startPointX - 2 + cam.position.x * moveRate, startPointY+cam.position.y*moveRate);
        }
        
        
    }
}
