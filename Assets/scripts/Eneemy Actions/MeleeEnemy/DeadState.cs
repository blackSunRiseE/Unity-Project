using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : BaseState
{
    // Start is called before the first frame update
    public override void EnterState(BaseMeleeEnemyAI stateControler)
    {
        stateControler.animator.SetBool("isDead", true);
        //start idle animation

    }
    public override void UpdateState(BaseMeleeEnemyAI stateControler)
    {
    }

    public void StateExit(BaseState state, BaseMeleeEnemyAI stateControler)
    {
    }
}
