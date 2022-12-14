using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeadState : BaseBossState
{
    // Start is called before the first frame update
    public override void EnterState(BaseBossAI stateControler)
    {
    }

    // Update is called once per frame
    public override void UpdateState(BaseBossAI stateControler)
    {
    }

    public void StateExit(BaseBossState state, BaseBossAI stateControler)
    {
        stateControler.SwitchState(state);
    }
}
