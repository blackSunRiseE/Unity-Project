using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeEnemyAI : BaseEnemyAI
{
    // Start is called before the first frame update
    [SerializeField] LayerMask aimMask;
    [SerializeField] public Transform player;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float attackRange = 20;
    [SerializeField] public float attackDelay = 1;
    [SerializeField] public float attackDamage = 10;
    [SerializeField] public float Hp = 50;
    [SerializeField] public float enemyProjectileSpeed = 50;
    [SerializeField] public float enemyRangeToRun = 40;
    private NavMeshAgent enemy;

    BaseRangeState currentState;
    [HideInInspector] public BaseRangeState prevState;

    [HideInInspector] public RangeIdleState Idle = new RangeIdleState();
    [HideInInspector] public RangeAttackState Attack = new RangeAttackState();
    [HideInInspector] public RangeMoveState Move = new RangeMoveState();
    [HideInInspector] public RangeStrafeState Strafe = new RangeStrafeState();
    [SerializeField] GameObject projectile;
    [SerializeField] private Transform firePoint;

    private float lastAttackTime;
    Vector3 positionBeforeStrafe;
    Vector3 trans = Vector3.left;

    [HideInInspector] public Animator animator;
    [HideInInspector] public float animationDuration;

    [HideInInspector] public bool getHit = false;
    [HideInInspector] public WeaponState weaponState;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        SwitchState(Idle);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }


    public void SwitchState(BaseRangeState state)
    {
        prevState = currentState;
        currentState = state;
        currentState.EnterState(this);
    }
    public float getDistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.position);
    }

    public void ShootPlayer()
    {
        Vector3 playerPosition = new Vector3(player.position.x,
                                             transform.position.y,
                                             player.position.z);
        transform.LookAt(playerPosition);
        if (lastAttackTime + attackDelay < Time.time)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, aimMask))
            {
                ShootProjectile(hit,ray);
            }
            lastAttackTime = Time.time;
        }
    }

    void ShootProjectile(RaycastHit hit, Ray ray)
    {
        var projectileObj = Instantiate(projectile, firePoint.position, Quaternion.identity);
        projectileObj.GetComponent<Rigidbody>().velocity = (hit.point - firePoint.position).normalized * enemyProjectileSpeed;
    }
    
    public void getPositionBeforeStrafe()
    {
        positionBeforeStrafe = transform.position;
    }

    public void StrafeFromPlayer()
    {
        //transform.LookAt(playerPosition);
        
    }

    public void RunAwayFromPlayer()
    {
        Vector3 playerPosition = new Vector3(player.position.x,
                                             transform.position.y,
                                             player.position.z);
        transform.LookAt(playerPosition);

        Vector3 runTo = transform.position + ((transform.position - playerPosition + new Vector3(Random.Range(-12, 12), 0, Random.Range(-15, 12)) * 1));
        float distance = Vector3.Distance(transform.position, playerPosition);
        enemy.speed = moveSpeed;
        if (distance < attackRange) enemy.SetDestination(runTo);

    }

    public void GetPlayerState()
    {
        if (player.GetChild(1).GetChild(0).gameObject.activeSelf)
        {
            weaponState = WeaponState.RangeWeapon;
        }
        else
        {
            weaponState = WeaponState.MeleeWeapon;
        }
        
    }

    public override void TakeDamage(float damage)
    {
        animationDuration = animator.runtimeAnimatorController.animationClips[2].length;
        getHit = true;
        Hp -= damage;
    }



}
