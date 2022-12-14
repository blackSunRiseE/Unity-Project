using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChaseState : BaseBossState
{
    public override void EnterState(BaseBossAI stateControler)
    {
        stateControler.animator.SetBool("IsChase", true);
    }

    // Update is called once per frame
    public override void UpdateState(BaseBossAI stateControler)
    {
        float distanceToPlayer = stateControler.getDistanceToPlayer();
        stateControler.ChasePlayer();
        if(distanceToPlayer < stateControler.attackRange)
        {
            StateExit(stateControler.Attack2, stateControler);
        }
    }

    public void StateExit(BaseBossState state, BaseBossAI stateControler)
    {
        stateControler.animator.SetBool("IsChase", false);
        stateControler.SwitchState(state);
    }
}
