using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{    
    [Header("检测参数")]    
    public float checkRadius;//检测半径
    public LayerMask groundLayer;//碰撞面
    public Vector2 bottomOffset;//脚底位移差值,将圆心调整至人物脚底中心
    public Vector2 leftOffset;//左位移差值
    public Vector2 rightOffset;//右位移差值
    [Header("状态")]
    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;    
    private void Update()
    {
        Check();
    }
    public void Check()
    {
        //检测地面
        isGround=Physics2D.OverlapCircle(transform.localScale.x*bottomOffset + (Vector2)transform.position, checkRadius, groundLayer);
        //以人物脚底为圆心
        touchLeftWall = Physics2D.OverlapCircle(leftOffset+(Vector2)transform.position, checkRadius, groundLayer);
        touchRightWall = Physics2D.OverlapCircle(rightOffset + (Vector2)transform.position, checkRadius, groundLayer);        
    }
    private void OnDrawGizmosSelected()
    {
        //绘制虚线圆
        Gizmos.DrawWireSphere(transform.localScale.x * bottomOffset + (Vector2)transform.position, checkRadius);
        Gizmos.DrawWireSphere(leftOffset + (Vector2)transform.position, checkRadius);
        Gizmos.DrawWireSphere(rightOffset + (Vector2)transform.position, checkRadius);        
    }    
}
