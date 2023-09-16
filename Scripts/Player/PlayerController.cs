using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;//using UnityEngine��ζ����UnityEngine��������ռ䣨�����������ݣ�
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
//PlayerController������,�����ð����ζ���̳С����̳���MonoBehaviour
{
    private Rigidbody2D rb;    
    private PhysicsCheck physicscheck;
    public PlayerInputControl inputControl;
    //����һ�����Ե���PlayerInputControl�ű�����ı���
    private PlayerAnimation playerAnimation;
    private CapsuleCollider2D coll;
    public Vector2 inputDirection;

    [Header("�������")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;

    [Header("��������")]
    public float speed;
    public float jumpForce;
    public float hurtForce;
    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();        
        coll = GetComponent<CapsuleCollider2D>();
        physicscheck = GetComponent<PhysicsCheck>();
        playerAnimation = GetComponent<PlayerAnimation>();
        inputControl = new PlayerInputControl();
        //��������Ҫ����ʵ��������������Ϸ�ʼ�ʹ���
        inputControl.Gameplay.Jump.started += Jump;
        //started���¼���������Ҫ���һ���¼�ע��ĺ�������+=ע��һ���¼�����
        //��˼�ǰ�Jump�������������ӵ�����������һ����ִ�У�started��
        inputControl.Gameplay.Attack.started += PlayerAttack;
    }

    private void OnEnable()
    {
        inputControl.Enable();        
    }
    private void OnDisable()
    {
        inputControl.Disable();
    }
    private void Update()
    {
        //inputDirection.x��y��ֵ��-1��1֮��仯������С����
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        //Gameplay��Move�õ����Լ��������
        CheckState();
    }
    private void FixedUpdate()
    {
        if(!isHurt&&!isAttack)
        {
            Move();       
        }
    }
    //private void OnTriggerStay2D(Collider2D other)
    ////�����otherҲ��Collider2D���ͣ�ָ������һ������ײ�Ķ���
    //{
    //    Debug.Log(other.name);
    //}
    public void Move()
    {
        //����ʹ�øı���������ٶȵķ���ʵ���ƶ�����������Ǹı��ٶ�
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        //inputDirection.x��y��ֵ��-1��1֮��仯������С����

        //���﷭ת
        int faceDir = (int)transform.localScale.x;
        //�����±���,���Ҫֱ�Ӹı�transform��Щ�ؼ�ֵ
        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0)
            faceDir = -1;
        transform.localScale=new Vector3(faceDir, 1, 1);
    }
    private void CheckState()
    {
        coll.sharedMaterial = physicscheck.isGround ? normal : wall;        
    }//�����л�
    #region ��������
    private void Jump(InputAction.CallbackContext obj)
    //��Ծ���벻��Ҫ����Update��Fixedupdate�У�������Ҫ����ִ�У�,ֻҪ�ڰ�������ʱִ�м���
    {
        if (physicscheck.isGround)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            //ForceMode������ģʽ��������Force��˲ʱ��Impulse
        }
    }
    
    private void PlayerAttack(InputAction.CallbackContext obj)
    {        
            isAttack = true;
            playerAnimation.PlayerAttack();               
    }
    #endregion
    #region UnityEvent
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;//x���y���ٶȱ��0�����������ͣ����
        Vector2 dir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
        //�������˷���,Ϊ�˷�ֹ��ֵ����ʹ�ù�һ��,normalized

        rb.AddForce(dir*hurtForce, ForceMode2D.Impulse);
    }
    public void PlayerDead()
    {
        isDead= true;
        inputControl.Gameplay.Disable();//��ֹ��Ҳ���
    }
    #endregion
}
