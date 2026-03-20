using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;


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
    }


    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Update()
    {  //Ground Check :P
        grouded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, WhatIsGround);

        MyInput();

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

    }
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }
}
