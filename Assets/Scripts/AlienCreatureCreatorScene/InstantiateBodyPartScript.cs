using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateBodyPartScript : MonoBehaviour
{
    public GameObject grabbedObject;
    public GameObject duplicateObject;
    BodyPartData partData;

    void Start()
    {
        DragAndDropEventSystem.Instance.onLimbIconExitMenuArea += InstantiateDraggedObject;
    }

    void InstantiateDraggedObject(Vector3 objPos)
    {
        partData = CreatureCreatorStateManager.Instance.BodyPartData;
        grabbedObject = Instantiate(partData.bodyPartPrefab, /* alter this bit to sort out the object positioning issue*/ Camera.main.ScreenToWorldPoint(objPos), Quaternion.identity);
        grabbedObject.layer = 2;
        grabbedObject.GetComponent<BodyPartDataHolder>().partData = partData;
        InstantiateMirrorObject(partData);
    }

    void InstantiateMirrorObject(BodyPartData data)
    {
        duplicateObject = Instantiate(grabbedObject);;
        Vector3 objScale = grabbedObject.transform.localScale;
        duplicateObject.transform.localScale = new Vector3(-objScale.x, objScale.y, objScale.z);
        duplicateObject.layer = 2;
        duplicateObject.GetComponent<BodyPartDataHolder>().partData = data;
        grabbedObject.GetComponent<BodyPartDataHolder>().pairObj = duplicateObject.transform;
        duplicateObject.GetComponent<BodyPartDataHolder>().pairObj = grabbedObject.transform; 
        ChangeState();
        
    }

    void ChangeState()
    {
        CreatureCreatorStateManager.Instance.GrabbedObj = grabbedObject.transform;
        CreatureCreatorStateManager.Instance.MirroredObj = duplicateObject.transform;
        CreatureCreatorStateManager.Instance.SwitchState(CreatureCreatorStateManager.Instance._partDragState);
    }
}
