using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class that controls the camera movement in the creature creator scene, based on player input
/// </summary>
public class CameraController : MonoBehaviour
{

    //variables to control the camera rotation
    Quaternion newRot;
    [SerializeField] float camRotSpeed;
    [SerializeField] float camRotTime;

    //variables to control the camera zoom
    Vector3 newZoom;
    [SerializeField] Vector3 zoomAmount;
    [SerializeField] float camZoomTime;
    [SerializeField] float minYCamZoom;
    [SerializeField] float maxYCamZoom;
    [SerializeField] float minZCamZoom;
    [SerializeField] float maxZCamZoom;
    [SerializeField] Transform cameraTransform;


    //set the camera to slowly rotate when the player isnt doing anything
    //bool slowRotation = true;

    // Start is called before the first frame update
    void Start()
    {
        newRot = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

        HandleCameraZoomInput();
        HandleCameraRotationInput();

        /*if(slowRotation = true)
        {
            rotateslowlythecamera
        }*/
    }

    //method that handles any horizontal and vertical input from controllers to rotate around the creature
    void HandleCameraRotationInput()
    {
        
        if (Input.GetAxis("Horizontal") > 0)
        {
            //right
            newRot *= Quaternion.Euler(Vector3.up * -camRotSpeed);
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            //left
            newRot *= Quaternion.Euler(Vector3.up * camRotSpeed);
        }
        
        //smooths out the camera rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * camRotTime);
    }

    //method that handles any vertical input to zoom the camera in and out
    void HandleCameraZoomInput()
    {
        //clamps the camera movement so it cant zoom in or out too far
        newZoom.y = Mathf.Clamp(newZoom.y, minYCamZoom, maxYCamZoom);
        newZoom.z = Mathf.Clamp(newZoom.z, minZCamZoom, maxZCamZoom);
        newZoom.x = Mathf.Clamp(newZoom.x, 0, 0);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            //zoom in
            newZoom += zoomAmount;
        }
        if (Input.GetKey(KeyCode.S)|| Input.GetKey(KeyCode.DownArrow))
        {
            //zoom out
            newZoom -= zoomAmount;
        }
        
        //smooths out the camera zoom 
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * camZoomTime);
        
    }
}
