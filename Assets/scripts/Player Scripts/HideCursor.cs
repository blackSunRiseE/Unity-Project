using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCursor : MonoBehaviour
{
    bool cursorVisibility = false;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = cursorVisibility;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = cursorVisibility;
    }
    public void SwitchState(bool state)
    {
        cursorVisibility = state;
    }
}
