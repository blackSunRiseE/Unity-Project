using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeMoveState : BaseRangeState
{
    // Start is called before the first frame update
    public override void EnterState(RangeEnemyAI stateControler)
    {
        Debug.Log("Attack");
        //animator
    }
    public override void UpdateState(RangeEnemyAI stateControler)
    {
    }

    public void StateExit(BaseState state, BaseEnemyAI stateControler)
    {
    }
}
