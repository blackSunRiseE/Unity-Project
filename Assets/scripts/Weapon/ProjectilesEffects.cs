using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesEffects : MonoBehaviour
{
    [SerializeField] GameObject colisionImpact;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Projectile"))
        {
            var impact = Instantiate(colisionImpact, other.transform.position, Quaternion.identity) as GameObject;
            Destroy(impact, 1);
            Destroy(gameObject);
        }
        
    }
}
