using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] float weaponDamage = 10;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerTarget target = other.GetComponent<PlayerTarget>();
            target.TakeDamage(weaponDamage);
        }
    }
}
