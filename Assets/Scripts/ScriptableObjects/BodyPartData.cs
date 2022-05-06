using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object that stores the data for each body part
/// </summary>
[CreateAssetMenu(fileName = "New Body Part", menuName = "Body Part")]
public class BodyPartData : ScriptableObject
{
    public enum bpType { All, Head, Neck, Tail, Leg , Arm, Eyes, Decoration};
    public bpType type;

    public string bpName;
    public string bpDescription;

    public int evoPointCost;
    public bool unlocked = false;

    public Sprite lockedIcon;
    public Sprite unlockedIcon;

    public GameObject bodyPartPrefab;

    //method to unlock the body part
    public void UnlockBodyPart()
    {
        unlocked = true;
    }

    //method used for debugging
    public void DebugPrint()
    {
        Debug.Log("This " + bpName + " costs " + evoPointCost + " and is " + bpDescription);
        if(unlocked == false)
        {
            Debug.Log("it is currently locked");
        }
        else
        {
            Debug.Log("the player has unlocked this body part");
        }
    }
}
