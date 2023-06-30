using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputAction playerInputAction;

    public bool IsJump { get; private set; }

    private void Start()
    {
        GetJumpInput();
    }

    private void OnEnable()
    {
        if (playerInputAction == null)
        {
            playerInputAction = new PlayerInputAction();
        }
        
        playerInputAction.Enable();
    }

    private void OnDisable()
    {
        playerInputAction.Player.Jump.started -= JumpOnPerformed;
        playerInputAction.Player.Jump.canceled -= context => IsJump = false;
        playerInputAction.Disable();
    }
    

    public float GetHorizontalInput()
    {
        return playerInputAction.Player.Movement.ReadValue<Vector2>().x;
    }
    
    public float GetVerticalInput()
    {
        return playerInputAction.Player.Movement.ReadValue<Vector2>().y;
    }

    private void GetJumpInput()
    {
        playerInputAction.Player.Jump.started += JumpOnPerformed;
        playerInputAction.Player.Jump.canceled += context => IsJump = false;
    }

    private void JumpOnPerformed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsJump = true;
        }
    }

    public void UsedJumpInput()
    {
        IsJump = false;
    }
}
