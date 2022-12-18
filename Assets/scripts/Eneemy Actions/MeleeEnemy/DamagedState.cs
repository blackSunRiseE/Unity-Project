using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedState : BaseState
{
    float lastDamagedTime;
    public override void EnterState(BaseMeleeEnemyAI stateControler)
    {
        lastDamagedTime = Time.time;
        stateControler.stopUnit();
        Debug.Log("Damaged");
        stateControler.animator.SetBool("isDamaged",true);
        //start idle animation

    }
    public override void UpdateState(BaseMeleeEnemyAI stateControler)
    {
        
        if (lastDamagedTime + stateControler.damagedAnimationDuration < Time.time)
        {
            if(stateControler.Hp <= 0)
            {
                StateExit(stateControler.Dead, stateControler);
            }
            Debug.Log("exit");
            StateExit(stateControler.prevState, stateControler);
        }
        
    }

    public void StateExit(BaseState state, BaseMeleeEnemyAI stateControler)
    {
        stateControler.animator.SetBool("isDamaged", false);
        stateControler.getHit = false;
        stateControler.SwitchState(state);
    }
}
