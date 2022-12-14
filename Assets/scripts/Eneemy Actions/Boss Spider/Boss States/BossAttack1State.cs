using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack1State : BaseBossState
{
    // Start is called before the first frame update
    
    public override void EnterState(BaseBossAI stateControler)
    {
        stateControler.animator.SetBool("IsAttack1", true);
    }

    // Update is called once per frame
    public override void UpdateState(BaseBossAI stateControler)
    {
        float distanceToPlayer = stateControler.getDistanceToPlayer();
        stateControler.ShootPlayer();
        if (distanceToPlayer < 20)
        {
            StateExit(stateControler.Chase, stateControler);
        }

    }

    public void StateExit(BaseBossState state, BaseBossAI stateControler)
    {
        stateControler.animator.SetBool("IsAttack1", false);
        stateControler.SwitchState(state);
    }
}
