using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesEffects : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject colisionImpact;
    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var impact = Instantiate(colisionImpact, other.transform.position, Quaternion.identity) as GameObject;
        Destroy(impact, 1);
        Destroy(gameObject);
    }
}
