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
    }

    public override void Execute()
    {
        Debug.Log("Executing Jump State");

        //Check for input
        CheckInput();
        CheckStateChange();

        if (!characterController.isGrounded)
        {
            hasJumped = true;
        }

        Move();

        if (characterController.isGrounded)
        {
            hasJumped = false;
            owner.JumpsLeft = playerStats.maxJumps.BaseValue;
            owner.ChangeState(new PlayerMoveState(owner));
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Jump State");
    }

    public override void CheckStateChange()
    {
        if (movementPerformed)
        {
            OnMovementPerformed();
        }

        if (jumpCanceled)
        {
            OnJumpCanceled();
        }

        if (jumpPerformed)
        {
            OnJumpPerformed();
        }

        if (sprintPerformed)
        {
            OnSprintPerformed();
        }

        if (sprintCanceled)
        {
            OnSprintCanceled();
        }

        if (sprintTogglePerformed)
        {
            OnSprintTogglePerformed();
        }

        if (sprintToggleCanceled)
        {
            OnSprintToggleCanceled();
        }

        if (crouchTogglePerformed)
        {
            OnCrouchTogglePerformed();
        }

        if (crouchToggleCanceled)
        {
            OnCrouchToggleCanceled();
        }

        if (dodgePerformed)
        {
            OnDodgePerformed();
        }
    }

    private void Move()
    {
        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity);

        Vector3 displacement = velocity * Time.deltaTime;
        // Move the character controller (note that Move does not include gravity)
        characterController.Move(displacement);
    }

    private void OnMovementPerformed()
    {
        if (hasJumped)
        {
            owner.ChangeState(new PlayerMoveState(owner));
        }
    }


    private void OnJumpPerformed()
    {
        if (!hasPressedJump)
        {
            hasPressedJump = true;
            owner.JumpsLeft--;
            if (owner.JumpsLeft >= 1)
            {
                velocity.y = 0;
                // This formular determines the y velocity to impulse to reach the set jump height
                velocity.y += (Mathf.Sqrt(-2f * (Physics.gravity.y * owner.GravityMultiplier) * playerStats.jumpHeight.BaseValue)) * 
                    (owner.JumpsLeft + 1) / playerStats.maxJumps.BaseValue;
            }
        }
    }

    private void OnJumpCanceled()
    {
        hasPressedJump = false;
    }

    public void OnSprintPerformed()
    {
        sprinting = true;
    }

    public void OnSprintCanceled()
    {
        sprinting = false;
    }

    public void OnSprintTogglePerformed()
    {
        if (!hasPressedSprintToggle)
        {
            hasPressedSprintToggle = true;
            owner.toggleSprint = !owner.toggleSprint;
        }
    }

    private void OnSprintToggleCanceled()
    {
        hasPressedSprintToggle = false;
    }

    private void OnCrouchTogglePerformed()
    {
        if (hasPressedCrouchToggle)
        {
            hasPressedCrouchToggle = true;
            owner.toggleCrouch = !owner.toggleCrouch;
        }
    }

    private void OnCrouchToggleCanceled()
    {
        hasPressedCrouchToggle = false;
    }

    public void OnDodgePerformed()
    {
        if (owner.allowAirDash &&  (playerStamina.stamina - playerStats.dashStaminaCost.BaseValue) >= 0)
        {
            return;
        }
    }
}