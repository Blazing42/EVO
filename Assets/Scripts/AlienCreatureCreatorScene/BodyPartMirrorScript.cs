using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartMirrorScript : MonoBehaviour
{
    GameObject duplicateObject;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CreateMirrorObject(GameObject obj)
    {
        Mesh meshCopy = obj.GetComponent<MeshFilter>().mesh;
        duplicateObject = new GameObject(obj.name + " copy");
        duplicateObject.transform.position = new Vector3(-obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
        duplicateObject.transform.localScale = obj.transform.localScale;
        duplicateObject.AddComponent(typeof(MeshFilter));
        duplicateObject.AddComponent(typeof(MeshRenderer));
        duplicateObject.GetComponent<MeshFilter>().mesh = meshCopy;
        duplicateObject.GetComponent<MeshRenderer>().material = obj.GetComponent<MeshRenderer>().material;
        Vector3 objScale = obj.transform.localScale;
        duplicateObject.transform.localScale = new Vector3(-objScale.x, objScale.y, objScale.z);
    }
}
