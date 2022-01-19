using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEagle : Enemy
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform Top;
    [SerializeField] Transform Btn;


    [SerializeField] bool isUp = true;

    [SerializeField] float UpSpeed;

    float TopY, BtnY;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        transform.DetachChildren();
        TopY = Top.position.y;
        BtnY = Btn.position.y;
        Destroy(Top.gameObject);
        Destroy(Btn.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (isUp)
        {
            rb.velocity = new Vector2(0f, UpSpeed);
            if(transform.position.y > TopY)
            {
                isUp = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(0f, -UpSpeed);
            if (transform.position.y < BtnY)
            {
                isUp = true;
            }
        }
    }
}
