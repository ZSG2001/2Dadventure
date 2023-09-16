using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //不写修饰符，默认是private
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector]public Animator anim;    
    [HideInInspector] public PhysicsCheck physicsCheck; 
    
    [Header("基本参数")]
    public Transform attacker;
    public float normalSpeed;
    public float chaseSpeed;
    [HideInInspector] public float currentSpeed;
    public Vector3 faceDir;
    public float hurtForce;
    [Header("计数器")]
    public bool wait;
    public float waitTime;
    public float waitTimeCounter;
    [Header("状态")]
    public bool isHurt;
    public bool isDead;
    [Header("检测")]
    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;

    private BaseState currentState;
    protected BaseState patrolState;
    protected BaseState chaseState;
    protected virtual void Awake()
    {        
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();

        currentSpeed = normalSpeed;
        waitTimeCounter = waitTime;
    }
    private void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }    
    private void Update()
    //持续获得什么东西，放在Update中执行
    {             
        faceDir = new Vector3(-transform.localScale.x, 0, 0);//面朝方向        
        currentState.LogicUpdate();       
        TimeCounter();
    }
    private void FixedUpdate()
    //物理移动之类的，放在FixedUpdate中
    {
        if (!isHurt && !isDead && !wait)
        {
            Move();
        }
        currentState.PhysicsUpdate();
    }
    private void OnDisable()
    {
        currentState.OnExit();
    }
    public virtual void Move()
    //在父类中加一个virtual修饰符,virtual意思是不固定的,可以通过子类访问它进行修改
    {
        rb.velocity=new Vector2(normalSpeed*faceDir.x*Time.deltaTime,rb.velocity.y);        
    }
    public void TimeCounter()
    {        
        if (wait)
        {
            //rb.velocity=Vector2.zero;
            waitTimeCounter-= Time.deltaTime;
            if(waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;
                transform.localScale=new Vector3(faceDir.x,1,1);//敌人转身
            }
        }
    }
    public bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, faceDir, checkDistance);
    }

    public void SwitchState(NPCState state)//状态转换
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            _=>null//相当于default
        };
        currentState.OnExit();
        currentState= newState;
        currentState.OnEnter(this);
    }

    public void OnTakeDamage(Transform attackTrans)
    {
        attacker= attackTrans;
        //转身
        if (attackTrans.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        if (attackTrans.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(1, 1, 1);

        //受伤击退        
        isHurt = true;
        anim.SetTrigger("hurt");
        rb.velocity = Vector2.zero;
        //从后面和前面攻击野猪的击退距离是不一样的，因为击退的力与怪移动的力叠加了
        //要改的话在被攻击时把x方向的速度归零就行

        Vector2 dir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
        //受伤击退方向
        StartCoroutine(OnHurt(dir));//启动协程,把写好的协程传递进去
    }
    private IEnumerator OnHurt(Vector2 dir)
    //IEnumerate是协程的一个返回值,和int,float等相似
    //它返回的是一个迭代器;按着Ctrl点击可以详细查看
    //帮助我们用一定的顺序来逐一执行代码,在MoveNext中间可以添加等待的内容,
    //等待完成之后,在执行后面的内容
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);//返回关键词
        isHurt = false;
    }
    #region UnityEvent
    public void OnDie()
    {
        gameObject.layer = 2;//输入Layer编号即可
        anim.SetBool("dead", true);
        isDead = true;
    }
    public void DestroyAfterAnimation()
    {
        Destroy(this);        
    }
    #endregion
}
