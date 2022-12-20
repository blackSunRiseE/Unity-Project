using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeEnemyAI : BaseEnemyAI
{
    // Start is called before the first frame update
    [SerializeField] LayerMask aimMask;
    [HideInInspector] public Transform player;
    [HideInInspector] public float moveSpeed = 2f;
    [SerializeField] public float attackRange = 20f;
    float attackDelay = 3f;
    [SerializeField] public float attackDamage = 10f;
    [SerializeField] public float enemyProjectileSpeed = 50f;
    [SerializeField] public float enemyRangeToRun = 40f;

    BaseRangeState currentState;
    [HideInInspector] public BaseRangeState prevState;

    [HideInInspector] public RangeIdleState Idle = new RangeIdleState();
    [HideInInspector] public RangeAttackState Attack = new RangeAttackState();
    [HideInInspector] public RangeMoveState Move = new RangeMoveState();
    [HideInInspector] public RangeChaseState Chase = new RangeChaseState();
    [HideInInspector] public RangeDamagedState Damaged = new RangeDamagedState();
    [HideInInspector] public RangeDeadState Dead = new RangeDeadState();
    [SerializeField] GameObject projectile;
    [SerializeField] private Transform firePoint;

    bool animationStart = true;
    bool animationStop = false;
    public float lastAttackTime;

    [HideInInspector] public WeaponState weaponState;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        enemy = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        health = maxHealth;
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
        if(lastAttackTime + attackDelay - 0.5f < Time.time && animationStart)
        {
            animator.SetBool("isAttack", true);
            animationStart = false;
        }
        if (lastAttackTime + attackDelay < Time.time)
        {
            
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, aimMask))
            {
                ShootProjectile(hit, ray);
            }
            animationStart = true;
            animationStop = true;
            lastAttackTime = Time.time;
        }
        if(lastAttackTime + 0.5f < Time.time && animationStop)
        {
            animator.SetBool("isAttack", false);
            animationStop = false;
        }
    }

    void ShootProjectile(RaycastHit hit, Ray ray)
    {
        var projectileObj = Instantiate(projectile, firePoint.position, Quaternion.identity);
        projectileObj.GetComponent<Rigidbody>().velocity = (hit.point - firePoint.position).normalized * enemyProjectileSpeed;
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

    public bool PlayerOnSigth()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, aimMask))
        {
            return !hit.transform.CompareTag("Obstacle");
        }
        return false;
    }

    public void StopUnit()
    {
        enemy.SetDestination(transform.position);
    }
    public void DestroyEnemy()
    {
        deadAnimationDuration = 2;
        Destroy(gameObject, deadAnimationDuration);
    }

    private void OnDestroy()
    {
        Main.EnemyDeath(transform.position);
    }



}
