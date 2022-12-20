using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseBossAI : BaseEnemyAI
{
    [SerializeField] LayerMask aimMask;
    BaseBossState currentState;
    [HideInInspector] public BaseBossState prevState;
    [HideInInspector] public Transform player;
    [HideInInspector] public float attackRange = 5f;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float chaseRange = 4f;
    [SerializeField] public float attackDelay = 1f;
    [SerializeField] public float attackDamage = 10f;
    [HideInInspector] public float spawnTime;
    Vector3 startPosition;
    
    private float lastAttackTime;
    [HideInInspector] public float rangeAttackTime;
    [HideInInspector] public float rangeAttackDelay = 10f;
    [HideInInspector] public bool isShoot = false;
    [HideInInspector] public bool isSpawn = false;

    [HideInInspector] public BossIdleState Idle = new BossIdleState();
    [HideInInspector] public BossMeleeAttackState MeleeAttack = new BossMeleeAttackState();
    [HideInInspector] public BossRangeAttackState RangeAttack = new BossRangeAttackState();
    [HideInInspector] public BossDeadState Dead = new BossDeadState();
    [HideInInspector] public BossChaseState Chase = new BossChaseState();
    [HideInInspector] public BossGetHitState Damaged = new BossGetHitState();
    [HideInInspector] public BossSpawnState Spawn = new BossSpawnState();
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject spiderlingsPrefab;
    [SerializeField] Transform spawnPosition;
    [SerializeField] private Transform firePoint;
    [SerializeField] public float enemyProjectileSpeed = 50f;


    void Start()
    {
        maxHealth = 200;
        player = GameObject.FindWithTag("Player").transform;
        enemy = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        rangeAttackTime = Time.time;
        health = maxHealth;
        SwitchState(Idle);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(BaseBossState state)
    {
        prevState = currentState;
        currentState = state;
        currentState.EnterState(this);
    }
    public float GetDistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.position);
    }

    public void ShootPlayer()
    {
        Vector3 playerPosition = new Vector3(player.position.x,
                                             transform.position.y,
                                             player.position.z);
        transform.LookAt(playerPosition);
        
        for(int i = 0; i < 10; i++)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            direction = Quaternion.Euler(0, -36*i, 0) * direction;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, aimMask))
            {
                ShootProjectile(hit, ray);
            }
        }
        

    }

    void ShootProjectile(RaycastHit hit, Ray ray)
    {
        var projectileObj = Instantiate(projectile, firePoint.position, Quaternion.identity);
        projectileObj.GetComponent<Rigidbody>().velocity = (hit.point - firePoint.position).normalized * enemyProjectileSpeed;
    }

    public void SpawnSpidelings()
    {
        for(int i = 0; i < 10; i++)
        {
            
            Instantiate(spiderlingsPrefab, new Vector3(spawnPosition.position.x + Random.Range(1f,4f), spawnPosition.position.y, spawnPosition.position.z + Random.Range(1f, 4f)) , Quaternion.identity);
        }
        
    }

    public void DealDamage(Transform selfPosition, Vector3 playerPosition,float lastAttackTime,float attackDelayTime,float attackDamage)
    {
        Vector3 playerPos = new Vector3(playerPosition.x,
                                             selfPosition.position.y,
                                             playerPosition.z);
        transform.LookAt(playerPos);
        PlayerTarget target = player.GetComponent<PlayerTarget>();
        if (lastAttackTime + attackDelayTime < Time.time)
        {
            target.TakeDamage(attackDamage);
            lastAttackTime = Time.time;
        }
    }

    private void OnDestroy()
    {
        Main.EnemyDeath(transform.position);
    }

    
}
