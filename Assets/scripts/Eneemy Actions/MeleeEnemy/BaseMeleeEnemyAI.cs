using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseMeleeEnemyAI : BaseEnemyAI
{
    [HideInInspector] public Transform player;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float attackRange = 2;
    [SerializeField] public float chaseRange = 10;
    [SerializeField] public float attackDelay = 1;
    [SerializeField] public float attackDamage = 10;
    

    BaseState currentState;
    [HideInInspector] public BaseState prevState;

    [HideInInspector] public IdleState Idle = new IdleState();
    [HideInInspector] public ChaseState Chase = new ChaseState();
    [HideInInspector] public AttackState Attack = new AttackState();
    [HideInInspector] public DamagedState Damaged = new DamagedState();
    [HideInInspector] public DeadState Dead = new DeadState();
    
    void Start()
    {
        health = maxHealth;
        player = GameObject.FindWithTag("Player").transform;
        enemy = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        SwitchState(Idle);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }
  

    public void SwitchState(BaseState state)
    {
        prevState = currentState;
        currentState = state;
        currentState.EnterState(this);
    }
    public float GetDistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.position);
    }

    public void StopUnit()
    {
        enemy.SetDestination(transform.position);
    }

    public void LookAtPlayer()
    {
        Vector3 playerPosition = new Vector3(player.position.x,
                                             transform.position.y,
                                             player.position.z);
        transform.LookAt(playerPosition);
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
