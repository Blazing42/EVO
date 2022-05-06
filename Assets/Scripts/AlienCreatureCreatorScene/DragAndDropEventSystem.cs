using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class that contains all the events that pass information between the various scripts controlling the 
/// dragging and dropping of creature body parts into the creature creator scene and onto the alien creatures body
/// make this a scriptable object rather than a singleton, as it doesnt use start or update, to remove dependancies that gum up the system later on
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
    

    //triggers when a body part icon is selected from the menuz
    //have it pass on the limb objects scriptable object to the copied icon that you are dragging
    public event Action<BodyPartData, Vector2> onLimbIconSelected;
    public void LimbIconSelected(BodyPartData bodyPart, Vector2 mousePos)
    {
        if (onLimbIconSelected != null)
        {
            onLimbIconSelected.Invoke(bodyPart, mousePos);
        }
    }

    //triggers when a body part icon exits the UI menu area, method that only requires the mouse position 
    //the interface trigger on mouse exit on the Menu ui object will use this as it doesnt need to know any of the information on the icon just that the mouse left the menu
    public event Action<Vector3> onLimbIconExitMenuArea;
    public void LimbIconExitMenu(Vector3 mousePos)
    {
        if (onLimbIconExitMenuArea != null)
        {
            onLimbIconExitMenuArea.Invoke(mousePos);
        }
    }

    //triggers when a body part icon exits the UI menu area, method that requires the body part data scriptable object as well
    //event that is triggered by the previous event on this script, the one when the mouse leaves the menu, but has more info required for the other scripts
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
    /*public event Action<Transform, Transform> onMainBodyDragOverEnter;
    public void MainBodyDragOverEnter(Transform draggedObj, Transform mirroredObj)
    {
        if(onMainBodyDragOverEnter != null)
        {
            onMainBodyDragOverEnter.Invoke(draggedObj, mirroredObj);
        }
    }*/

    //triggers when a limb is dragged away from the body
    //deactivates the script that snaps the object to the body surface
   /* public event Action onMainBodyDragOverExit;
    public void MainBodyDragOverExit()
    {
        if (onMainBodyDragOverExit != null)
        {
            onMainBodyDragOverExit.Invoke();
        }
    }*/


    //triggers when a limb object that is not in the form of a 2d icon is selected
    //later down the line set glow etc to tell players the active object
    /*public event Action<Transform> onLimbObjectSelected;
    public void LimbObjectSelected (Transform draggedObj)
    {
        if (onLimbObjectSelected != null)
        {
            onLimbObjectSelected.Invoke(draggedObj);
        }
    }*/

    //triggers when a limb object that is not in the form of a 2d icon is deselected
    //triggers the script that joins the meshes together, and triggers the scripts that allow you to edit the limbs 
    /*public event Action<Transform, Transform> onLimbObjectDeselected;
    public void LimbObjectDeselected(Transform draggedObj, Transform mirroredObj)
    {
        if (onLimbObjectDeselected != null)
        {
            onLimbObjectDeselected.Invoke(draggedObj, mirroredObj);
        }
    }*/

    //triggers when the limb object, not in the form of a 2d icon is destroyed, removed from the scene
    //mainly creates changes in stats and evo points etc
    /*public event Action onLimbObjectDestruction;
    public void LimbObjectDestruction()
    {
        if(onLimbObjectDestruction != null)
        {
            onLimbObjectDestruction.Invoke();
        }
    }*/
}
