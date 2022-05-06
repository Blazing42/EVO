using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Class that triggers the change of state events that happen when the bodypart icon leaves the menu, placed on the parent menu object that encompases the entire left hand menu
/// </summary>
public class OnMenuIconExitTrigger : MonoBehaviour, IPointerExitHandler
{
    //method that changes the state of the creature creator, and passes the mouse position on to the instantiate body part script 
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("exit menu area");
        if(CreatureCreatorStateManager.Instance.CurrentState == CreatureCreatorStateManager.Instance._iconDragState)
        {
            DragAndDropEventSystem.Instance.LimbIconExitMenu(eventData.position);
            CreatureCreatorStateManager.Instance.SwitchState(CreatureCreatorStateManager.Instance._partDragState);
        }
    }
}
