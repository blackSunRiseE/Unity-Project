using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public override void EnterState(BaseMeleeEnemyAI stateControler)
    {
        stateControler.animator.SetBool("isIdle", true);
    }
    public override void UpdateState(BaseMeleeEnemyAI stateControler)
    {
        float distanceToPlayer = stateControler.GetDistanceToPlayer(stateControler.player.position);
        if (distanceToPlayer < stateControler.rangeToStopAct)
        {
            StateExit(stateControler.Chase, stateControler);
        }
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
    }

    public void StateExit(BaseState state, BaseMeleeEnemyAI stateControler)
    {
        stateControler.animator.SetBool("isIdle", false);
        stateControler.SwitchState(state);
    }
}
