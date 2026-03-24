using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;


    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;


    [Header("Keybinds")]

    InputAction JumpAction;
 

    [Header("GroundCheck")]
    public float playerHeight;
    public LayerMask WhatIsGround;
    bool grouded;


    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        JumpAction = InputSystem.actions.FindAction("Jump");
    }


    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Update()
    {  //Ground Check :P
        grouded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, WhatIsGround);

        print(JumpAction.WasPerformedThisFrame());
        MyInput();
        SpeedControl();
        //handle drag
        if (grouded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;


    }


    
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        if (JumpAction.WasPerformedThisFrame() && readyToJump && grouded)
        {
            print("JUMP NOW");
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //on ground
        if (grouded)
         rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        else if (!grouded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f,rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
      
    }
    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x,0f,rb.linearVelocity.z);
        rb.AddForce(transform.up*jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
}
