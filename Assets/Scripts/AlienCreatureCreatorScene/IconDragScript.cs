using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconDragScript : MonoBehaviour
{
    Transform draggedIcon;
    Canvas canvas;
    GameObject draggedIconObj;
    ScrollRect scrollRect;
    public GameObject dragIconPrefab;
    BodyPartData bodyPartData;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindObjectOfType<Canvas>();
        scrollRect = GameObject.FindObjectOfType<ScrollRect>();
        DragAndDropEventSystem.Instance.onLimbIconSelected += CreateIconCopy;
        DragAndDropEventSystem.Instance.onLimbIconExitMenuArea += RemoveIconCopy;
    }
    
    void Update()
    {
        if(draggedIconObj != null)
        {
            if (Input.GetMouseButton(0))
            {
                draggedIcon.position = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                DestroyIconCopy();
            }
        }
    }

    void CreateIconCopy(BodyPartData partData, Vector2 mousePos) 
    {
        //just in case make sure there is only one dragged object at a time
        if(draggedIcon != null)
        {
            DestroyIconCopy();
        }
        //if the part is unlocked start the drag process
        if(partData.unlocked == true)
        {
            draggedIconObj = Instantiate(dragIconPrefab, mousePos, Quaternion.identity, canvas.transform);
            draggedIcon = draggedIconObj.transform;
            draggedIconObj.GetComponent<Image>().sprite = partData.unlockedIcon;
            bodyPartData = partData;
            scrollRect.enabled = false;
        }
    }

    //method that is triggered by the ipointer exit event on the menu, chains and triggers the event to spawn the prefab from the game data
    //as the iponter event cant access the scriptable objects body data only the mouse position
    void RemoveIconCopy(Vector3 mousePosition)
    {
        if(bodyPartData != null)
        {
            DragAndDropEventSystem.Instance.LimbIconExitMenuBodyPart(mousePosition, bodyPartData);
        }
        DestroyIconCopy();
        
    }

    void DestroyIconCopy()
    {
        Destroy(draggedIconObj);
        draggedIcon = null;
        draggedIconObj = null;
        scrollRect.enabled = true;
        bodyPartData = null;
    }

    void OnDestroy()
    {
        DragAndDropEventSystem.Instance.onLimbIconSelected -= CreateIconCopy;
    }
}
