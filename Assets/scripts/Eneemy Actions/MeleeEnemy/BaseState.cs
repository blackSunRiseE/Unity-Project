using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    // Start is called before the first frame update
    public abstract void EnterState(BaseMeleeEnemyAI stateControler);

    // Update is called once per frame
    public abstract void UpdateState(BaseMeleeEnemyAI state);
}
