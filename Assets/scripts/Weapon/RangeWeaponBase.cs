using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponBase : MonoBehaviour
{
    [SerializeField] LayerMask aimMask;
    private float LastShootTime;
    [SerializeField] private float ShootDelay = 0.5f;
    FireBallSpell fireBall = new FireBallSpell();
    [SerializeField] GameObject projectile;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projSpeed = 30;
    [SerializeField] private float damage = 10;
    private Vector3 destination;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            Ray ray = Camera.main.ScreenPointToRay(screenCenter);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, aimMask))
            {
                destination = hit.point;
            }
            else
            {
                destination = ray.GetPoint(100);
            }
            Shoot(hit);
        }
    }

    private void Shoot(RaycastHit hit)
    {
        if (LastShootTime + ShootDelay < Time.time)
        {
            ///add different spells from projecttile prefab
            InstantiateProjecttile();
            LastShootTime = Time.time;

        }
        if (hit.transform != null)
        {
            Debug.Log("Shoot");
            BaseEnemyAI target = hit.transform.GetComponent<BaseEnemyAI>();
            target.TakeDamage(damage);
        }
    }

    void InstantiateProjecttile()
    {
        var projectileObj = Instantiate(projectile, firePoint.position, Quaternion.identity);
        projectileObj.GetComponent<Rigidbody>().velocity = (destination - firePoint.position).normalized * projSpeed;
    }

    
}