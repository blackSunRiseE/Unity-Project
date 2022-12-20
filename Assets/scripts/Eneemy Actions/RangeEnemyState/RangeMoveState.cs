using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeMoveState : BaseRangeState
{
    // Start is called before the first frame update
    public override void EnterState(RangeEnemyAI stateControler)
    {
        stateControler.animator.SetBool("isChase",true);
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
        stateControler.animator.SetBool("isChase", false);
        stateControler.SwitchState(state);
    }
}
