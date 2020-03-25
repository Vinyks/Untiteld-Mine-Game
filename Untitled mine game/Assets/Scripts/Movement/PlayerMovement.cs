using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool movement_switch = true; 
    public CharacterController controller;

    public float wobbleSpeed = 15f;
    public float baseMs = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public Camera kamera;

    Vector3 pos;
    float counter;

    Vector3 velocity;
    bool isOnFloor;
    float ms;


    void Update()
    {
        if(Input.GetKey("left shift"))
        {
            ms = baseMs*2;
        }
        else
        {
            ms = baseMs;
        }
        isOnFloor = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isOnFloor && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        if (movement_switch)
        {
            controller.Move(move * ms * Time.deltaTime);
        }
        if (Input.GetButtonDown("Jump") && isOnFloor)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        
        
        controller.Move(velocity * Time.deltaTime);


        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            if(isOnFloor)
            {
                counter += (0.04f) * ms *Time.deltaTime* wobbleSpeed;
                pos = kamera.transform.position;
                pos.y = Mathf.Sin(counter) / 5 + this.transform.position.y + 1.4f;
                kamera.transform.position = pos;
            }
        }

    }

}
