using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// script that contains the trigger that causes the events that happen when the bodypart icon leaves the menu
/// must be placed on the parent menu object that encompases the entire left had menu
/// </summary>
public class OnMenuIconExitTrigger : MonoBehaviour, IPointerExitHandler
{
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("exit menu area");
        DragAndDropEventSystem.Instance.LimbIconExitMenu(eventData.position);
    }
}
