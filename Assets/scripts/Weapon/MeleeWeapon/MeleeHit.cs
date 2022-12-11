using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            BaseEnemyAI target = other.transform.GetComponent<BaseEnemyAI>();
            target.TakeDamage(10);
        }
    }
}
