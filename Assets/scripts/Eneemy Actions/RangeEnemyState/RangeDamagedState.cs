using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDamagedState : BaseRangeState
{
    float lastDamagedTime;
    public override void EnterState(RangeEnemyAI stateControler)
    {
        stateControler.StopUnit();
        lastDamagedTime = Time.time;
        stateControler.animator.SetBool("isDamaged",true);
    }
    public override void UpdateState(RangeEnemyAI stateControler)
    {
        if (lastDamagedTime + stateControler.damagedAnimationDuration < Time.time)
        {
            StateExit(stateControler.prevState, stateControler);
        }

    }

    public void StateExit(BaseRangeState state, RangeEnemyAI stateControler)
    {
        stateControler.animator.SetBool("isDamaged", false);
        stateControler.getHit = false;
        stateControler.SwitchState(state);
    }
}
