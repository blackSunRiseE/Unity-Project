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
    [SerializeField] float aimSmoothSpeed =  20;
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

        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        RaycastHit hit;

        /*if (Physics.Raycast(ray, out hit, Mathf.Infinity, aimMask))
        {
            aimPos.position = Vector3.Lerp(aimPos.position, hit.point, aimSmoothSpeed * Time.deltaTime);
        }*/


    }


}
