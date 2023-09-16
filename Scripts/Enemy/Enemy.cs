using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //��д���η���Ĭ����private
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector]public Animator anim;    
    [HideInInspector] public PhysicsCheck physicsCheck; 
    
    [Header("��������")]
    public Transform attacker;
    public float normalSpeed;
    public float chaseSpeed;
    [HideInInspector] public float currentSpeed;
    public Vector3 faceDir;
    public float hurtForce;
    [Header("������")]
    public bool wait;
    public float waitTime;
    public float waitTimeCounter;
    [Header("״̬")]
    public bool isHurt;
    public bool isDead;
    [Header("���")]
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
    //�������ʲô����������Update��ִ��
    {             
        faceDir = new Vector3(-transform.localScale.x, 0, 0);//�泯����        
        currentState.LogicUpdate();       
        TimeCounter();
    }
    private void FixedUpdate()
    //�����ƶ�֮��ģ�����FixedUpdate��
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
    //�ڸ����м�һ��virtual���η�,virtual��˼�ǲ��̶���,����ͨ����������������޸�
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
                transform.localScale=new Vector3(faceDir.x,1,1);//����ת��
            }
        }
    }
    public bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, faceDir, checkDistance);
    }

    public void SwitchState(NPCState state)//״̬ת��
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            _=>null//�൱��default
        };
        currentState.OnExit();
        currentState= newState;
        currentState.OnEnter(this);
    }

    public void OnTakeDamage(Transform attackTrans)
    {
        attacker= attackTrans;
        //ת��
        if (attackTrans.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        if (attackTrans.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(1, 1, 1);

        //���˻���        
        isHurt = true;
        anim.SetTrigger("hurt");
        rb.velocity = Vector2.zero;
        //�Ӻ����ǰ�湥��Ұ��Ļ��˾����ǲ�һ���ģ���Ϊ���˵�������ƶ�����������
        //Ҫ�ĵĻ��ڱ�����ʱ��x������ٶȹ������

        Vector2 dir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
        //���˻��˷���
        StartCoroutine(OnHurt(dir));//����Э��,��д�õ�Э�̴��ݽ�ȥ
    }
    private IEnumerator OnHurt(Vector2 dir)
    //IEnumerate��Э�̵�һ������ֵ,��int,float������
    //�����ص���һ��������;����Ctrl���������ϸ�鿴
    //����������һ����˳������һִ�д���,��MoveNext�м������ӵȴ�������,
    //�ȴ����֮��,��ִ�к��������
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);//���عؼ���
        isHurt = false;
    }
    #region UnityEvent
    public void OnDie()
    {
        gameObject.layer = 2;//����Layer��ż���
        anim.SetBool("dead", true);
        isDead = true;
    }
    public void DestroyAfterAnimation()
    {
        Destroy(this);        
    }
    #endregion
}
