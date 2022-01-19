using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFrog : Enemy
{
    [SerializeField] Rigidbody2D rb;   
    [SerializeField] Collider2D coll;  
    [SerializeField] Transform Left; 
    [SerializeField] Transform Right;

    [SerializeField] LayerMask ground;

    [SerializeField] bool Faceleft = true;

    [SerializeField] float Speed, JumpForce;

    float LeftX, RightX;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();       
        coll = GetComponent<Collider2D>();
        transform.DetachChildren();
        LeftX = Left.position.x;
        RightX = Right.position.x;
        Destroy(Left.gameObject);    
        Destroy(Right.gameObject);
    }

    private void FixedUpdate()
    {
        //Move();
  
    }

    private void Update()
    {
        SwitchAnim();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       /* if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Ground")
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            Faceleft = !Faceleft;
        }*/
      
    }


    void Move()
    {
        if (Faceleft)
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumpping", true);
                rb.velocity = new Vector2(-Speed, JumpForce);
                if (transform.position.x < LeftX)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                    Faceleft = false;
                }
            }
           
        }
        else
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumpping", true);
                rb.velocity = new Vector2(Speed, JumpForce);
                if (transform.position.x > RightX)
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    Faceleft = true;
                }
            }
            
        }
    }

    void SwitchAnim()
    {
        if (anim.GetBool("jumpping"))
        {
            if (rb.velocity.y < 0.1)
            {
                anim.SetBool("jumpping", false);
                anim.SetBool("falling", true);
            }
        }
        if (coll.IsTouchingLayers(ground) && anim.GetBool("falling"))
        {
            anim.SetBool("falling", false);
        }
    }

   
}
