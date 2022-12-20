using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRangeAttackState : BaseBossState
{
    public override void EnterState(BaseBossAI stateControler)
    {
        stateControler.animator.SetBool("rangeAttack", true);
        stateControler.rangeAttackTime = Time.time;
    }

    public override void UpdateState(BaseBossAI stateControler)
    {
        if (stateControler.rangeAttackTime + 0.5f < Time.time && !stateControler.isShoot)
        {
            stateControler.ShootPlayer();
            stateControler.isShoot = true;
        }
        if (stateControler.rangeAttackTime + 2f < Time.time)
        {
            stateControler.isShoot = false;
            StateExit(stateControler.prevState, stateControler);
        }
    }

    public void StateExit(BaseBossState state, BaseBossAI stateControler)
    {
        stateControler.animator.SetBool("rangeAttack", false);
        stateControler.SwitchState(state);
    }
}
