using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetKey : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            PlayerTarget playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTarget>();
            playerTarget.AddKey();
            Destroy(gameObject);
        }
    }
}
