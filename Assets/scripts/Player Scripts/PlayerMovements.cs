using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{

    public float moveSpeed;
    float RunningSpeed = 15;
    public float WalkingSpeed = 2;
    [SerializeField] float gravity = -9.81f;

    [SerializeField] float groundYOffset;
    [SerializeField] LayerMask groundMask;
    [HideInInspector] public Vector3 dir;
    float hzInput, vInput;
    CharacterController controller;
    Vector3 velocity;
    Vector3 spherePos;
    Rigidbody rb;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        moveSpeed = WalkingSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = RunningSpeed;
        }
        GetDirectionAndMove();
        Gravity();
    }

    bool IsGrounded()
    {
        spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        if (Physics.CheckSphere(spherePos, controller.radius - 0.05f, groundMask)) return true;
        return false;
    }

    void Gravity()
    {
        if (!IsGrounded()) velocity.y += gravity * Time.deltaTime;
        else if (velocity.y < 0) velocity.y = -2;

        controller.Move(velocity * Time.deltaTime);
    }
    void GetDirectionAndMove()
    {
        hzInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");

        dir = transform.forward * vInput + transform.right * hzInput;

        Vector3 directionVector = new Vector3(hzInput, 0, vInput);

        controller.Move(dir.normalized * moveSpeed * Time.deltaTime);
    }

    
}
