using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private bool isHurt; 
    
    private int jumpCount;//��Ծ����
    private bool jumpPressed;//��Ծ������

    public bool isGround,isJump;//������Ծ����

    public float speed, jumpForce;    
    public Transform cellingCheck,groundCheck;
    public Collider2D coll, disColl;
    public int cherry;
    public Text score;           
    public LayerMask ground;    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();        
    }    
    void FixedUpdate()
    {        
        if (!isHurt)
        {
            MoveMent();
        }        
        SwitchAnim();
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        Jump();
    }
    void Update()
    {        
        //NewJump();
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }
        Crouch();
        score.text = cherry.ToString();
    }
    void MoveMent()//��ɫ�ƶ�
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }
        //float horizontalMove = Input.GetAxis("Horizontal");
        //float faceDirection = Input.GetAxisRaw("Horizontal");
        //if (horizontalMove != 0)//��ɫ�ƶ�
        //{
        //    rb.velocity = new Vector2(speed * horizontalMove * Time.fixedDeltaTime, rb.velocity.y);
        //    anim.SetFloat("Running", Mathf.Abs(faceDirection));
        //}
        //if (faceDirection != 0)//��ɫ����
        //{
        //    transform.localScale = new Vector3(faceDirection, 1, 1);
        //}

    }    
    void Jump()
    {
        if (isGround)
        {
            jumpCount = 2;
            isJump = false;//������Ծ����
        }
        if (jumpPressed && isGround)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            SoundManager.instance.JumpAudio();
            jumpCount--;
            jumpPressed = false;
        }else if (jumpPressed && jumpCount > 0 && isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            SoundManager.instance.JumpAudio();
            jumpCount--;
            jumpPressed = false;
        }
        
        //if (Input.GetButton("Jump") && coll.IsTouchingLayers(ground))
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
        //    jumpAudio.Play();
        //    anim.SetBool("Jumping", true);
        //}
    }
    //void NewJump()//��ɫ��Ծ
    //{
    //    if (isGround)
    //    {
    //        extraJump = 1;
    //    }
    //    if (Input.GetButtonDown("Jump") && extraJump > 0)
    //    {
    //        rb.velocity = Vector2.up * jumpForce;
    //        SoundManager.instance.JumpAudio();
    //        //jumpAudio.Play();
    //        anim.SetBool("Jumping", true);
    //        extraJump--;
    //    }
    //    if (Input.GetButtonDown("Jump") && extraJump == 0 && isGround)
    //    {
    //        rb.velocity = Vector2.up * jumpForce;
    //        SoundManager.instance.JumpAudio();
    //        //jumpAudio.Play();
    //        anim.SetBool("Jumping", true);
    //    }
    //}
    void SwitchAnim()//�����л�
    {
        anim.SetFloat("Running", Mathf.Abs(rb.velocity.x));
        if (isGround)
        {
            anim.SetBool("Falling", false);
        }else if (!isGround && rb.velocity.y > 0)
        {
            anim.SetBool("Jumping", true);
        }else if (rb.velocity.y < 0)
        {
            anim.SetBool("Jumping", false);
            anim.SetBool("Falling", true);
        }
        //�Ӹߴ����䴥��Falling����
        //if (rb.velocity.y < 0.1 && !coll.IsTouchingLayers(ground))
        //{            
        //    anim.SetBool("Falling", true);                        
        //}
        //if (anim.GetBool("Jumping"))
        //{
        //    if (rb.velocity.y < 0)
        //    {
        //        anim.SetBool("Jumping", false);
        //        anim.SetBool("Falling", true);
        //    }
        //}
        if (isHurt)
        {
            anim.SetBool("Hurt", true);
            anim.SetFloat("Running", 0);
            if (Mathf.Abs(rb.velocity.x) < 0.1)
            {
                isHurt = false;
                anim.SetBool("Hurt", false);                
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)//��ײ�崥����
    {
        //�ռ���Ʒ
        if (collision.tag == "Collection")
        {

            //Destroy(collision.gameObject);
            //cherry += 10;
            //cherryAudio.Play();
            SoundManager.instance.EatAudio();
            collision.GetComponent<Animator>().Play("isGot");
            
        }
        //
        if (collision.tag == "DeadLine")
        {
            //GetComponent<AudioSource>().enabled = false;
            FindObjectOfType<SoundManager>().Bgm();
            Invoke("Restart", 2f);
            
        }
    }
     void OnCollisionEnter2D(Collision2D collision)//�������
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (anim.GetBool("Falling"))
            {
                enemy.JumpOn();
                rb.velocity = new Vector2(rb.velocity.x,jumpForce);               
            }else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-3, rb.velocity.y);
                //hurtAudio.Play();
                SoundManager.instance.HurtAudio();
                isHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(3, rb.velocity.y);
                //hurtAudio.Play();
                SoundManager.instance.HurtAudio();
                isHurt = true;
            }
        }
        
    }
    void Crouch()//�¶׶���
    {        
        //����˵������û���ϰ���ʱ�����¶׼������¶ף�������վ�𣬿������¶׺�վ�𶯻����л���
        //����������ϰ��if��䲻��ִ�У�С�����һֱ�����¶׶�����ֱ������û���ϰ���ʱ�ٻָ�
        if (!Physics2D.OverlapCircle(cellingCheck.position, 0.2f, ground))
        {//����û���ϰ���ʱִ��
            if (Input.GetButton("Crouch"))
            {
                anim.SetBool("Crouching", true);
                disColl.enabled = false;
            }
            else
            {
                anim.SetBool("Crouching", false);
                disColl.enabled = true;
            }
        }        
    }
    void Restart()//���¼��س���
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //��õ�ǰʹ�ó���������
    }
    public void CherryCount()
    {
        cherry += 10;
    }

}
