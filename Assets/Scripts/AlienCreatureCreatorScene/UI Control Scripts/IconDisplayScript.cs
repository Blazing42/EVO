using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Class that controls how the body part data stored in the scriptable object is displayed by the icon object
/// </summary>
public class IconDisplayScript : MonoBehaviour, IPointerDownHandler 
{
    Sprite displayedIcon;
    Image iconImage;

    [SerializeField]
    private BodyPartData partData;
    public BodyPartData PartData { get { return partData; } set { partData = value; } }

    // Start is called before the first frame update
    void Start()
    {
        iconImage = GetComponent<Image>();
        SetUpIcon();
        iconImage.sprite = displayedIcon;
    }

    //method that sets up the UI, currently determines what image should be displayed based on whether the body part has been unlocked
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

    //methods to control what happens to the icon when it is interacted with, changes the editor state 
    public void OnIconClick()
    {
        Debug.Log("press");
        CreatureCreatorStateManager.Instance.BodyPartData = partData;
        CreatureCreatorStateManager.Instance.SwitchState(CreatureCreatorStateManager.Instance._iconDragState);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnIconClick();
    }
}
