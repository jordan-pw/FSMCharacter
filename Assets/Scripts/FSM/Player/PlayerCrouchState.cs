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
        crouching = GetPreviousMovingState().crouching;

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

        // Variables for movement
        float speed = owner.GetCrouchSpeed();

        float maxAcceleration = owner.GetMaxAcceleration();

        // Desired velocity is the direction*speed
        Vector3 desiredVelocity =
            new Vector3(movementVector.x * speed, -1f, movementVector.z * speed);

        maxSpeedChange = maxAcceleration * Time.deltaTime;

        // Move the x and z (horz and vert) velocity towards our desired velocity
        velocity.x =
            Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z =
            Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        // Apply gravity, when grounded only apply a small amount to enforce groundedness
        velocity.y = !owner.characterController.isGrounded ?
            velocity.y += gravity * Time.deltaTime : velocity.y = gravity * Time.deltaTime;

        Vector3 displacement = velocity * Time.deltaTime;

        owner.characterController.Move(displacement);

        if (!owner.characterController.isGrounded)
        {
            owner.toggleCrouch = false;
            owner.ChangeState(new PlayerIdleState(owner));
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

    private void OnMovementPerformed()
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
        owner.toggleCrouch = false;
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
        if (!hasPressedCrouchToggle && !crouching)
        {
            hasPressedCrouchToggle = true;
            toggleCrouch = !toggleCrouch;
            owner.toggleCrouch = toggleCrouch;

            if (!toggleCrouch)
            {
                crouching = true;
                owner.ChangeState(new PlayerMoveState(owner));
            }
        }
    }

    private void OnCrouchToggleCanceled()
    {
        hasPressedCrouchToggle = false;
        crouching = false;
    }

    private  void OnDodgePerformed()
    {
        //throw new System.NotImplementedException();
    }
}
