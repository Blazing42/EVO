using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class that is the base class for all of the creature creator scene states to inherit from
/// </summary>
public abstract class CreatureCreatorBaseState 
{
    public abstract void EnterState(CreatureCreatorStateManager stateManager);

    public abstract void UpdateState(CreatureCreatorStateManager stateManager);

    public abstract void ExitState(CreatureCreatorStateManager stateManager);

}
