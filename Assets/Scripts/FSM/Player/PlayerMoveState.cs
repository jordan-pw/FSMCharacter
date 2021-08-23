using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMoveState : MovingState
{
    private Vector3 move;

    private float maxSpeedChange;
    private float sprintSpeed;

    public PlayerMoveState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        Debug.Log("Entering Move State");
        // Callbacks for when movement keys are used
        movement.canceled += OnMovementCanceled;
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
        isStateActive = true;
    }

    public override void Execute()
    {
        if (isStateActive)
        {
            Debug.Log("Executing Move State");

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
                new Vector3(move.x * speed, -1f, move.z * speed);

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
                isStateActive = false;
                owner.ChangeState(new PlayerIdleState(owner));
            }
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Move State");
        isStateActive = false;
    }

    // Event happens when jump input is used
    public override void OnJump(InputAction.CallbackContext context)
    {
        if (isStateActive && owner.characterController.isGrounded)
        {
            // When jumping, change to jump state
            isStateActive = false;
            owner.ChangeState(new PlayerJumpState(owner));
        }
    }

    // Event happens when movement input are used
    public override void OnMovement(InputAction.CallbackContext context)
    {
        if (isStateActive)
        {
            // Create a move vector with the horizontal and vertical input axis's
            move = new Vector3(
                context.ReadValue<Vector2>().x, 
                0f, 
                context.ReadValue<Vector2>().y
                );
        }
    }

    public override void OnSprint(InputAction.CallbackContext context)
    {
        if (isStateActive)
        {
            sprinting = true;
            owner.SetMaterial(owner.sprintMaterial);
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
                owner.SetMaterial(owner.moveMaterial);
            }
        }
    }

    public override void OnSprintToggle(InputAction.CallbackContext context)
    {
        if (isStateActive)
        {
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

    // Event happens when movement input are used
    public void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (isStateActive)
        {
            // This sets the x and y to 0, since when movement is canceled, there is no input
            move = new Vector3(
                context.ReadValue<Vector2>().x,
                0f,
                context.ReadValue<Vector2>().y
                );
        }
    }

    public override void OnCrouch(InputAction.CallbackContext context)
    {
        if (isStateActive && owner.characterController.isGrounded)
        {
            owner.ChangeState(new PlayerCrouchState(owner));
        }
    }

    public override void OnCrouchCanceled(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnCrouchToggle(InputAction.CallbackContext context)
    {
        if (isStateActive)
        {
            owner.toggleCrouch = !owner.toggleCrouch;
            if (owner.toggleCrouch)
            {
                owner.ChangeState(new PlayerCrouchState(owner));
            }
        }
    }

    public override void OnDodge(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
}