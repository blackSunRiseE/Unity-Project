using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGetHitState : BaseBossState
{
    float lastDamagedTime;
    public override void EnterState(BaseBossAI stateControler)
    {
        lastDamagedTime = Time.time;
        //stateControler.stopUnit();
        Debug.Log("Damaged");
        stateControler.animator.SetBool("IsDamaged", true);
    }

    // Update is called once per frame
    public override void UpdateState(BaseBossAI stateControler)
    {
        if (lastDamagedTime + stateControler.animationDuration < Time.time)
        {
            if (stateControler.health <= 0)
            {
                StateExit(stateControler.Dead, stateControler);
            }
            Debug.Log("exit");
            StateExit(stateControler.prevState, stateControler);
        }
    }

    public void StateExit(BaseBossState state, BaseBossAI stateControler)
    {
        stateControler.animator.SetBool("IsDamaged", false);
        stateControler.getHit = false;
        stateControler.SwitchState(state);
    }
}
