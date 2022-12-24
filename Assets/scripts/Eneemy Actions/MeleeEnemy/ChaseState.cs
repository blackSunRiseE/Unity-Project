using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BaseState
{
    public override void EnterState(BaseMeleeEnemyAI stateControler)
    {
        stateControler.animator.SetBool("isChase", true);
    }

    public override void UpdateState(BaseMeleeEnemyAI stateControler)
    {
        stateControler.ChasePlayer(stateControler.player, stateControler.moveSpeed);
        float distanceToPlayer = stateControler.GetDistanceToPlayer(stateControler.player.position);
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
        else if (distanceToPlayer > stateControler.rangeToStopAct)
        {
            stateControler.StopUnit();
            StateExit(stateControler.Idle, stateControler);
        }
        else if(distanceToPlayer < stateControler.attackRange)
        {
            stateControler.StopUnit();
            StateExit(stateControler.Attack, stateControler);
        }
        
    }

    public void StateExit(BaseState state, BaseMeleeEnemyAI stateControler)
    {
        stateControler.animator.SetBool("isChase", false);
        stateControler.SwitchState(state);
    }
}
