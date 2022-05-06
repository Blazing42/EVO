using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnBinButtonEnterTrigger : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("delete limb object");
        DragAndDropEventSystem.Instance.LimbObjectDestruction();
    }
}
