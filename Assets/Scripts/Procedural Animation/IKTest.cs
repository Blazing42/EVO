using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTest : MonoBehaviour
{
    public int chainLength;
    //set this target to aligh with terrain surface
    public Transform target;
    public Transform pole;
    Transform current;

    public int iterations;
    public float delta;

    float[] boneLengths;
    float legLength;
    Transform[] boneTrans;
    Vector3[] bonePos;

    //all the starting positions and locations of the bones, target and root
    Vector3[] startBoneDirs;
    Quaternion[] startBoneRots;
    Quaternion startTargetRot;
    Quaternion startRootRot;


    // Start is called before the first frame update
    void Start()
    {
        Setup();
        //Debug.Log(legLength);
    }

    void LateUpdate()
    {
        ResolveIK();
    }

    private void ResolveIK()
    {
        //check if there is a target
        if(target == null)
        {
            return;
        }

        //check to see if the bone lengths have been calculated
        if(boneLengths.Length != chainLength)
        {
            Setup();
        }

        //get the positions and rotation the bones are in to start with
        for (int i = 0; i < boneTrans.Length; i++)
        {
            bonePos[i] = boneTrans[i].position;
        }

        Quaternion rootRot = (boneTrans[0].parent != null) ? boneTrans[0].parent.rotation : Quaternion.identity;
        Quaternion rootRotDif = rootRot * Quaternion.Inverse(startRootRot);

        //calculate the new positions the bones should be in
        //if the target/ground is further away from the body than the length of the leg
        if((target.position - bonePos[0]).magnitude >= legLength)
        {
            //get the vector between the target and the root bone
            Vector3 direction = (target.position - bonePos[0]).normalized;

            //distribute the bones along this vector based on their lengths calculated in setup
            for (int i = 1; i < bonePos.Length; i++)
            {
                bonePos[i] = bonePos[i - 1] + direction * boneLengths[i - 1];
            }
        }
        else
        {
            //start iterating the algorithm
            for (int iter = 0; iter < iterations; iter++)
            {
                //back pass, starting from target
                bonePos[bonePos.Length - 1] = target.position;
                for (int i = bonePos.Length - 2; i > 0; i--)
                {
                    bonePos[i] = bonePos[i + 1] + (bonePos[i] - bonePos[i + 1]).normalized * boneLengths[i];
                }

                //forward pass, starting from the base bone anchor
                for (int i = 1; i < bonePos.Length; i++)
                {
                    bonePos[i] = bonePos[i - 1] + (bonePos[i] - bonePos[i - 1]).normalized * boneLengths[i - 1];
                }

                //if the final bone position is close enough to the target break before the max number of iterations
                if((bonePos[bonePos.Length - 1]- target.position).magnitude < delta)
                {
                    break;
                }
            }
        }

        if(pole != null)
        {
            for (int i = 1; i < bonePos.Length - 1; i++)
            {
                Plane plane = new Plane(bonePos[i + 1] - bonePos[i - 1], bonePos[i - 1]);
                Vector3 projectedPole = plane.ClosestPointOnPlane(pole.position);
                Vector3 projectedBone = plane.ClosestPointOnPlane(bonePos[i]);
                float angle = Vector3.SignedAngle(projectedBone - bonePos[i - 1], projectedPole - bonePos[i - 1], plane.normal);
                bonePos[i] = Quaternion.AngleAxis(angle, plane.normal) * (bonePos[i] - bonePos[i - 1]) + bonePos[i - 1];
            }
        }

        //set the positions and rotations of the bones to the new calculated pos
        for (int i = 0; i < bonePos.Length-1; i++)
        {
            boneTrans[i].rotation = Quaternion.FromToRotation(startBoneDirs[i], bonePos[i + 1] - bonePos[i]) * startBoneRots[i];
            boneTrans[i].position = bonePos[i];
        }
    }

    void Setup()
    {
        //Initiate arrays and references
        boneTrans = new Transform[chainLength + 1];
        bonePos = new Vector3[chainLength + 1];
        boneLengths = new float[chainLength];
        startBoneDirs = new Vector3[chainLength + 1];
        startBoneRots = new Quaternion[chainLength + 1];

        startTargetRot = target.rotation;
        legLength = 0;
        current = transform;

        //set the first values in the array
        boneTrans[chainLength] = current;
        startBoneRots[chainLength] = current.rotation;
        startBoneDirs[chainLength] = target.position - current.position;
        
        //fill in the rest of the array values
        for (int i = chainLength - 1; i >= 0; i--)
        {
            startBoneDirs[i] = boneTrans[i + 1].position - current.position;
            boneLengths[i] = startBoneDirs[i].magnitude;
            startBoneRots[i] = current.rotation;
            boneTrans[i] = current;
            legLength += boneLengths[i];
            current = current.parent;
        }

    }
}
