using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackState : BaseRangeState
{
    public override void EnterState(RangeEnemyAI stateControler)
    {
        stateControler.lastAttackTime = Time.time;
    }
    public override void UpdateState(RangeEnemyAI stateControler)
    {
        float distanceToPlayer = stateControler.getDistanceToPlayer();
        
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
        else if (!stateControler.PlayerOnSigth())
        {
            StateExit(stateControler.Chase, stateControler);
        }
        else
        {
            stateControler.ShootPlayer();
        }
        
        
    }

    public void StateExit(BaseRangeState state, RangeEnemyAI stateControler)
    {
        stateControler.animator.SetBool("isAttack", false);
        stateControler.SwitchState(state);
    }
}
