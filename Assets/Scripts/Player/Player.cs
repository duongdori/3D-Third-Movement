using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera playerCamera;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private float yVelocity;
    private float gravity = -9.81f;

    [SerializeField] private float rotateSpeed = 720f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isJumping;


    [SerializeField] private Transform groundCheckPosition;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundCheckLayerMask;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();

        if (Camera.main != null)
        {
            playerCamera = Camera.main;
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        characterController.Move(moveDirection * (moveSpeed * Time.deltaTime));

    }

    private void HandleMovement()
    {
        horizontalInput = playerInput.GetHorizontalInput();
        verticalInput = playerInput.GetVerticalInput();
        
        moveDirection.Set(horizontalInput, 0f, verticalInput);
        moveDirection.Normalize();
        
        moveDirection = Quaternion.AngleAxis(playerCamera.transform.rotation.eulerAngles.y, Vector3.up) * moveDirection;

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotate = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, rotateSpeed * Time.deltaTime);
        }
        
    }

    private void HandleJump()
    {
        isGrounded = Physics.CheckSphere(groundCheckPosition.position, groundCheckRadius, groundCheckLayerMask);

        if (isGrounded && yVelocity < 0)
        {
            yVelocity = -2f;
            isJumping = false;
        }
        
        if (playerInput.IsJump && isGrounded)
        {
            yVelocity += Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJumping = true;
            playerInput.UsedJumpInput();
        }
        
        yVelocity += gravity * Time.deltaTime;
        moveDirection.y = yVelocity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheckPosition.position, groundCheckRadius);
    }
}
