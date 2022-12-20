using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedState : BaseState
{
    float lastDamagedTime;
    public override void EnterState(BaseMeleeEnemyAI stateControler)
    {
        
        stateControler.StopUnit(stateControler.transform.position);
        lastDamagedTime = Time.time;
        stateControler.animator.SetBool("isDamaged", true);
    }

    public override void UpdateState(BaseMeleeEnemyAI stateControler)
    {
        
        if (lastDamagedTime + stateControler.damagedAnimationDuration < Time.time)
        {
            StateExit(stateControler.prevState, stateControler);
        }
    }

    public void StateExit(BaseState state, BaseMeleeEnemyAI stateControler)
    {
        stateControler.animator.SetBool("isDamaged", false);
        stateControler.getHit = false;
        stateControler.SwitchState(state);
    }
}
