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
        if (stateControler.PlayerOnSigth())
        {
            StateExit(stateControler.Attack, stateControler);
        }
        else
        {
            StateExit(stateControler.Chase, stateControler);
        }
        
    }

    public void StateExit(BaseRangeState state, RangeEnemyAI stateControler)
    {
        stateControler.SwitchState(state);
    }
}
