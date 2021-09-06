using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerIdleState : MovingState
{
    public PlayerIdleState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        Debug.Log("Entering Idle State");

        if (crouchCheck = (GetPreviousMovingState() != null))
        {
            crouchCheck = GetPreviousMovingState().crouchCheck;
        }
        else crouchCheck = false;

        // Change material
        owner.SetMaterial(owner.idleMaterial);
        // Set velocity
        velocity = Vector3.zero;
    }

    public override void Execute()
    {
        Debug.Log("Executing Idle State");

        //Check for input
        CheckInput();
        CheckStateChange();

        if (owner.toggleCrouch) owner.ChangeState(new PlayerCrouchState(owner));
        // Even in idle, gravity must apply.
        characterController.Move(new Vector3(0, gravity, 0) * Time.deltaTime);
    }

    public override void Exit()
    {
        Debug.Log("Exiting Idle State");
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

    private void OnMovementPerformed()
    {
        owner.ChangeState(new PlayerMoveState(owner));
    }

    private void OnJumpPerformed()
    {
        owner.ChangeState(new PlayerJumpState(owner));
    }

    private void OnSprintPerformed()
    {
        sprinting = true;
    }

    private void OnSprintCanceled()
    {
        sprinting = false;
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
        owner.ChangeState(new PlayerCrouchState(owner));
    }

    private void OnCrouchTogglePerformed()
    {
        if (!hasPressedCrouchToggle && !crouchCheck)
        {
            owner.toggleCrouch = true;
            crouchCheck = true;
            owner.ChangeState(new PlayerCrouchState(owner));
        }
    }

    private void OnCrouchToggleCanceled()
    {
        hasPressedCrouchToggle = false;
        crouchCheck = false;
    }

    private void OnDodgePerformed()
    {
        // Set velocity to how we moved last, so that the dash has a direction
        velocity = GetPreviousMovingState().savedVelocity;
        Debug.Log(velocity);
        owner.ChangeState(new PlayerDashState(owner));
    }
}