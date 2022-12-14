using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeMoveState : BaseRangeState
{
    // Start is called before the first frame update
    public override void EnterState(RangeEnemyAI stateControler)
    {
        Debug.Log("Move");
        //animator
    }
    public override void UpdateState(RangeEnemyAI stateControler)
    {
        float distanceToPlayer = stateControler.getDistanceToPlayer();
        stateControler.RunAwayFromPlayer();
        if (distanceToPlayer > stateControler.enemyRangeToRun)
        {
            StateExit(stateControler.Attack, stateControler);
        }
    }

    public void StateExit(BaseRangeState state, RangeEnemyAI stateControler)
    {
        stateControler.SwitchState(state);
    }
}
