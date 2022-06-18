using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that controls the Creature Creator Body Part Drag State
/// </summary>
public class CreatureCreatorBodyPartDragState : CreatureCreatorBaseState
{
    Transform grabbedObject;
    Transform grabbedMirroredObject;
    float distanceFromCamera;

    //Method that is called when this state is entered
    public override void EnterState(CreatureCreatorStateManager stateManager)
    {
        SetUpObjectEditing(stateManager.GrabbedObj, stateManager.MirroredObj);
        //makes sure the dragged object is always a suitable distance from the camera depending on how zoomed in the player is
        distanceFromCamera = Vector3.Distance(Camera.main.transform.position, Vector3.zero)/3;
    }
    
    //Method that controls what happens each time the update is called in this state
    public override void UpdateState(CreatureCreatorStateManager stateManager)
    {
        //while the mouse button is held down drag the grabbed object and its mirror around the scene
        if (Input.GetMouseButton(0))
        {
            if (grabbedObject != null)
            {
                RaycastHit hit = CastRay();
                //if the player drags the object over the creature, change the state to the Body Part Snap State
                if (hit.collider != null && (hit.collider.CompareTag("MainBody") || hit.collider.CompareTag("DraggableBodyPart")))
                {
                    stateManager.SwitchState(stateManager._partSnapState);
                }
                else
                {
                    //if not set the object so that it is positioned in worldspace where the mouse is
                    grabbedObject.transform.position = GrabbedObjPos();

                }

                //update the mirrored objects position and rotation so it is next to the grabbed object but mirrored
                if (grabbedMirroredObject != null)
                {
                    grabbedMirroredObject.transform.position = grabbedObject.transform.position;
                    grabbedMirroredObject.transform.localRotation = new Quaternion(grabbedObject.transform.localRotation.x * -1.0f,
                        grabbedObject.transform.localRotation.y,
                        grabbedObject.transform.localRotation.z,
                        grabbedObject.transform.localRotation.w * -1.0f);
                }
            }
        }

        //if the mouse button is no longer held destroy the grabbed object and its mirror, before switching to the Idle State
        if (Input.GetMouseButtonUp(0))
        {
            GameObject.Destroy(grabbedObject.gameObject);
            GameObject.Destroy(grabbedMirroredObject.gameObject);
            stateManager.SwitchState(stateManager._idleState);
        }
    }

    //Method that is called when this state is exited 
    public override void ExitState(CreatureCreatorStateManager stateManager)
    { 
    }

    //Method that sends out a raycast from the mouse position into the screen using the cameras forward vector
    //later needs to be put in a utilities class
    RaycastHit CastRay()
    {
        RaycastHit rayHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out rayHit);
        return rayHit;
    }

    //method that calculates where in space the grabbed object will be when not over the character
    Vector3 GrabbedObjPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 pos = ray.origin + (ray.direction * distanceFromCamera);
        return pos;
    }

    //Method to set up the references to the grabbed object and its mirror
    void SetUpObjectEditing(Transform objOne, Transform objMir)
    {
        //make sure the grabbed and mirrored objects are assigned correctly, based on the object that is under the players mouse
        grabbedObject = objOne;
        grabbedMirroredObject = objMir;
        if (grabbedObject.localScale.x < 0)
        {
            grabbedMirroredObject = objOne;
            grabbedObject = objMir;
        }
        //set the objects to ignore raycasts if they dont already so the objects behind the grabbed object can be detected
        grabbedObject.gameObject.layer = 2;
        grabbedMirroredObject.gameObject.layer = 2;
    }
}
