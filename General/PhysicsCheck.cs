using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{    
    [Header("������")]    
    public float checkRadius;//���뾶
    public LayerMask groundLayer;//��ײ��
    public Vector2 bottomOffset;//�ŵ�λ�Ʋ�ֵ,��Բ�ĵ���������ŵ�����
    public Vector2 leftOffset;//��λ�Ʋ�ֵ
    public Vector2 rightOffset;//��λ�Ʋ�ֵ
    [Header("״̬")]
    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;    
    private void Update()
    {
        Check();
    }
    public void Check()
    {
        //������
        isGround=Physics2D.OverlapCircle(transform.localScale.x*bottomOffset + (Vector2)transform.position, checkRadius, groundLayer);
        //������ŵ�ΪԲ��
        touchLeftWall = Physics2D.OverlapCircle(leftOffset+(Vector2)transform.position, checkRadius, groundLayer);
        touchRightWall = Physics2D.OverlapCircle(rightOffset + (Vector2)transform.position, checkRadius, groundLayer);        
    }
    private void OnDrawGizmosSelected()
    {
        //��������Բ
        Gizmos.DrawWireSphere(transform.localScale.x * bottomOffset + (Vector2)transform.position, checkRadius);
        Gizmos.DrawWireSphere(leftOffset + (Vector2)transform.position, checkRadius);
        Gizmos.DrawWireSphere(rightOffset + (Vector2)transform.position, checkRadius);        
    }    
}
