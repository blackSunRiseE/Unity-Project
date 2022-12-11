using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectiles : MonoBehaviour
{
   

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
        if(other.CompareTag("Player"))
        {
            PlayerTarget target = other.GetComponent<PlayerTarget>();
            target.TakeDamage(10);
        }
    }
}
