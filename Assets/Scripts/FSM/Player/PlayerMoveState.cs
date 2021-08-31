using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMoveState : MovingState
{
    private Vector3 movementVector;

    private float maxSpeedChange;
    private float sprintSpeed;

    public PlayerMoveState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        Debug.Log("Entering Move State");
        // Set crouch
        crouchCheck = GetPreviousMovingState().crouchCheck;
        hasPressedJump = GetPreviousMovingState().hasPressedJump;

        // Set velocity
        velocity = GetPreviousMovingState().velocity;

        // Set sprint fields
        sprintSpeed = playerStats.sprintSpeed.BaseValue;
        SetSprint();

        // Change material
        if (!sprinting) owner.SetMaterial(owner.moveMaterial);
        else owner.SetMaterial(owner.sprintMaterial);
        // State is active and the character hasn't moved yet
    }

    public override void Execute()
    {
        Debug.Log("Executing Move State");

        //Check for input
        CheckInput();
        CheckStateChange();

        Move();

        if (owner.toggleCrouch && !sprinting && characterController.isGrounded)
        {
            owner.ChangeState(new PlayerCrouchState(owner));
        }

        // If the velocity is 0, and the character has already moved, when the character is not moving return to idle
        if (velocity.x == 0 && velocity.z == 0 && characterController.isGrounded)
        {
            owner.ChangeState(new PlayerIdleState(owner));
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Move State");
    }

    public override void CheckStateChange()
    {
        if (movementPerformed)
        {
            OnMovementPerformed();
        }

        if (movementCanceled)
        {
            OnMovementCanceled();
        }

        if (jumpPerformed)
        {
            OnJumpPerformed();
        }

        if (jumpCanceled)
        {
            OnJumpCanceled();
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

        if (crouchPerformed)
        {
            OnCrouchPerformed();
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
        // Variables for movement
        float speed;
        if (characterController.isGrounded)
        {
            speed = sprinting ? sprintSpeed : playerStats.maxSpeed.BaseValue;
        }  // Can't sprint in the air
        else speed = playerStats.maxSpeed.BaseValue;

        float maxAcceleration = playerStats.maxAcceleration.BaseValue;
        float maxAirAcceleration = playerStats.maxAirAcceleration.BaseValue;

        // Desired velocity is the direction*speed
        Vector3 desiredVelocity =
            new Vector3(movementVector.x * speed, -1f, movementVector.z * speed);

        // Max speed on change on ground is fast and responsive
        // Max speed change in the air is slower and less responsive
        maxSpeedChange = characterController.isGrounded ?
            maxSpeedChange = maxAcceleration * Time.deltaTime :
            maxSpeedChange = maxAirAcceleration * Time.deltaTime;

        // Move the x and z (horz and vert) velocity towards our desired velocity
        velocity.x =
            Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z =
            Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        SetGravity();

        Vector3 displacement = velocity * Time.deltaTime;

        // Move the character controller (note that Move does not include gravity)
        characterController.Move(displacement);
    }

    private void SetGravity()
    {
        groundedGravity = OnSlope() ? -characterController.stepOffset / Time.deltaTime : -1f;

        // Apply gravity, when grounded only apply a small amount to enforce groundedness
        velocity.y = !characterController.isGrounded ?
            velocity.y += gravity * Time.deltaTime : velocity.y = groundedGravity;
    }

    private void SetSprint()
    {
        toggleSprint = owner.toggleSprint;
        if (toggleSprint)
        {
            sprinting = toggleSprint;
        }
        else sprinting = GetPreviousMovingState().sprinting;
    }


    // Event happens when movement input are used
    private void OnMovementPerformed()
    {
        movementVector = InputHandler.movementVector;
        savedVelocity = velocity;
    }

    private void OnMovementCanceled()
    {
        movementVector = InputHandler.movementVector;
    }

    private void OnJumpPerformed()
    {
        if (!hasPressedJump)
        {
            hasPressedJump = true;
            if (owner.JumpsLeft >= 1)
            {
                owner.ChangeState(new PlayerJumpState(owner));
            }
        }
    }

    private void OnJumpCanceled()
    {
        hasPressedJump = false;
    }

    private void OnSprintPerformed()
    {
        sprinting = true;
        owner.SetMaterial(owner.sprintMaterial);
    }

    private void OnSprintCanceled()
    {
        if (!toggleSprint)
        {
            sprinting = false;
            owner.SetMaterial(owner.moveMaterial);
        }
    }

    private void OnSprintTogglePerformed()
    {
        if (!hasPressedSprintToggle)
        {
            hasPressedSprintToggle = true;

            toggleSprint = !toggleSprint;
            sprinting = toggleSprint;
            owner.toggleSprint = toggleSprint;

            if (toggleSprint)
            {
                owner.SetMaterial(owner.sprintMaterial);
            }
            else
            {
                owner.SetMaterial(owner.moveMaterial);
            }
        }
    }

    private void OnSprintToggleCanceled()
    {
        hasPressedSprintToggle = false;
    }

    private void OnCrouchPerformed()
    {
        if (characterController.isGrounded)
        {
            owner.ChangeState(new PlayerCrouchState(owner));
        }
    }

    private void OnCrouchTogglePerformed()
    {
        if (!hasPressedCrouchToggle && !crouchCheck)
        {
            owner.toggleCrouch = !owner.toggleCrouch;
            crouchCheck = true;
            if (!sprinting) owner.ChangeState(new PlayerCrouchState(owner));
        }
    }

    private void OnCrouchToggleCanceled()
    {
        hasPressedCrouchToggle = false;
        crouchCheck = false;
    }


    private void OnDodgePerformed()
    {
        if ((playerStamina.stamina - playerStats.dashStaminaCost.BaseValue) >= 0)
        {
            owner.ChangeState(new PlayerDashState(owner));
        }
    }
}