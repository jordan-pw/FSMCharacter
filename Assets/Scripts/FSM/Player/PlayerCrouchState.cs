using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerCrouchState : MovingState
{
    private Vector3 movementVector;

    private float maxSpeedChange;

    public PlayerCrouchState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        Debug.Log("Entering Crouch State");
        toggleCrouch = owner.toggleCrouch;
        crouchCheck = GetPreviousMovingState().crouchCheck;

        // Set velocity
        velocity = GetPreviousMovingState().velocity;
        // Change material
        owner.SetMaterial(owner.crouchMaterial);
    }

    public override void Execute()
    {
        Debug.Log("Executing Crouch State");

        //Check for input
        CheckInput();
        CheckStateChange();

        Move();

        if (!characterController.isGrounded)
        {
            owner.ChangeState(new PlayerMoveState(owner));
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Crouch State");
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

        if (crouchCanceled)
        {
            OnCrouchCanceled();
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
        float speed = playerStats.crouchSpeed.BaseValue;

        float maxAcceleration = playerStats.maxAcceleration.BaseValue;

        // Desired velocity is the direction*speed
        Vector3 desiredVelocity =
            new Vector3(movementVector.x * speed, -1f, movementVector.z * speed);

        maxSpeedChange = maxAcceleration * Time.deltaTime;

        // Move the x and z (horz and vert) velocity towards our desired velocity
        velocity.x =
            Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z =
            Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        SetGravity();

        Vector3 displacement = velocity * Time.deltaTime;

        characterController.Move(displacement);
    }

    private void SetGravity()
    {
        groundedGravity = OnSlope() ? -characterController.stepOffset / Time.deltaTime : -1f;

        // Apply gravity, when grounded only apply a small amount to enforce groundedness
        velocity.y = !characterController.isGrounded ?
            velocity.y += gravity * Time.deltaTime : velocity.y = groundedGravity;
    }


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
        owner.toggleCrouch = false;
        owner.ChangeState(new PlayerJumpState(owner));
    }

    private void OnSprintPerformed()
    {
        sprinting = true;
        //owner.toggleCrouch = false;
        owner.ChangeState(new PlayerMoveState(owner));
    }

    private void OnSprintTogglePerformed()
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

    private void OnCrouchPerformed()
    {
        hasPressedCrouch = true;
    }

    private void OnCrouchCanceled()
    {
        if (hasPressedCrouch)
        {
            hasPressedCrouch = false;
            owner.ChangeState(new PlayerMoveState(owner));
        }
    }

    private  void OnCrouchTogglePerformed()
    {
        if (!hasPressedCrouchToggle && !crouchCheck)
        {
            hasPressedCrouchToggle = true;
            toggleCrouch = !toggleCrouch;
            owner.toggleCrouch = toggleCrouch;

            if (!toggleCrouch)
            {
                crouchCheck = true;
                owner.ChangeState(new PlayerMoveState(owner));
            }
        }
    }

    private void OnCrouchToggleCanceled()
    {
        hasPressedCrouchToggle = false;
        crouchCheck = false;
    }

    private  void OnDodgePerformed()
    {
        //throw new System.NotImplementedException();
    }
}
