using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    // Start is called before the first frame update
    public override void EnterState(BaseMeleeEnemyAI stateControler)
    {
        Debug.Log("attack");
        stateControler.animator.SetBool("isAttack", true);
        // attack animation
    }
    public override void UpdateState(BaseMeleeEnemyAI stateControler)
    {
        
        stateControler.dealDamage();
        float distanceToPlayer = stateControler.getDistanceToPlayer();

        if (stateControler.Hp <= 0)
        {
            StateExit(stateControler.Dead, stateControler);
        }
        if (distanceToPlayer > stateControler.attackRange)
        {
            Debug.Log("melee Attack");
            StateExit(stateControler.Chase, stateControler);
        }
    }

    public void StateExit(BaseState state, BaseMeleeEnemyAI stateControler)
    {
        stateControler.animator.SetBool("isAttack", false);
        stateControler.SwitchState(state);
    }
}
