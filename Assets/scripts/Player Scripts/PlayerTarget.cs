using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : MonoBehaviour
{
    [SerializeField] float health = 50f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Debug.Log("Dies");
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
