using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerJumpState : MovingState
{
    private bool hasJumped;

    public PlayerJumpState(PlayerController player)
    {
        owner = player;
        InitiateInputSystem();
    }

    public override void Enter()
    {
        // Instantiate input system components
        Debug.Log("Entering Jump State");
        jump.canceled += OnJumpCanceled;
        owner.SetMaterial(owner.jumpMaterial);
        // Set gravity
        gravity = owner.GetGravity();
        gravityMultiplier = owner.GetGravityMultiplier();
        gravity *= gravityMultiplier;
        // Set velocity
        velocity = GetPreviousMovingState().velocity;

        isStateActive = true;
        // Jump!
        velocity.y += Mathf.Sqrt(-2f * Physics.gravity.y * 5f);
    }

    public override void Execute()
    {
        if (isStateActive)
        {
            Debug.Log("Executing Jump State");

            if (!owner.characterController.isGrounded)
            {
                hasJumped = true;
            }

            // Apply gravity
            velocity.y += gravity * Time.deltaTime;
            velocity.y = Mathf.Max(velocity.y, gravity);

            Vector3 displacement = velocity * Time.deltaTime;
            // Move the character controller (note that Move does not include gravity)
            owner.characterController.Move(displacement);

            if (owner.characterController.isGrounded)
            {
                hasJumped = false;
                owner.ChangeState(new PlayerMoveState(owner));
            }
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Jump State");
        isStateActive = false; ;
    }

    // Event happens when jump input is used
    public override void OnJump(InputAction.CallbackContext context)
    {
        if (isStateActive)
        {
            return;
        }
    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        if (isStateActive)
        {
            //owner.ChangeState(new PlayerIdleState(owner));
        }
    }

    // Event happens when movement inputs are used
    public override void OnMovement(InputAction.CallbackContext context)
    {
        if (isStateActive && hasJumped)
        {
            owner.ChangeState(new PlayerMoveState(owner));
        }
    }

}