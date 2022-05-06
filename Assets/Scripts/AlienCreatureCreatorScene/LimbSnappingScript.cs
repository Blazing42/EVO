using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbSnappingScript : MonoBehaviour
{
    //link this object to an event system, as lots of scrits will be accessing this variable and will need to know
    //if an object is currently selected, have a bool and be able to pass the object reference between scripts
    Transform selectedObject;
    bool sendOutRay = false;

    // Start is called before the first frame update
    void Start()
    {
        DragAndDropEventSystem.Instance.onMainBodyDragOverEnter += MouseOverBody;
        DragAndDropEventSystem.Instance.onMainBodyDragOverExit += MouseExitBody;
    }

    // Update is called once per frame
    void Update()
    {
        if(sendOutRay == true)
        {
            RaycastHit hit = CastRay();
            if(hit.collider.CompareTag("MainBody"))
            {
                Vector3 surfaceNorm = hit.normal;
                Debug.Log("(" + surfaceNorm.x + " , " + surfaceNorm.y + " , " + surfaceNorm.z + ")");
                Debug.DrawRay(hit.point, surfaceNorm * 20, Color.blue);
                if (selectedObject != null)
                {
                    selectedObject.position = hit.point;
                    selectedObject.rotation = Quaternion.Lerp(selectedObject.rotation, Quaternion.LookRotation(hit.normal), Time.deltaTime * 5f);
                }
            }
        }  
    }

    //starts the raycast being sent out
    void MouseOverBody(Transform obj)
    {
        selectedObject = obj;
        sendOutRay = true;
    }

    //stops the raycast from being sent out
    void MouseExitBody()
    {
        selectedObject = null;
        sendOutRay = false;
    }

    void OnDestroy()
    {
        DragAndDropEventSystem.Instance.onMainBodyDragOverEnter -= MouseOverBody;
        DragAndDropEventSystem.Instance.onMainBodyDragOverExit -= MouseExitBody;
    }

    //put this into a utilities static class, as will be used a few times
    //creates a raycast from the mouse position
    private RaycastHit CastRay()
    {
        RaycastHit rayHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out rayHit);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        Debug.Log("raycast sent");
        return rayHit;
    }
}
