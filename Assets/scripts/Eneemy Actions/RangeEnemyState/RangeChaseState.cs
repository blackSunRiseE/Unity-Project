using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeChaseState : BaseRangeState
{
    // Start is called before the first frame update
    public override void EnterState(RangeEnemyAI stateControler)
    {
        stateControler.animator.SetBool("isChase", true);
    }
    public override void UpdateState(RangeEnemyAI stateControler)
    {
        float distanceToPlayer = stateControler.GetDistanceToPlayer(stateControler.player.position);
        stateControler.ChasePlayer(stateControler.player, stateControler.moveSpeed);
        if (stateControler.getHit)
        {
            stateControler.StopUnit();
            if (stateControler.health <= 0)
            {
                StateExit(stateControler.Dead, stateControler);
            }
            else
            {
                StateExit(stateControler.Damaged, stateControler);
            }
        }
        if (distanceToPlayer > stateControler.rangeToStopAct)
        {
            stateControler.StopUnit();
            StateExit(stateControler.Idle, stateControler);
        }
        else if (stateControler.PlayerOnSigth())
        {
            stateControler.StopUnit();
            StateExit(stateControler.Attack, stateControler);
        }
        
    }

    public void StateExit(BaseRangeState state, RangeEnemyAI stateControler)
    {
        stateControler.animator.SetBool("isChase", false);
        stateControler.SwitchState(state);
    }
}
