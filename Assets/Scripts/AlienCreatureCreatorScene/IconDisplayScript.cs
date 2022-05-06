using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Script that controls how the body part data is displayed in the alien creature creation part menu ui
/// </summary>
public class IconDisplayScript : MonoBehaviour, IPointerDownHandler 
{
    Sprite displayedIcon;
    Image iconImage;
    ScrollRect scrollRect;

    [SerializeField]
    private BodyPartData partData;
    public BodyPartData PartData { get { return partData; } set { partData = value; } }

    // Start is called before the first frame update
    void Start()
    {
        scrollRect = GameObject.FindObjectOfType<ScrollRect>();
        iconImage = GetComponent<Image>();
        SetUpIcon();
        iconImage.sprite = displayedIcon;
    }

    void SetUpIcon()
    {
        if(partData.unlocked != true)
        {
            displayedIcon = partData.lockedIcon;
        }
        else
        {
            displayedIcon = partData.unlockedIcon;
        }
    }

    public void UpdateIcon()
    {
        SetUpIcon();
    }

    //methods to control what happens to the icon when it is interacted with  
    public void OnIconClick()
    {
        Debug.Log("press");
        DragAndDropEventSystem.Instance.LimbIconSelected(partData, Input.mousePosition);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnIconClick();
    }
}
