using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{

    private const float NORMAL_FOV = 60f;
    private const float RUNNIG_FOV = 85f;

    float playerHeight = 2f;

    [SerializeField] Transform orientation;
    [SerializeField] CameraMovement cameraMovement;
    [SerializeField] GameObject camera;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float slideForce = 400f;
    [SerializeField] float airMultiplier = 0.4f;
    [SerializeField] float slidingMultiplier = 4f;
    float movementMultiplier = 10f;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;

    [Header("Jumping")]
    public float jumpForce = 5f;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;
    [SerializeField] float slideDrag = 3f;


    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;
    public bool isGrounded { get; private set; }
    public bool Jumping { get => jumping; }
    public bool HasSecondJump { get => hasSecondJump; set => hasSecondJump = value; }
    public bool Sprinting { get => sprinting; set => sprinting = value; }

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;

    RaycastHit slopeHit;

    Vector2 move;

    private bool jumping = false;
    private bool crouching = false;
    private bool sprinting = false;
    private bool hasSecondJump = false;
    private bool isSliding = false;

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
   
    private void Start()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            camera.SetActive(false);
        }
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        //transform.position = new Vector3(-33, 24, -71);

    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        MyInput();
        ControlDrag();
        ControlSpeed();

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        if (OnSlope() && Vector3.Dot(slopeMoveDirection, transform.up) < 0)
        {
            isSliding = true; // Maybe add with time to build velovity instead of having just a push at crouch
            rb.AddForce(moveDirection.normalized * Time.deltaTime * slideForce, ForceMode.Force);
        }
        else
        {
            isSliding = false;
            if (isGrounded)
                rb.drag = groundDrag;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MyInput()
    {
        horizontalMovement = move.x;
        verticalMovement = move.y;

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;


        jumping = false;

    }

    void Jump()
    {
        if (isGrounded)
        {
            hasSecondJump = true;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        else if (hasSecondJump)
        {
            hasSecondJump = false;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce * 1.5f, ForceMode.Impulse);
        }
    }

    void ControlSpeed()
    {
        if (sprinting && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }

    void ControlDrag()
    {
        if (isGrounded && !isSliding)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    void MovePlayer()
    {
        if (isGrounded && !OnSlope())
        {
            if (crouching)
                moveSpeed *= 0.5f;
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            if (crouching)
                moveSpeed *= 0.5f;
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier * 2, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }

    private void Crouch()
    {
        transform.localPosition -= new Vector3(0, transform.localScale.y * 0.5f, 0);
        transform.localScale *= 0.5f;
        sprinting = false;

        if (rb.velocity.magnitude > 0.5f)
        {
            if (isGrounded)
            {
                if (isSliding)
                {
                    rb.AddForce(slopeMoveDirection.normalized * Time.deltaTime * slideForce * slidingMultiplier, ForceMode.Force);
                    rb.drag = slideDrag;
                }
                else
                {
                    rb.AddForce(moveDirection.normalized * Time.deltaTime * slideForce * 10, ForceMode.Impulse);
                    //rb.drag = groundDrag * 2;
                }
            }
            else
            {
                rb.AddForce(moveDirection.normalized * Time.deltaTime * slideForce * 5, ForceMode.Force);
            }
        }
    }

    private void StopCrouch()
    {
        transform.localPosition += new Vector3(0, transform.localScale.y, 0);
        transform.localScale *= 2f;
        rb.drag = groundDrag;
    }

    #region InputCommands

    private void OnMove(InputValue value)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        move = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        jumping = value.isPressed;
        if (jumping)
        {
            Jump();
        }
    }

    private void OnRun(InputValue value)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        if (value.isPressed)
        {
            sprinting = !sprinting;
            if (crouching)
            {
                crouching = false;
                StopCrouch();
            }
            if (sprinting)
            {
                //cameraMovement.TargetFov = RUNNIG_FOV;
            }
            else
            {
                //cameraMovement.TargetFov = NORMAL_FOV;
            }
        }
    }

    private void OnSlide(InputValue value)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        crouching = !crouching;
        if (crouching)
        {
            Crouch();
        }
        else
        {
            StopCrouch();
        }
    }


    private void OnReload(InputValue value)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        //TODO
        Debug.Log("onReload");

    }

    private void OnMelee(InputValue value)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        //TODO
        Debug.Log("onMelee");

    }


    private void OnOptions(InputValue value)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        //TODO
        //Debug.Log("onOptions");

    }

    private void OnLook_Left(InputValue value)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        //TODO
        Debug.Log("onLook_Left");

    }

    private void OnShoot(InputValue value)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        //TODO
        //Debug.Log("onShoot");

    }

    private void OnLook_Behind(InputValue value)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        //TODO
        Debug.Log("onLook_Behind");

    }

    #endregion
}
