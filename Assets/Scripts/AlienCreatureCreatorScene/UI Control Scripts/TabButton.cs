using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Class that triggers the methods in the tab controller, using unitys inbuilt mouseover event interfaces, sits on all of the tab buttons
/// </summary>
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TabController tabController;
    public Image tabBackground;
    //set in the inspector, used to determine which body part type icons to display when the tab button is selected
    public BodyPartData.bpType type;

    // Start is called before the first frame update
    void Start()
    {
        //adds itself to the tab controllers list of observers
        tabController.AddButton(this);
        tabBackground = GetComponent<Image>();
    }

    //event triggering the On Tab Selected method in the tab controller class, when the player clicks on this button
    public void OnPointerClick(PointerEventData eventData)
    {
        tabController.OnTabSelected(this);
    }

    //event triggering the On Tab Entered method in the tab controller class, when the player mouse pointer enters this button
    public void OnPointerEnter(PointerEventData eventData)
    {
        tabController.OnTabEnter(this);
    }

    //event triggering the On Tab Exit method in the tab controller class, when the players mouse pointer leaves this button
    public void OnPointerExit(PointerEventData eventData)
    {
        tabController.OnTabExit(this);
    }
}
