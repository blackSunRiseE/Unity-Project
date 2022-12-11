using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackState : BaseRangeState
{
    public override void EnterState(RangeEnemyAI stateControler)
    {
        Debug.Log("Attack");
        //animator
    }
    public override void UpdateState(RangeEnemyAI stateControler)
    {
        stateControler.ShootPlayer();
    }

    public void StateExit(BaseState state, BaseEnemyAI stateControler)
    {
    }
}
