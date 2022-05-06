using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that controls the Creature Creator Icon Drag State
/// </summary>
public class CreatureCreatorIconDragState : CreatureCreatorBaseState
{
    GameObject icon;
    Transform draggedIcon;
    BodyPartData partData;
    ScrollRect scrollRect;

    //Mathod that is called when this state is first entered
    public override void EnterState(CreatureCreatorStateManager stateManager)
    {
        //set up the class variables using the references to gameobjects stored in the monobehaviour state manager class
        Vector3 mousePos = Input.mousePosition;
        icon = stateManager.DraggedIconPrefab;
        partData = stateManager.BodyPartData;
        scrollRect = stateManager.Canvas.GetComponentInChildren<ScrollRect>();

        //just in case make sure there is only one dragged icon object at a time
        if (draggedIcon != null)
        {
            DestroyIconCopy();
        }

        //if the part is unlocked create a copy of the icon that was clicked on to be dragged by the player
        if (partData.unlocked == true)
        {
            draggedIcon = icon.transform;
            draggedIcon.position = mousePos;
            icon.GetComponent<Image>().enabled = true;
            icon.GetComponent<Image>().sprite = partData.unlockedIcon;

            //disable the ui menu scroll when dragging the icon around 
            scrollRect.enabled = false;
        }
    }

    //method that controls what happens each time the update is called in this state
    public override void UpdateState(CreatureCreatorStateManager stateManager)
    {
        //if player holds down the mouse button the icon copy will be dragged, following the mouse position
        if (draggedIcon != null)
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
    
    //method that controls what happens when this state is exited
    public override void ExitState(CreatureCreatorStateManager stateManager)
    {
        DestroyIconCopy();
    }

    //resets the icon copy that was dragged/moved across the screen
    void DestroyIconCopy()
    {
        icon.GetComponent<Image>().enabled = false;
        draggedIcon = null;

        //renables the scrollRect so the player can interact with it
        scrollRect.enabled = true;
        partData = null;
    }
}
