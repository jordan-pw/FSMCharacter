using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMoveState : MovingState
{
    private Vector3 movementVector;

    private float maxSpeedChange;
    private float sprintSpeed;

    private bool isCrouchToggled;

    public PlayerMoveState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        Debug.Log("Entering Move State");
        crouching = GetPreviousMovingState().crouching;
        // Set velocity
        velocity = GetPreviousMovingState().velocity;
        // Set sprint fields
        sprintSpeed = owner.GetSprintSpeed();
        toggleSprint = owner.toggleSprint;
        if (toggleSprint)
        {
            sprinting = toggleSprint;
        }
        else sprinting = GetPreviousMovingState().sprinting;
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

        // Variables for movement
        float speed;
        if (owner.characterController.isGrounded)
        {
            speed = sprinting ? sprintSpeed : owner.GetMaxSpeed();
        }  // Can't sprint in the air
        else speed = owner.GetMaxSpeed();

        float maxAcceleration = owner.GetMaxAcceleration();
        float maxAirAcceleration = owner.GetMaxAirAcceleration();

        // Desired velocity is the direction*speed
        Vector3 desiredVelocity = 
            new Vector3(movementVector.x * speed, -1f, movementVector.z * speed);

        // Max speed on change on ground is fast and responsive
        // Max speed change in the air is slower and less responsive
        maxSpeedChange = owner.characterController.isGrounded ? 
            maxSpeedChange = maxAcceleration * Time.deltaTime : 
            maxSpeedChange = maxAirAcceleration * Time.deltaTime;

        // Move the x and z (horz and vert) velocity towards our desired velocity
        velocity.x =
            Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z =
            Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        // Apply gravity, when grounded only apply a small amount to enforce groundedness
        velocity.y = !owner.characterController.isGrounded ?
            velocity.y += gravity * Time.deltaTime : velocity.y = gravity * Time.deltaTime;

        Vector3 displacement = velocity * Time.deltaTime;

        // Move the character controller (note that Move does not include gravity)
        owner.characterController.Move(displacement);

        // If the velocity is 0, and the character has already moved, when the character is not moving return to idle
        if (velocity.x == 0 && velocity.z == 0 && owner.characterController.isGrounded)
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

    // Event happens when movement input are used
    private void OnMovementPerformed()
    {
        movementVector = InputHandler.movementVector;
    }

    private void OnMovementCanceled()
    {
        movementVector = InputHandler.movementVector;
    }

    private void OnJumpPerformed()
    {
        if (owner.characterController.isGrounded)
        {
            owner.ChangeState(new PlayerJumpState(owner));
        }
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
        if (owner.characterController.isGrounded)
        {
            owner.ChangeState(new PlayerCrouchState(owner));
        }
    }

    private void OnCrouchTogglePerformed()
    {
        if (!hasPressedCrouchToggle && !crouching)
        {
            owner.toggleCrouch = !owner.toggleCrouch;
            crouching = true;
            owner.ChangeState(new PlayerCrouchState(owner));
        }
    }

    private void OnCrouchToggleCanceled()
    {
        hasPressedCrouchToggle = false;
        crouching = false;
    }


    private void OnDodgePerformed()
    {
        throw new System.NotImplementedException();
    }
}