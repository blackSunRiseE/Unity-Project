using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeadState : BaseBossState
{
    public override void EnterState(BaseBossAI stateControler)
    {
        stateControler.StopUnit(stateControler.transform.position);
        stateControler.animator.SetBool("IsDead", true);
        stateControler.DestroyEnemy(stateControler.gameObject);
        GameObject.FindGameObjectWithTag("Spawner").GetComponent<Main>().gameOver = true;
        GameObject.FindGameObjectWithTag("Spawner").GetComponent<Main>().gameWin = true;
    }

    public override void UpdateState(BaseBossAI stateControler)
    {
    }

    public void StateExit(BaseBossState state, BaseBossAI stateControler)
    {
        stateControler.SwitchState(state);
    }
}
