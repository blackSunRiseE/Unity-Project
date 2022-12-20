using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : BaseState
{
    public override void EnterState(BaseMeleeEnemyAI stateControler)
    {
        stateControler.StopUnit();
        stateControler.animator.SetBool("isDead", true);
        stateControler.DestroyEnemy();

    }
    public override void UpdateState(BaseMeleeEnemyAI stateControler)
    {
    }

    public void StateExit(BaseState state, BaseMeleeEnemyAI stateControler)
    {
    }
}
