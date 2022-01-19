using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected AudioSource sXF;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        sXF = GetComponent<AudioSource>();
    }

    public void Death()
    {
        GetComponent<Collider2D>().enabled = false;
        gameObject.SetActive(false);
    }

    public void JumpOn()
    {
        sXF.Play();
        anim.SetTrigger("death");
    }

}
