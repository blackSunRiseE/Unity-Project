using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BaseBossState
{
    public override void EnterState(BaseBossAI stateControler)
    {
        stateControler.animator.SetBool("IsIdle", true);
    }

    public override void UpdateState(BaseBossAI stateControler)
    {
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
        else if (distanceToPlayer < stateControler.rangeToStopAct)
        {
            StateExit(stateControler.Chase, stateControler);
        }
        
    }

    public void StateExit(BaseBossState state, BaseBossAI stateControler)
    {
        stateControler.animator.SetBool("IsIdle", false);
        stateControler.SwitchState(state);
    }
}
