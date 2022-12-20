using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    public abstract void EnterState(BaseMeleeEnemyAI stateControler);

    public abstract void UpdateState(BaseMeleeEnemyAI state);
}
