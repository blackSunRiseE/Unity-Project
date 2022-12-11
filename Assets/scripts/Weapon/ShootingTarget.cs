using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTarget : MonoBehaviour
{
    [SerializeField] float health = 50f;
    [HideInInspector] public Animator animator;
    bool Chase;

    private void Start()
    {
        animator = GetComponent<Animator>();
        
    }
    public void TakeDamage(float amount)
    {
        Chase = animator.GetBool("isChase");

        animator.SetBool("isDamaged", true);

        animator.SetBool("isChase", false);

        Invoke("normal",1f);
        health -= amount;
        if(health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool("isDead", true);
        //Destroy(gameObject,1);
    }

    void normal()
    {
        animator.SetBool("isDamaged", false);

        animator.SetBool("isChase", Chase);
    }
}
