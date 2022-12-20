using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnState : BaseBossState
{
    public override void EnterState(BaseBossAI stateControler)
    {
        stateControler.spawnTime = Time.time;
        stateControler.SpawnSpidelings();
    }

    public override void UpdateState(BaseBossAI stateControler)
    {
        if(stateControler.spawnTime + 1f < Time.time)
        {
            StateExit(stateControler.Chase, stateControler);
        }
    }

    public void StateExit(BaseBossState state, BaseBossAI stateControler)
    {
        stateControler.SwitchState(state);
    }
}
