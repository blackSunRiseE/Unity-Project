using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BaseState
{
    // Start is called before the first frame update
    public override void EnterState(BaseMeleeEnemyAI stateControler)
    {
        Debug.Log("Chase");
        stateControler.animator.SetBool("isChase", true);
        // chase animations
    }
    public override void UpdateState(BaseMeleeEnemyAI stateControler)
    {
        stateControler.chasePlayer(stateControler.player, stateControler.moveSpeed);
        float distanceToPlayer = stateControler.getDistanceToPlayer();
        if (stateControler.getHit)
        {
            StateExit(stateControler.Damaged,stateControler);
        }
        if (distanceToPlayer > stateControler.chaseRange)
        {
            StateExit(stateControler.Idle, stateControler);
        }
        else if(distanceToPlayer < stateControler.attackRange)
        {
            StateExit(stateControler.Attack, stateControler);
        }
        
    }

    public void StateExit(BaseState state, BaseMeleeEnemyAI stateControler)
    {
        stateControler.animator.SetBool("isChase", false);
        stateControler.SwitchState(state);
    }
    

}
