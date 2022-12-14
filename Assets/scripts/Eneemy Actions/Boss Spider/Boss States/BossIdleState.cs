using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BaseBossState
{
    public override void EnterState(BaseBossAI stateControler)
    {
        Debug.Log("Idle");
        stateControler.animator.SetBool("IsIdle", true);
    }

    // Update is called once per frame
    public override void UpdateState(BaseBossAI stateControler)
    {
        float distanceToPlayer = stateControler.getDistanceToPlayer();
        if (stateControler.health < 300)
        {
            StateExit(stateControler.Attack1,stateControler);
        }
        if (stateControler.getHit)
        {
            StateExit(stateControler.GetHit, stateControler);
        }
        if(distanceToPlayer > 20)
        {
            StateExit(stateControler.Attack1,stateControler);
        }
    }

    public void StateExit(BaseBossState state, BaseBossAI stateControler)
    {
        stateControler.animator.SetBool("IsIdle", false);
        stateControler.SwitchState(state);
    }
}
