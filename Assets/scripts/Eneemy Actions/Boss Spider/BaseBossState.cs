using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBossState 
{
    // Start is called before the first frame update
    public abstract void EnterState(BaseBossAI stateControler);

    // Update is called once per frame
    public abstract void UpdateState(BaseBossAI state);
}
