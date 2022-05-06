using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class that contains all the events that pass information between the various scripts controlling the 
/// dragging and dropping of creature body parts into the creature creator scene and onto the alien creatures body
/// </summary>
public class DragAndDropEventSystem : MonoBehaviour
{
    //singleton 
    private static DragAndDropEventSystem _instance;
    public static DragAndDropEventSystem Instance { get { return _instance; } }
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
    

    //triggers when a body part icon is selected from the menu
    //have it pass on the limb objecs scriptable object
    public event Action<BodyPartData, Vector2> onLimbIconSelected;
    public void LimbIconSelected(BodyPartData bodyPart, Vector2 mousePos)
    {
        if (onLimbIconSelected != null)
        {
            onLimbIconSelected.Invoke(bodyPart, mousePos);
        }
    }

    //triggers when a body part icon exits the UI menu area, method that only requires the mouse position
    public event Action<Vector3> onLimbIconExitMenuArea;
    public void LimbIconExitMenu(Vector3 mousePos)
    {
        if (onLimbIconExitMenuArea != null)
        {
            onLimbIconExitMenuArea.Invoke(mousePos);
        }
    }

    //triggers when a body part icon exits the UI menu area, method that requires the body part data scriptable object as well
    public event Action<Vector3, BodyPartData> onLimbIconExitMenuAreaBodyPart;
    public void LimbIconExitMenuBodyPart(Vector3 mousePos, BodyPartData partData)
    {
        if (onLimbIconExitMenuAreaBodyPart != null)
        {
            onLimbIconExitMenuAreaBodyPart.Invoke(mousePos, partData);
        }
    }



    //triggers when a limb is dragged over the body
    //triggers the script that snaps the limb to the object surface depending on its type
    public event Action<Transform> onMainBodyDragOverEnter;
    public void MainBodyDragOverEnter(Transform draggedObj)
    {
        if(onMainBodyDragOverEnter != null)
        {
            onMainBodyDragOverEnter.Invoke(draggedObj);
        }
    }

    //triggers when a limb is dragged away from the body
    //deactivates the script that snaps the object to the body surface
    public event Action onMainBodyDragOverExit;
    public void MainBodyDragOverExit()
    {
        if (onMainBodyDragOverExit != null)
        {
            onMainBodyDragOverExit.Invoke();
        }
    }


    //triggers when a limb object that is not in the form of a 2d icon is selected
    //later down the line set glow etc to tell players the active object
    public event Action<Transform> onLimbObjectSelected;
    public void LimbObjectSelected (Transform draggedObj)
    {
        if (onLimbObjectSelected != null)
        {
            onLimbObjectSelected.Invoke(draggedObj);
        }
    }

    //triggers when a limb object that is not in the form of a 2d icon is deselected and is no longer the active object
    //triggers the script that joins the meshes together
    public event Action onLimbObjectDeselected;
    public void LimbObjectDeselected()
    {
        if (onLimbObjectDeselected != null)
        {
            onLimbObjectDeselected.Invoke();
        }
    }

    //triggers when the limb object, not in the form of a 2d icon is destroyed, removed from the scene
    //mainly creates changes in stats and evo points etc
    public event Action onLimbObjectDestruction;
    public void LimbObjectDestruction()
    {
        if(onLimbObjectDestruction != null)
        {
            onLimbObjectDestruction.Invoke();
        }
    }

    

    

    

}
