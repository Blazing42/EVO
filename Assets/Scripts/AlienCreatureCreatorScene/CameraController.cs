using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour
{

    //variables to control the camera rotation
    Quaternion newRot;
    [SerializeField] float camRotSpeed;
    [SerializeField] float camRotTime;

    //variables to control the camera zoom
    [SerializeField] float zoomSpeedMultiplier;
    [SerializeField] float minCamZoom;
    [SerializeField] float maxCamZoom;
    Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        //newPos = transform.position;
        newRot = transform.rotation;
        camera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleCameraZoomInput();
        HandleCameraRotationInput();
    }

    void HandleCameraRotationInput()
    {
        //if statements that handle any horizontal and vertical input from controllers to navigate the scene left right up and down
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
        
        //to make the camera scrolling less jarring this causes the cameras transform to happen over a certain amount of time
        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * camRotTime);
    }

    void HandleCameraZoomInput()
    {
        //if statements to handle the camera zoom
        if (Input.mouseScrollDelta.y > 0)
        {
            //zoom out
            camera.orthographicSize -= (Input.GetAxis("Mouse ScrollWheel") * zoomSpeedMultiplier);
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            //zoom in
            camera.orthographicSize -= (Input.GetAxis("Mouse ScrollWheel") * zoomSpeedMultiplier);
        }
        //clamping the zoom value so the player cant zoom in or out to far
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, maxCamZoom, minCamZoom);
    }
}
