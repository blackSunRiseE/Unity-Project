using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeleeAttackState : BaseBossState
{
    public override void EnterState(BaseBossAI stateControler)
    {
        stateControler.animator.SetBool("meleeAttack", true);
        
    }

    public override void UpdateState(BaseBossAI stateControler)
    {
        stateControler.LookAtPlayer(stateControler.transform,stateControler.player.position);
        float distanceToPlayer = stateControler.GetDistanceToPlayer();
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
        if (distanceToPlayer > stateControler.attackRange)
        {
            StateExit(stateControler.Chase, stateControler);
        }
        if (stateControler.rangeAttackTime + stateControler.rangeAttackDelay < Time.time)
        {
            StateExit(stateControler.RangeAttack, stateControler);
        }
    }

    public void StateExit(BaseBossState state, BaseBossAI stateControler)
    {
        stateControler.animator.SetBool("meleeAttack", false);
        stateControler.SwitchState(state);
    }
}
