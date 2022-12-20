using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGetHitState : BaseBossState
{
    float lastDamagedTime;
    public override void EnterState(BaseBossAI stateControler)
    {
        stateControler.StopUnit(stateControler.transform.position);
        lastDamagedTime = Time.time;
        stateControler.animator.SetBool("IsDamaged", true);
    }
    public override void UpdateState(BaseBossAI stateControler)
    {
        if (lastDamagedTime + stateControler.damagedAnimationDuration < Time.time)
        {
            if (stateControler.health <= stateControler.maxHealth / 2 && !stateControler.isSpawn)
            {
                stateControler.isSpawn = true;
                StateExit(stateControler.Spawn, stateControler);
            }
            else
            {
                StateExit(stateControler.prevState, stateControler);
            }
        }
    }

    public void StateExit(BaseBossState state, BaseBossAI stateControler)
    {
        stateControler.animator.SetBool("IsDamaged", false);
        stateControler.getHit = false;
        stateControler.SwitchState(state);
    }
}
