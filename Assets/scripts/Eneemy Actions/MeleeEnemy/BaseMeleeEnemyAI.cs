using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMeleeEnemyAI : BaseEnemyAI
{
    [SerializeField] public Transform player;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float attackRange = 1;
    [SerializeField] public float chaseRange = 10;
    [SerializeField] public float attackDelay = 1;
    [SerializeField] public float attackDamage = 10;
    [SerializeField] public float Hp = 50;

    BaseState currentState;
    [HideInInspector] public BaseState prevState;

    [HideInInspector] public IdleState Idle = new IdleState();
    [HideInInspector] public ChaseState Chase = new ChaseState();
    [HideInInspector] public AttackState Attack = new AttackState();
    [HideInInspector] public DamagedState Damaged = new DamagedState();
    [HideInInspector] public DeadState Dead = new DeadState();

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
  

    public void SwitchState(BaseState state)
    {
        prevState = currentState;
        currentState = state;
        currentState.EnterState(this);
    }
    public float getDistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.position);
    }

    public void chasePlayer(Transform player, float moveSpeed)
    {
        Vector3 playerPosition = new Vector3(player.position.x,
                                             transform.position.y,
                                             player.position.z);
        transform.LookAt(playerPosition);
        transform.Translate(0, 0, moveSpeed * Time.deltaTime);
    }

    public void findEnemies()
    {
    }

    public void dealDamage()
    {
        Vector3 playerPosition = new Vector3(player.position.x,
                                             transform.position.y,
                                             player.position.z);
        transform.LookAt(playerPosition);
        PlayerTarget target = player.GetComponent <PlayerTarget>();
        if(lastAttackTime + attackDelay < Time.time)
        {
            Debug.Log("attack");
            target.TakeDamage(attackDamage);
            lastAttackTime = Time.time;
        }
    }

    public override void TakeDamage(float damage)
    {
        animationDuration = animator.runtimeAnimatorController.animationClips[2].length;
        getHit = true;
        Hp -= damage;
    }
}
