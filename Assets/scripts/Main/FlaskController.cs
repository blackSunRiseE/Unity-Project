using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaskController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerTarget>().Heal(10f);
            Destroy(gameObject);
        }
    }
}
