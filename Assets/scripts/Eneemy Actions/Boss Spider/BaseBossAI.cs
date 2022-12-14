using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseBossAI : BaseEnemyAI
{
    BaseBossState currentState;
    [HideInInspector] public BaseBossState prevState;
    [HideInInspector] public Animator animator;
    [SerializeField] public Transform player;
    [HideInInspector] public bool getHit = false;
    [HideInInspector] public float animationDuration;
    [HideInInspector] public float attackRange = 4;

    private NavMeshAgent enemy;

    [HideInInspector] public float health = 500;


    [HideInInspector] public BossIdleState Idle = new BossIdleState();
    [HideInInspector] public BossAttack1State Attack1 = new BossAttack1State();
    [HideInInspector] public BossAttack1State Attack2 = new BossAttack1State();
    [HideInInspector] public BossDeadState Dead = new BossDeadState();
    [HideInInspector] public BossChaseState Chase = new BossChaseState();
    [HideInInspector] public BossGetHitState GetHit = new BossGetHitState();

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

    public void SwitchState(BaseBossState state)
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

    }

    public void ChasePlayer()
    {
        Vector3 playerPosition = new Vector3(player.position.x,
                                             transform.position.y,
                                             player.position.z);
        enemy.SetDestination(playerPosition);
    }

    public override void TakeDamage(float damage)
    {
        animationDuration = animator.runtimeAnimatorController.animationClips[2].length;
        getHit = true;
        health -= damage;
    }
}
