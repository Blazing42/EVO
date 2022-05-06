using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Body Part", menuName = "Body Part")]
public class BodyPartData : ScriptableObject
{
    public enum bpType { Body, Head, Neck_Tail, Leg , Arm_Hands, Feet, Eyes, Nose, Teeth, Wings, Decoration};
    public bpType type;

    public string bpName;
    public string bpDescription;

    public int evoPointCost;
    public bool unlocked = false;

    public Sprite lockedIcon;
    public Sprite unlockedIcon;

    public GameObject bodyPartPrefab;

    public void UnlockBodyPart()
    {
        unlocked = true;
    }

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
