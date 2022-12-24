using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeIdleState : BaseRangeState
{
    // Start is called before the first frame update
    public override void EnterState(RangeEnemyAI stateControler)
    {
    }
    public override void UpdateState(RangeEnemyAI stateControler)
    {
        float distanceToPlayer = stateControler.GetDistanceToPlayer(stateControler.player.position);
        if (distanceToPlayer < stateControler.rangeToStopAct)
        {

            if (stateControler.PlayerOnSigth())
            {
                StateExit(stateControler.Attack, stateControler);
            }
            else
            {
                StateExit(stateControler.Chase, stateControler);
            }
        }
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
    }

    public void StateExit(BaseRangeState state, RangeEnemyAI stateControler)
    {
        stateControler.SwitchState(state);
    }
}
