using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCreatorIdleState : CreatureCreatorBaseState
{
    GameObject _hoveredObj;
    GameObject _hoveredMirroredObj;

    public override void EnterState(CreatureCreatorStateManager stateManager)
    {
        //reset the scene so there is no active object
        ResetActiveObject(stateManager);
    }
    public override void UpdateState(CreatureCreatorStateManager stateManager)
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = CastRay();
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("DraggableBodyPart"))
                {
                
                    _hoveredObj = hit.collider.gameObject;
                    _hoveredMirroredObj = hit.collider.gameObject.GetComponent<BodyPartDataHolder>().pairObj.gameObject;
                    //set tha aura on the objects
                    //set up a tooltip to show the objects stats after a few seconds over the object etc
                    if (Input.GetMouseButtonDown(0))
                    {
                        stateManager.GrabbedObj = _hoveredObj.transform;
                        stateManager.MirroredObj = _hoveredMirroredObj.transform;
                        stateManager.SwitchState(stateManager._partEditState);
                    }
                }
            }
        }
    }

    public override void ExitState(CreatureCreatorStateManager stateManager)
    {
        
    }

    //add this to a utilities script
    RaycastHit CastRay()
    {
        RaycastHit rayHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out rayHit);
        Debug.Log("raycast sent");
        return rayHit;
    }

    void ResetActiveObject(CreatureCreatorStateManager stateManager)
    {
        if (stateManager.GrabbedObj != null)
        {
            stateManager.GrabbedObj = null;
        }
        if (stateManager.MirroredObj != null)
        {
            stateManager.MirroredObj = null;
        }
        if(stateManager.BodyPartData != null)
        {
            stateManager.BodyPartData = null;
        }
    }
}
