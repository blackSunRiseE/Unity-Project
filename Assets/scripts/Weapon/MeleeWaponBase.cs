using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWaponBase : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("hit");
            animator.SetTrigger("IsAttack");
        }
    }
}
