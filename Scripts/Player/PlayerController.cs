using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;//using UnityEngine意味调用UnityEngine这个命名空间（这个库里的内容）
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
//PlayerController是类名,后面的冒号意味“继承”，继承自MonoBehaviour
{
    private Rigidbody2D rb;    
    private PhysicsCheck physicscheck;
    public PlayerInputControl inputControl;
    //创建一个可以调用PlayerInputControl脚本代码的变量
    private PlayerAnimation playerAnimation;
    private CapsuleCollider2D coll;
    public Vector2 inputDirection;

    [Header("物理材质")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;

    [Header("基本参数")]
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
        //想调用类就要创建实例，这里是在游戏最开始就创建
        inputControl.Gameplay.Jump.started += Jump;
        //started是事件方法，需要添加一个事件注册的函数，用+=注册一个事件函数
        //意思是把Jump这个函数方法添加到按键按下那一刻来执行（started）
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
        //inputDirection.x和y的值在-1到1之间变化（包含小数）
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        //Gameplay和Move用的是自己起的名字
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
    ////这里的other也是Collider2D类型，指的是另一个被碰撞的东西
    //{
    //    Debug.Log(other.name);
    //}
    public void Move()
    {
        //可以使用改变坐标或者速度的方法实现移动，但更多的是改变速度
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        //inputDirection.x和y的值在-1到1之间变化（包含小数）

        //人物翻转
        int faceDir = (int)transform.localScale.x;
        //引入新变量,最后不要直接改变transform这些关键值
        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0)
            faceDir = -1;
        transform.localScale=new Vector3(faceDir, 1, 1);
    }
    private void CheckState()
    {
        coll.sharedMaterial = physicscheck.isGround ? normal : wall;        
    }//材质切换
    #region 按键函数
    private void Jump(InputAction.CallbackContext obj)
    //跳跃代码不需要放在Update或Fixedupdate中（即不需要持续执行）,只要在按键按下时执行即可
    {
        if (physicscheck.isGround)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            //ForceMode有两种模式，连续力Force和瞬时力Impulse
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
        rb.velocity = Vector2.zero;//x轴和y轴速度变成0，把人物惯性停下来
        Vector2 dir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
        //计算受伤方向,为了防止数值过大，使用归一化,normalized

        rb.AddForce(dir*hurtForce, ForceMode2D.Impulse);
    }
    public void PlayerDead()
    {
        isDead= true;
        inputControl.Gameplay.Disable();//禁止玩家操作
    }
    #endregion
}
