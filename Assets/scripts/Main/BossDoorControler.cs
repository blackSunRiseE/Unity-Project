using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoorControler : MonoBehaviour
{
    public float openSpeedMultiplier = 2.0f;
    public float doorOpenAngle = 90.0f;

    bool open = false;
    bool enter = false;
    [HideInInspector] public bool isInteract = true;
    float defaultRotationAngle;
    float currentRotationAngle;
    float openTime = 0;
    [HideInInspector] public static bool isAvailible = true;

    void Start()
    {
        defaultRotationAngle = transform.GetChild(0).GetChild(0).localEulerAngles.y;
        currentRotationAngle = transform.GetChild(0).GetChild(0).localEulerAngles.y;

    }
    void Update()
    {
        if (openTime < 1)
        {
            openTime += Time.deltaTime * openSpeedMultiplier;
        }
        transform.GetChild(0).GetChild(0).localEulerAngles = new Vector3(transform.GetChild(0).GetChild(0).localEulerAngles.x, Mathf.LerpAngle(currentRotationAngle, defaultRotationAngle + (open ? -doorOpenAngle : 0), openTime), transform.GetChild(0).GetChild(0).localEulerAngles.z);
        
        if (!isAvailible)
        {
            open = false;
            isInteract = false;
        }
        if (Input.GetKeyDown(KeyCode.F) && enter && PlayerTarget.keys == PlayerTarget.maxKeys)
        {
            open = !open;
            currentRotationAngle = transform.GetChild(0).GetChild(0).localEulerAngles.y;
            openTime = 0;

        }
    }

    void OnGUI()
    {
        if (enter)
        {
            if(PlayerTarget.keys == PlayerTarget.maxKeys)
            {
                GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height - 100, 155, 30), "Press 'F' to " + (open ? "close" : "open") + " the door");
            }
            else
            {
                GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height - 100, 155, 60), "Collect all keys to open the door");
            }
        }
            
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isInteract)
        {
            enter = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enter = false;
        }
    }
}
