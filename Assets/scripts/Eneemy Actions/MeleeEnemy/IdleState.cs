using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public override void EnterState(BaseMeleeEnemyAI stateControler)
    {
        stateControler.animator.SetBool("isIdle", true);

    }
    public override void UpdateState(BaseMeleeEnemyAI stateControler)
    {
        StateExit(stateControler.Chase, stateControler);
    }

    public void StateExit(BaseState state, BaseMeleeEnemyAI stateControler)
    {
        stateControler.animator.SetBool("isIdle", false);
        stateControler.SwitchState(state);
    }
}
