using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] float mouseSense = 1;
    public float xAxis, yAxis;
    [SerializeField] Transform cameraFollowPos;

    //[SerializeField] Transform aimPos;
    [HideInInspector] public Vector3 actualAimPos;
    [SerializeField] LayerMask aimMask;
    float rotationX = 0;

    private float LastShootTime;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        rotationX += -Input.GetAxis("Mouse Y") * mouseSense;
        rotationX = Mathf.Clamp(rotationX, -45, 45);
        Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * mouseSense, 0);

        xAxis += Input.GetAxisRaw("Mouse X") * mouseSense;
        yAxis -= Input.GetAxisRaw("Mouse Y") * mouseSense;
        yAxis = Mathf.Clamp(yAxis, -80, 80);
    }


}
