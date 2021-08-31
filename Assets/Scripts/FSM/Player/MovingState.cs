using System;
using UnityEngine;
using UnityEngine.InputSystem;
public abstract class MovingState : IState
{
    // Initiate fields
    public bool sprinting;
    public bool crouchCheck;
    public Vector3 velocity;
    public Vector3 savedVelocity;

    protected bool movementPerformed;
    protected bool movementCanceled;
    protected bool jumpPerformed;
    protected bool jumpCanceled;
    protected bool sprintPerformed;
    protected bool sprintCanceled;
    protected bool sprintTogglePerformed;
    protected bool sprintToggleCanceled;
    protected bool crouchPerformed;
    protected bool crouchCanceled;
    protected bool crouchTogglePerformed;
    protected bool crouchToggleCanceled;
    protected bool dodgePerformed;
    protected bool dodgeCanceled;

    public bool hasPressedMovement = false;
    public bool hasPressedJump = false;
    public bool hasPressedSprint = false;
    public bool hasPressedSprintToggle = false;
    public bool hasPressedCrouch = false;
    public bool hasPressedCrouchToggle = false;
    public bool hasPressedDodge = false;

    protected bool toggleSprint;
    protected bool toggleCrouch;

    protected float gravity;
    protected float groundedGravity = -1f;
    protected float gravityMultiplier;

    protected PlayerController owner;
    protected CharacterController characterController;
    protected PlayerStats playerStats;
    protected PlayerStamina playerStamina;

    public MovingState(PlayerController player)
    {
        owner = player;
        CheckInput();
        gravity = PlayerController.gravity;
        gravityMultiplier = owner.GravityMultiplier;
        gravity *= gravityMultiplier;
        characterController = player.characterController;
        playerStats = player.PlayerStats;
        playerStamina = player.PlayerStamina;
    }

    public abstract void Enter();

    public abstract void Execute();

    public abstract void Exit();

    public virtual void CheckStateChange() { }

    protected void CheckInput()
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

    protected bool OnSlope()
    { 
        return owner.OnSlope(); 
    }

    protected IState GetPreviousState()
    {
        IState previousState = owner.BodyStateMachine.GetPreviousState();
        return previousState;
    }

    protected MovingState GetPreviousMovingState()
    {
        IState previousState = owner.BodyStateMachine.GetPreviousState();
        MovingState adjustedState = (MovingState)previousState;
        return adjustedState;
    }

}