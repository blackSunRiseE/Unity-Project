using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponState : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject MeleePrefab;
    [SerializeField] GameObject RangePrefab;
    [SerializeField] Transform Player;

    static WeaponState weaponState = WeaponState.MeleeWeapon;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponState = WeaponState.MeleeWeapon;
            changeWeaponState();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponState = WeaponState.RangeWeapon;
            changeWeaponState();
        }
    }

    public static WeaponState GetWeaponState()
    {
        return weaponState;
    }

    void changeWeaponState()
    {
        if(weaponState == WeaponState.MeleeWeapon)
        {
            Player.GetChild(1).GetChild(1).gameObject.SetActive(true);
            Player.GetChild(1).GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            Player.GetChild(1).GetChild(1).gameObject.SetActive(false);
            Player.GetChild(1).GetChild(0).gameObject.SetActive(true);
        }
    }
}
