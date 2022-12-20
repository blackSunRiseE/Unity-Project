using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDeadState : BaseRangeState
{
    public override void EnterState(RangeEnemyAI stateControler)
    {
        stateControler.StopUnit();
        stateControler.animator.SetBool("isDead", true);
        stateControler.DestroyEnemy();
    }
    public override void UpdateState(RangeEnemyAI stateControler)
    {
    }

    public void StateExit(BaseRangeState state, RangeEnemyAI stateControler)
    {
        stateControler.SwitchState(state);
    }
}
