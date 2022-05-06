using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that controls the tab button behaviour, using events and observer pattern. Sits on the tabs object
/// </summary>
public class TabController : MonoBehaviour
{
    //class that controls tha UI icons
    public BodyPartList iconList;

    //the colours that the buttons switch between
    public Color idleColor;
    public Color hoverColor;
    public Color selectedColor;

    //variables that control the tab buttons
    public List<TabButton> tabs;
    public TabButton selectedTab;

    //method to add tab buttons to the list of subjects 
    public void AddButton(TabButton tab)
    {
        if(tabs == null)
        {
            tabs = new List<TabButton>();
        }
        tabs.Add(tab);
    }
   
    //method that is called when a tab button is hovered over, changes the tab colour
    public void OnTabEnter(TabButton tab)
    {
        ResetTabs();
        if(selectedTab == null || tab != selectedTab)
        {
            tab.tabBackground.color = hoverColor;
        }
        
    }

    //method that is called when a tab is selected, sets the selected tab to be the current active tab, 
    //changes the button colour and sets the appropriate icons to be visable
    public void OnTabSelected(TabButton tab)
    {
        selectedTab = tab;
        ResetTabs();
        tab.tabBackground.color = selectedColor;
        if(selectedTab.type != BodyPartData.bpType.All)
        {
            iconList.ChangeIconsDisplayedbyType(tab.type);
        }
        else
        {
            iconList.DisplayAllIcons();
        }
        
    }

    //method that is called when the mouse exits the tab, resets the tab buttons to their original colour
    public void OnTabExit(TabButton tab)
    {
        ResetTabs();
    }

    //method that resets all but the currently active tab to its original colour
    public void ResetTabs()
    {
        foreach (TabButton tab in tabs)
        {
            if(tab != selectedTab)
            {
                tab.tabBackground.color = idleColor;
            }
        }
    }
}
