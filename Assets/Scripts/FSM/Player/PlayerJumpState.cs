using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerJumpState : MovingState
{
    private bool hasJumped;

    public PlayerJumpState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        Debug.Log("Entering Jump State");
        toggleSprint = owner.toggleSprint;
        if (toggleSprint)
        {
            sprinting = toggleSprint;
        }
        else sprinting = GetPreviousMovingState().sprinting;
        // Set material
        owner.SetMaterial(owner.jumpMaterial);
        // Set velocity
        velocity = GetPreviousMovingState().velocity;
        // State is active
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
            // Double jump!
            return;
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

    public override void OnSprint(InputAction.CallbackContext context)
    {
        if (isStateActive)
        {
            sprinting = true;
        }
    }

    public override void OnSprintCanceled(InputAction.CallbackContext context)
    {
        if (isStateActive)
        {

            if (toggleSprint == true)
            {
                sprinting = true;
            }
            else
            {
                sprinting = false;
            }
        }
    }

    public override void OnSprintToggle(InputAction.CallbackContext context)
    {
        if (isStateActive)
        {
            owner.toggleSprint = !owner.toggleSprint;
            Debug.Log(owner.toggleSprint);
        }
    }
}