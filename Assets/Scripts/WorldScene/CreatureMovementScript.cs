using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMovementScript : MonoBehaviour
{
    public CharacterController charCon;

    public Transform cam;
    public float speed = 2;
    public float gravity = -9.81f;
    public float jumpHeight = 0.7f;
    Vector3 velocity;
    bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    // Update is called once per frame

    void Update()
    {
        //jump
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2.5f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //gravity
        velocity.y += gravity * Time.deltaTime;
        charCon.Move(velocity * Time.deltaTime);

        //walk
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;


        if (direction.magnitude >= 0.1f)
        {
            float facingDir = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, facingDir, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, facingDir, 0f) * Vector3.forward;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                charCon.Move(moveDir.normalized * (speed * 2) * Time.deltaTime);
            }
            else
            {
                charCon.Move(moveDir.normalized * speed * Time.deltaTime);
            }

        }

    }

}
