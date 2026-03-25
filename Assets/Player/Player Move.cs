using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;



    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool DisablePlaneMovement;
    bool readyToJump;


    [Header("Keybinds")]

    InputAction JumpAction;
 

    [Header("GroundCheck")]

    public float playerHeight;
    public Collider Collider;
    public LayerMask WhatIsGround;
    bool grounded;
    private bool PreviousGrounded;
    private bool ApplyFriction = false;
    private float Friction_Until;
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
        Invoke(nameof(ResetJump), jumpCooldown);
        DisablePlaneMovement = false;
    }


    private void FixedUpdate()
    {
        MovePlayer();

        if (PreviousGrounded != grounded)
        {

            ApplyFriction = true;
            Friction_Until = Time.time + 2f;
        }
        if (ApplyFriction == true)
            {
                if (Time.time <= Friction_Until)
                {
                    Collider.material.staticFriction = 100f;
                }
                else
                {
                    ApplyFriction = false;
                    Collider.material.staticFriction = 0f;
                }
            }
        }


    private void Update()
    {  //Ground Check :P
        PreviousGrounded = grounded;
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, WhatIsGround);
        
        
        SpeedControl();
    
            if (DisablePlaneMovement == false && grounded)
            {
                MyInput();

            }
        



        //handle drag
        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
 


  
    }


    
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        if (JumpAction.WasPerformedThisFrame() && readyToJump && grounded)
        {

            readyToJump = false;
            DisablePlaneMovement = true;
            Jump();
           
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

   

        //on ground
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
          
        }
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
        DisablePlaneMovement = false;
        
    }
}
