using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that controls the icons displayed in the UI menu, currently sits on the content scroll object
/// </summary>
public class BodyPartList : MonoBehaviour
{
    //BodyPartDatalist is defined in inspector
    //turn this list into scriptable object
    public List<BodyPartData> bodyPartDataList;
    public List<GameObject> bodyPartIcons;
    public GameObject partPrefab; 

    // Start is called before the first frame update
    void Start()
    {
        //instantiate an icon for each part and add to new icon list
        if (bodyPartDataList != null)
        {
            foreach (BodyPartData part in bodyPartDataList)
            {
                GameObject partObj = CreateIcon(part);
                if(bodyPartIcons == null)
                {
                    bodyPartIcons = new List<GameObject>();
                }
                bodyPartIcons.Add(partObj);
            }
        }
    }

    //creates a new icon gameobject displaying the icon info stored in the bodyPartData, as a child of this object
    GameObject CreateIcon(BodyPartData partData)
    {
        GameObject bodyPartIcon = Instantiate(partPrefab, this.transform);
        bodyPartIcon.GetComponentInChildren<IconDisplayScript>().PartData = partData;
        return bodyPartIcon;
    }

    //used by tab system controller, changes the displayed icons based on their type
    public void ChangeIconsDisplayedbyType (BodyPartData.bpType bpType)
    {
        if(bodyPartIcons != null)
        {
            foreach(GameObject iconObj in bodyPartIcons)
            {
                iconObj.SetActive(false);
                BodyPartData bpData = iconObj.GetComponentInChildren<IconDisplayScript>().PartData;
                if(bpData.type == bpType)
                {
                    iconObj.SetActive(true);
                }
            }
        }
    }

    //resets the display to show all of the icons in the list
    public void DisplayAllIcons()
    {
        if (bodyPartIcons != null)
        {
            foreach (GameObject iconObj in bodyPartIcons)
            {
                iconObj.SetActive(true);
            }
        }
    }
}
