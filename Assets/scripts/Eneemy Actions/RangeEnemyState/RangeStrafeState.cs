using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeStrafeState : BaseRangeState
{
    public override void EnterState(RangeEnemyAI stateControler)
    {
        Debug.Log("Strafe");
        stateControler.getPositionBeforeStrafe();
        //animator
    }
    public override void UpdateState(RangeEnemyAI stateControler)
    {
        stateControler.StrafeFromPlayer();
        StateExit(stateControler.Idle,stateControler);
    }

    public void StateExit(BaseRangeState state, RangeEnemyAI stateControler)
    {
        stateControler.SwitchState(state);
    }
}
