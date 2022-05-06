using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that controls the Creature Creator Body Part Snap State
/// </summary>
public class CreatureCreatorBodyPartSnapState : CreatureCreatorBaseState
{
    Transform grabbedObject;
    Transform grabbedMirroredObject;

    //Method that is called when this state is entered
    public override void EnterState(CreatureCreatorStateManager stateManager)
    {
        MouseOverBody(stateManager);
    }

    //Method that controls what happens each time the update is called in this state
    public override void UpdateState(CreatureCreatorStateManager stateManager)
    {
        //When the mouse button is held down a raycast is sent out to check if the players mouse is still over the creature
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit = CastRay();
            if(hit.collider != null && (hit.collider.CompareTag("MainBody") || hit.collider.CompareTag("DraggableBodyPart")))
            { 
                
                if (grabbedObject != null && grabbedMirroredObject != null)
                {
                    //if the mouse is still over the the creature and the player has a grabbed object, align the grabbed object to the surface of the creature under the mouse
                    grabbedObject.position = hit.point;
                    grabbedObject.rotation = Quaternion.Lerp(grabbedObject.rotation, Quaternion.LookRotation(hit.normal), Time.deltaTime * 5f);
                    //if the surface of the creature under the mouse is near the mirror x=0 line, remove the mirrored body part and snap the position of the grabbed part to the center
                    if(grabbedObject.position.x < 0.06f && grabbedObject.position.x > -0.06f)
                    {
                        grabbedObject.position = new Vector3(0f, grabbedObject.position.y, grabbedObject.position.z);
                        grabbedMirroredObject.gameObject.SetActive(false);
                    }
                    else
                    {
                        //if the surface of the creature under the mouse isnt near the center x=0 line, create a mirrored body part of the grabbed body part and/or update its position
                        if(grabbedMirroredObject.gameObject.activeInHierarchy == false)
                        {
                            grabbedMirroredObject.gameObject.SetActive(true);
                        }
                        
                        grabbedMirroredObject.position = new Vector3(-grabbedObject.transform.position.x, grabbedObject.transform.position.y, grabbedObject.transform.position.z);
                        grabbedMirroredObject.localRotation = new Quaternion(grabbedObject.transform.localRotation.x * -1.0f,
                        grabbedObject.transform.localRotation.y,
                        grabbedObject.transform.localRotation.z,
                        grabbedObject.transform.localRotation.w * -1.0f);

                    }
                }
            }
            //if the mouse is held down but the mouse is no longer over the creature switch to the Drag state
            else 
            {
                stateManager.SwitchState(stateManager._partDragState);
            }
        }
        //if the mouse button is no longer held down, ensure that the grabbed body part and its mirror can be interacted with, in the default layer,
        //before switching to the Body Part Edit State
        else
        {
            grabbedObject.gameObject.layer = 0;
            grabbedMirroredObject.gameObject.layer = 0;
            stateManager.SwitchState(stateManager._partEditState);
        }
    }

    //Method that is called when this state is exited 
    public override void ExitState(CreatureCreatorStateManager stateManager)
    {}

    //Method that sends out a raycast from the mouse position into the screen using the cameras forward vector
    //later needs to be put in a utilities class 
    private RaycastHit CastRay()
    {
        RaycastHit rayHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out rayHit);
        return rayHit;
    }

    //Method that sets the grabbed object and the grabbed mirrored object class variables to match the objects stored in the state manager
    void MouseOverBody(CreatureCreatorStateManager stateManager)
    {
        grabbedObject = stateManager.GrabbedObj;
        grabbedMirroredObject = stateManager.MirroredObj;
    }
}
