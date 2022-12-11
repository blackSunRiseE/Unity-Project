using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseRangeState 
{
    // Start is called before the first frame update
    public abstract void EnterState(RangeEnemyAI stateControler);

    // Update is called once per frame
    public abstract void UpdateState(RangeEnemyAI state);
}
