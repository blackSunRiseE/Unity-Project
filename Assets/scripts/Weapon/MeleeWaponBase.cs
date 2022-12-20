using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWaponBase : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = transform.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("IsAttack");
        }
    }
}
