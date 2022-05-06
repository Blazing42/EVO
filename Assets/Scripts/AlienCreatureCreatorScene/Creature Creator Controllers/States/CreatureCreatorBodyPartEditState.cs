using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that controls the Creature Creator Body Part Edit State
/// </summary>
public class CreatureCreatorBodyPartEditState : CreatureCreatorBaseState
{
    Transform grabbedObject;
    Transform grabbedMirroredObject;
    float limbScaleSpeed = 2;
    Vector3 startPos = Vector3.zero;
    Vector3 mouseDeltaPos = Vector3.zero;
    bool circleAdjust = false;
    bool sphereAdjust = false;

    public override void EnterState(CreatureCreatorStateManager stateManager)
    {
        SetUpObjectEditing(stateManager.GrabbedObj, stateManager.MirroredObj);
        EnableEditingGizmos(grabbedObject);
        EnableEditingGizmos(grabbedMirroredObject);
    }

    public override void UpdateState(CreatureCreatorStateManager stateManager)
    {
        HandleScaleInput();
        
        if (Input.GetMouseButtonDown(0))
        {
            //send out a ray
            RaycastHit hit = CastRay();
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.transform.gameObject.name);
                //if it doesnt hit an editable object do nothing 
                if (hit.collider.CompareTag("DraggableBodyPart"))
                {
                    HandleSwitchingEditableObj(hit,stateManager);
                }
                else if( hit.collider.transform.CompareTag("AjustmentCircle"))
                {
                    circleAdjust = true;
                }
                else if(hit.collider.transform.CompareTag("AjustmentSphere"))
                {
                    sphereAdjust = true;
                }
            }
            else
            {
                //when clicking outside of the object change to the idle state
                stateManager.SwitchState(stateManager._idleState);
            }
        }

        if (Input.GetMouseButton(0))
        {
            if(startPos != new Vector3(0, 0, 0))
            {
                RaycastHit hit = CastRay();
                //track the position of the mouse while it is held down if it is more than 2 units away from the start position
                mouseDeltaPos = Input.mousePosition;
                if(Vector3.Distance(startPos, mouseDeltaPos) > 3.0f)
                {
                    //set the objects to idnore raycasts while doing the check
                    grabbedObject.gameObject.layer = 2;
                    grabbedMirroredObject.gameObject.layer = 2;
                    if(hit.collider.CompareTag("MainBody"))
                    {
                        //change the state to the snapping object state, if still over the main body 
                        stateManager.SwitchState(stateManager._partSnapState);
                    }
                    else
                    {
                        //change the state to the dragging object script, if not
                        stateManager.SwitchState(stateManager._partDragState);
                    }
                }
            }
            if (circleAdjust == true)
            {
                float inputXAxis = Input.GetAxis("Mouse X") * 2f;
                grabbedObject.transform.Rotate(grabbedObject.transform.forward, inputXAxis /*-Vector3.Dot(mouseDeltaPos, Camera.main.transform.right)*0.04f*/ ,Space.World);
                grabbedMirroredObject.localRotation = new Quaternion(grabbedObject.transform.localRotation.x * -1.0f,
                        grabbedObject.transform.localRotation.y,
                        grabbedObject.transform.localRotation.z,
                        grabbedObject.transform.localRotation.w * -1.0f);
            }
            if(sphereAdjust == true)
            {
                float inputYAxis = Input.GetAxis("Mouse Y") * 2f;
                grabbedObject.transform.Rotate(grabbedObject.transform.right, -inputYAxis/*Vector3.Dot(mouseDeltaPos, Camera.main.transform.forward)*0.04f*/, Space.World);
                grabbedMirroredObject.localRotation = new Quaternion(grabbedObject.transform.localRotation.x * -1.0f,
                        grabbedObject.transform.localRotation.y,
                        grabbedObject.transform.localRotation.z,
                        grabbedObject.transform.localRotation.w * -1.0f);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            sphereAdjust = false;
            circleAdjust = false;
            startPos = new Vector3(0,0,0);
            //enable the gizmos on the new object that was selected if not choosing to drag the object away from the body
            EnableEditingGizmos(grabbedObject);
            EnableEditingGizmos(grabbedMirroredObject);
        }
        
    }
    public override void ExitState(CreatureCreatorStateManager stateManager)
    {
        //disable the editing gizmos when changing to a different state
        DisableEditingGizmos(grabbedObject);
        DisableEditingGizmos(grabbedMirroredObject);
        //make sure the limb objects are set to not be ignored by raycasts when leaving the state
        grabbedObject.gameObject.layer = 0;
        grabbedMirroredObject.gameObject.layer = 0;
    }

    void SetUpObjectEditing(Transform objOne, Transform objMir)
    {
        grabbedObject = objOne;
        grabbedMirroredObject = objMir;

        if (grabbedObject.localScale.x < 0)
        {
            grabbedMirroredObject = objOne;
            grabbedObject = objMir;
        }
        startPos = new Vector3(0, 0, 0);
    }

    void EnableEditingGizmos(Transform editableObj)
    {
        for (int i = 0; i < editableObj.childCount; i++)
        {
            //set them so they are visable
            editableObj.GetChild(i).gameObject.SetActive(true);
            //set them so they arent ignored by raycasts
            editableObj.GetChild(i).gameObject.layer = 0;
        }
    }

    void DisableEditingGizmos(Transform editableObj)
    {
        for (int i = 0; i < editableObj.childCount; i++)
        {
            //set them so they are not visable
            editableObj.GetChild(i).gameObject.SetActive(false);
            //set them so that they ignore raycasts
            editableObj.GetChild(i).gameObject.layer = 2;
        }
    }
    void HandleScaleInput()
    {
        Vector3 min = new Vector3(0.06f, 0.06f, 0.06f);
        Vector3 max = new Vector3(0.15f, 0.15f, 0.15f);

        Vector3 newScale = new Vector3();
        Vector3 newMirScale = new Vector3();

        //if statements to handle the scaling of the object
        if (Input.mouseScrollDelta.y != 0)
        {
            //object on right of x=0
            newScale.x = Mathf.Clamp(grabbedObject.localScale.x - 0.01f * Input.GetAxis("Mouse ScrollWheel") * limbScaleSpeed, min.x, max.x);
            newScale.y = Mathf.Clamp(grabbedObject.localScale.y - 0.01f * Input.GetAxis("Mouse ScrollWheel") * limbScaleSpeed, min.y, max.y);
            newScale.z = Mathf.Clamp(grabbedObject.localScale.z - 0.01f * Input.GetAxis("Mouse ScrollWheel") * limbScaleSpeed, min.z, max.z);
            grabbedObject.localScale = newScale;

            //object on left of x=0
            newMirScale.x = Mathf.Clamp(grabbedMirroredObject.localScale.x + 0.01f * Input.GetAxis("Mouse ScrollWheel") * limbScaleSpeed, -max.x, -min.x);
            newMirScale.y = Mathf.Clamp(grabbedMirroredObject.localScale.y - 0.01f * Input.GetAxis("Mouse ScrollWheel") * limbScaleSpeed, min.y, max.y);
            newMirScale.z = Mathf.Clamp(grabbedMirroredObject.localScale.z - 0.01f * Input.GetAxis("Mouse ScrollWheel") * limbScaleSpeed, min.z, max.z);
            grabbedMirroredObject.localScale = newMirScale;
        }
    }

    void HandleSwitchingEditableObj(RaycastHit hit, CreatureCreatorStateManager stateManager)
    {
        //if the player clicks on a different editable object
        if (hit.collider.transform != grabbedObject || hit.collider.transform != grabbedMirroredObject)
        {
            if (hit.collider.transform.gameObject.tag == "DraggableBodyPart")
            {
                //disable the old grabbed object gizmos while deciding what to do
                DisableEditingGizmos(grabbedObject);
                DisableEditingGizmos(grabbedMirroredObject);

                //set the new editable object up 
                var selectedObj = hit.collider.transform;
                var mirroredObj = selectedObj.GetComponent<BodyPartDataHolder>().pairObj;
                SetUpObjectEditing(selectedObj, mirroredObj);
                startPos = Input.mousePosition;
                stateManager.GrabbedObj = selectedObj;
                stateManager.MirroredObj = mirroredObj;
            }
        }
        else
        {
            startPos = Input.mousePosition;
        }
    }

    //remember to put in utilities script
    private RaycastHit CastRay()
    {
        RaycastHit rayHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out rayHit);
        return rayHit;
    }
}
