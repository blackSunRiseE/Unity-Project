using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBossState 
{
    public abstract void EnterState(BaseBossAI stateControler);

    public abstract void UpdateState(BaseBossAI state);
}
