using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeIdleState : BaseRangeState
{
    // Start is called before the first frame update
    public override void EnterState(RangeEnemyAI stateControler)
    {
        Debug.Log("RangeIdle");
    }
    public override void UpdateState(RangeEnemyAI stateControler)
    {
        float distanceToPlayer = stateControler.getDistanceToPlayer();
        if (distanceToPlayer < stateControler.attackRange)
        {
            StateExit(stateControler.Attack, stateControler);
        }
    }

    public void StateExit(BaseRangeState state, RangeEnemyAI stateControler)
    {
        stateControler.SwitchState(state);
    }
}
