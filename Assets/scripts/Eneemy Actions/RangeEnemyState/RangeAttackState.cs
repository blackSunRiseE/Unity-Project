using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackState : BaseRangeState
{
    public override void EnterState(RangeEnemyAI stateControler)
    {
        Debug.Log("Attack");
        //animator
    }
    public override void UpdateState(RangeEnemyAI stateControler)
    {
        float distanceToPlayer = stateControler.getDistanceToPlayer();
        stateControler.ShootPlayer();
        if (distanceToPlayer < stateControler.enemyRangeToRun)
        {
            stateControler.GetPlayerState();
            if (stateControler.weaponState == WeaponState.MeleeWeapon)
            {
                StateExit(stateControler.Move, stateControler);
            }
            else
            {
                StateExit(stateControler.Strafe, stateControler);
            }
        }
    }

    public void StateExit(BaseRangeState state, RangeEnemyAI stateControler)
    {
        stateControler.SwitchState(state);
    }
}
