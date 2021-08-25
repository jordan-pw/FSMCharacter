using System;
using UnityEngine;
using UnityEngine.InputSystem;
public abstract class MovingState : IState
{
    // Initiate fields
    public bool movementPerformed;
    public bool movementCanceled;
    public bool jumpPerformed;
    public bool jumpCanceled;
    public bool sprintPerformed;
    public bool sprintCanceled;
    public bool sprintTogglePerformed;
    public bool sprintToggleCanceled;
    public bool crouchPerformed;
    public bool crouchCanceled;
    public bool crouchTogglePerformed;
    public bool crouchToggleCanceled;
    public bool dodgePerformed;
    public bool dodgeCanceled;

    public bool hasPressedMovement = false;
    public bool hasPressedJump = false;
    public bool hasPressedSprint = false;
    public bool hasPressedSprintToggle = false;
    public bool hasPressedCrouch = false;
    public bool hasPressedCrouchToggle = false;
    public bool hasPressedDodge = false;

    public bool toggleSprint;
    public bool sprinting;
    public bool toggleCrouch;
    public bool crouching;

    public float gravity;
    public float gravityMultiplier;

    public Vector3 velocity;

    protected PlayerController owner;

    public MovingState(PlayerController player)
    {
        owner = player;
        CheckInput();
        gravity = owner.GetGravity();
        gravityMultiplier = owner.GetGravityMultiplier();
        gravity *= gravityMultiplier;
    }

    public abstract void Enter();

    public abstract void Execute();

    public abstract void Exit();

    public abstract void CheckStateChange();

    public void CheckInput()
    {
        movementPerformed = InputHandler.movementPerformed;
        movementCanceled = InputHandler.movementCanceled;
        jumpPerformed = InputHandler.jumpPerformed;
        jumpCanceled = InputHandler.jumpCanceled;
        sprintPerformed = InputHandler.sprintPerformed;
        sprintCanceled = InputHandler.sprintCanceled;
        sprintTogglePerformed = InputHandler.sprintTogglePerformed;
        sprintToggleCanceled = InputHandler.sprintToggleCanceled;
        crouchPerformed = InputHandler.crouchPerformed;
        crouchCanceled = InputHandler.crouchCanceled;
        crouchTogglePerformed = InputHandler.crouchTogglePerformed;
        crouchToggleCanceled = InputHandler.crouchToggleCanceled;
        dodgePerformed = InputHandler.dodgePerformed;
        dodgeCanceled = InputHandler.dodgeCanceled;
    }

    public IState GetPreviousState()
    {
        IState previousState = owner.GetBodyStateMachine().GetPreviousState();
        return previousState;
    }

    public MovingState GetPreviousMovingState()
    {
        IState previousState = owner.GetBodyStateMachine().GetPreviousState();
        MovingState adjustedState = (MovingState)previousState;
        return adjustedState;
    }

}