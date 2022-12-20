using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    public override void EnterState(BaseMeleeEnemyAI stateControler)
    {
        stateControler.animator.SetBool("isAttack", true);
    }
    public override void UpdateState(BaseMeleeEnemyAI stateControler)
    {
        stateControler.LookAtPlayer();
        float distanceToPlayer = stateControler.GetDistanceToPlayer();

        if (stateControler.getHit)
        {
            if (stateControler.health <= 0)
            {
                StateExit(stateControler.Dead, stateControler);
            }
            else
            {
                StateExit(stateControler.Damaged, stateControler);
            }
        }
        if (distanceToPlayer > stateControler.attackRange)
        {
            StateExit(stateControler.Chase, stateControler);
        }
    }

    public void StateExit(BaseState state, BaseMeleeEnemyAI stateControler)
    {
        stateControler.animator.SetBool("isAttack", false);
        stateControler.SwitchState(state);
    }
}
