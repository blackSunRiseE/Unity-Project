using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChaseState : BaseBossState
{
    public override void EnterState(BaseBossAI stateControler)
    {
        stateControler.animator.SetBool("IsChase", true);
    }

    public override void UpdateState(BaseBossAI stateControler)
    {
        float distanceToPlayer = stateControler.GetDistanceToPlayer();
        stateControler.ChasePlayer(stateControler.player,stateControler.moveSpeed);
        if (distanceToPlayer < stateControler.attackRange)
        {
            stateControler.StopUnit(stateControler.transform.position);
            StateExit(stateControler.MeleeAttack, stateControler);
        }
        if (stateControler.getHit)
        {
            if (stateControler.health <= 0)
            {
                StateExit(stateControler.Dead, stateControler);
            }
            else
            {
                StateExit(stateControler.Damaged, stateControler);
            }
        }
        if (stateControler.rangeAttackTime + stateControler.rangeAttackDelay < Time.time)
        {
            stateControler.StopUnit(stateControler.transform.position);
            StateExit(stateControler.RangeAttack, stateControler);
        }
        
    }

    public void StateExit(BaseBossState state, BaseBossAI stateControler)
    {
        stateControler.animator.SetBool("IsChase", false);
        stateControler.SwitchState(state);
    }
}
