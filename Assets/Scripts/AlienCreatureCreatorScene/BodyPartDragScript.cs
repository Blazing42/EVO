using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartDragScript : MonoBehaviour
{
    //singleton 
    private static BodyPartDragScript _instance;
    public static BodyPartDragScript Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public GameObject grabbedObject;
    public float objectDistance;
    public GameObject duplicateObject;

    void Start()
    {
        DragAndDropEventSystem.Instance.onLimbIconExitMenuAreaBodyPart += InstantiateDraggedObject;
        DragAndDropEventSystem.Instance.onLimbObjectDestruction += DestroyLimbObjects;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(grabbedObject == null)
            {
                RaycastHit hit = CastRay();
                if(hit.collider != null)
                {
                    if(!hit.collider.CompareTag("DraggableBodyPart"))
                    {
                        return;
                    }
                    else
                    {
                        grabbedObject = hit.collider.gameObject;
                        //set it so that it ignores raycasts
                        grabbedObject.layer = 2;
                        DragAndDropEventSystem.Instance.LimbObjectSelected(grabbedObject.transform);
                        if(duplicateObject == null)
                        {
                            InstantiateMirrorObject();
                        }
                    }
                }
            }
        }

       
        //once the object is picked up drag it around
        if(Input.GetMouseButton(0))
        {
            if(grabbedObject != null)
            {
                RaycastHit hit = CastRay();

                if (hit.collider != null && hit.collider.CompareTag("MainBody"))
                {
                    DragAndDropEventSystem.Instance.MainBodyDragOverEnter(grabbedObject.transform);
                }
                else
                {
                    DragAndDropEventSystem.Instance.MainBodyDragOverExit();
                    Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(grabbedObject.transform.position).z);
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
                    grabbedObject.transform.position = worldPosition;
                }
            
                //make sure the other object is still mirroring the movements
                if(duplicateObject != null)
                {
                    duplicateObject.transform.position = new Vector3(-grabbedObject.transform.position.x , 
                        grabbedObject.transform.position.y, 
                        grabbedObject.transform.position.z);
                    duplicateObject.transform.localRotation = new Quaternion(grabbedObject.transform.localRotation.x * -1.0f,
                        grabbedObject.transform.localRotation.y,
                        grabbedObject.transform.localRotation.z,
                        grabbedObject.transform.localRotation.w * -1.0f);
                }   
            }    
        }
        
        if(Input.GetMouseButtonUp(0))
        {
            DragAndDropEventSystem.Instance.LimbObjectDeselected();
            DragAndDropEventSystem.Instance.MainBodyDragOverExit();
            if(grabbedObject != null)
            {
                grabbedObject.layer = 0;
            }
            
            grabbedObject = null;
        }

    }

    private RaycastHit CastRay()
    {
        RaycastHit rayHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out rayHit);
        Debug.Log("raycast sent");
        return rayHit;
    }

    void InstantiateDraggedObject(Vector3 objPos, BodyPartData partData)
    {
        grabbedObject = Instantiate(partData.bodyPartPrefab, Camera.main.ScreenToWorldPoint(objPos), Quaternion.identity);
        grabbedObject.layer = 2;
        InstantiateMirrorObject();
    }

    void InstantiateMirrorObject()
    {
        duplicateObject = Instantiate(grabbedObject);
        duplicateObject.transform.position = new Vector3(-grabbedObject.transform.position.x, grabbedObject.transform.position.y, grabbedObject.transform.position.z);
        Vector3 objScale = grabbedObject.transform.localScale;
        duplicateObject.transform.localScale = new Vector3(-objScale.x, objScale.y, objScale.z);
        duplicateObject.layer = 2;
    }

    void DestroyLimbObjects()
    {
        if(grabbedObject != null)
        {
            Destroy(grabbedObject.gameObject);
            grabbedObject = null;
        }
        if(duplicateObject != null)
        {
            Destroy(duplicateObject);
            duplicateObject = null;
        }
    }
}
