using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerControlle : MonoBehaviour
{ 
    [SerializeField] Rigidbody2D rb;            //刚体
    [SerializeField] Collider2D coll;           //碰撞器    
    [SerializeField] Collider2D coll2;          //下蹲碰撞器   
    [SerializeField] Collider2D coll3;          //单向板碰撞器
    [SerializeField] Text CherryText;           //文本对象
    [SerializeField] Text GemText;              //文本对象
    Animator anim;                              //动画组件


    [Header("========Move========")]
    [SerializeField] float movespeed;           //移动速度

    [Header("========Jump========")]
    [SerializeField] AudioSource jumpSFX;       //跳跃音效
    [SerializeField] float jumpForce;           //跳跃增加的力
    [SerializeField] int jumpCount;             //跳跃次数
    [SerializeField] Transform groundCheck;     //地面检测点
    [SerializeField] Transform ceilingCheck;    //头顶检测点
    [SerializeField] LayerMask ground;          //地面层级
    [SerializeField] LayerMask enemy;           //地面层级

    [SerializeField] bool isGround;             //是否在地上 
    
    [SerializeField] bool isEnemy;              //是否踩到敌人 

    [Header("========Num========")]
    [SerializeField] AudioSource numSFX;       //Num音效
    [SerializeField] int Cherry;
    [SerializeField] int Gem;

    [Header("========Hurt========")]
    [SerializeField] AudioSource hurtSFX;       //Hurt音效
    [SerializeField] bool isHurt;    
    [SerializeField] float hurtForce;

    float horizontalmove;                       //水平移动速度

    bool jumpPressed;                           //跳跃键是否按下
    int tmp;                                    //跳跃次数
    float hurtTime = 0.5f;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        tmp = jumpCount;
        //coll = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, ground); 
        isEnemy = Physics2D.OverlapCircle(groundCheck.position, 0.2f, enemy);

        if (!isHurt)
        {
            onMove();
        }
       
        //onJump();
        SwitchAnim();
    }   
    
    void Update()
    {
        onJump();
        onCrouch();
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true; 
        }
    }

    void onMove()
    {
        horizontalmove = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(horizontalmove * movespeed, rb.velocity.y);
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));
   
        if (horizontalmove != 0) //角色朝向
        {
            transform.localScale = new Vector3(horizontalmove, 1, 1);
         
        }
    }

    void onJump()
    {
        if (isGround)
        {
            jumpCount = tmp;
        }
        if (jumpPressed && isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
            jumpSFX.Play();
        }
        else if (jumpPressed && jumpCount > 0 && !isGround) 
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
            jumpSFX.Play();
        }
    }

    void onCrouch()
    {

        if (Input.GetButton("Crouch") && isGround)
        {
            anim.SetBool("crouch", true);
            coll.enabled = false;
            coll2.enabled = true;
        }
        else
        {
            if (!Physics2D.OverlapCircle(ceilingCheck.position, 0.3f, ground))
            {
                anim.SetBool("crouch", false);
                coll.enabled = true;
                coll2.enabled = false;
            }
        }
        
      
    }


    //动画切换
    void SwitchAnim()
    {
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));

        if (rb.velocity.y < 0.2f && !coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", true);
        }
        if (anim.GetBool("jumpping"))
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("jumpping", false);
                anim.SetBool("falling", true);
            }
        }
        else if (isHurt)
        {
            
            anim.SetBool("hurt", true);
            StartCoroutine(nameof(WaitTime));
            if (Mathf.Abs(rb.velocity.x) < 0.1f) 
             {
                 anim.SetBool("hurt", false);
                 isHurt = false;
                StopAllCoroutines();
            }
   
        }
        else if (isGround)
        {
            anim.SetBool("falling", false);
        }
        else if (!isGround && rb.velocity.y > 0)
        {
            anim.SetBool("jumpping", true);
        }
        else if (rb.velocity.y < 0)
        {
            anim.SetBool("jumpping", false);
            anim.SetBool("falling", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Cherry")
        {
            numSFX.Play();
            collision.gameObject.SetActive(false);
            Cherry += 1;
            CherryText.text = Cherry.ToString();
        }  
        if (collision.tag == "Gem")
        {
            numSFX.Play();
            collision.gameObject.SetActive(false);
            Gem += 1;
            GemText.text = Gem.ToString();
        }

        if(collision.tag == "DeadLine")
        {
            //GetComponent<AudioSource>().enabled = false;
            Invoke("Restart", 1.5f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" )
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (isEnemy)
            {
                enemy.JumpOn();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                anim.SetBool("jumpping", true);
            }
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {

                rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                hurtSFX.Play();
                isHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                hurtSFX.Play();
                isHurt = true;
            }
        }
        
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(hurtTime);
        rb.velocity = new Vector2(0, 0);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerStay2D(Collider2D collision) 
    {
        if (!isGround) 
        {
            return;
        }
        if (isGround && Input.GetAxis("Vertical") < -0.1f) 
        {
            coll3.enabled = false; 
            Invoke("RestoreCollider", 0.5f);
        }
    }

    private void RestoreCollider() //恢复碰撞体函数
    {
        coll3.enabled = true;
    }
}
