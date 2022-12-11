using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    BaseRangeState currentState;
    [HideInInspector] public BaseRangeState prevState;

    [HideInInspector] public RangeIdleState Idle = new RangeIdleState();
    [HideInInspector] public RangeAttackState Attack = new RangeAttackState();
    [SerializeField] GameObject projectile;
    [SerializeField] private Transform firePoint;

    private float lastAttackTime;

    [HideInInspector] public Animator animator;
    [HideInInspector] public float animationDuration;

    [HideInInspector] public bool getHit = false;

    // Start is called before the first frame update
    void Start()
    {
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
            Debug.Log("attack");
            lastAttackTime = Time.time;
        }
    }

    void ShootProjectile(RaycastHit hit, Ray ray)
    {
        var projectileObj = Instantiate(projectile, firePoint.position, Quaternion.identity);
        projectileObj.GetComponent<Rigidbody>().velocity = (hit.point - firePoint.position).normalized * enemyProjectileSpeed;
    }

    public void dealDamage()
    {
        
    }

    public override void TakeDamage(float damage)
    {
        animationDuration = animator.runtimeAnimatorController.animationClips[2].length;
        getHit = true;
        Hp -= damage;
    }
}
