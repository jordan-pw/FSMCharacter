using System;
using UnityEngine;
using UnityEngine.InputSystem;
public abstract class HeadState : IState
{
    // Initiate fields
    protected Vector2 mouseVector;
    protected Vector2 relativeMouseVector;

    protected PlayerController owner;
    protected CharacterController characterController;
    protected PlayerStats playerStats;
    protected PlayerStamina playerStamina;
    protected Transform inputSpace;

    public HeadState(PlayerController player)
    {
        owner = player;
        CheckInput();
        characterController = player.characterController;
        playerStats = player.PlayerStats;
        inputSpace = player.playerInputSpace;
    }

    public abstract void Enter();

    public abstract void Execute();

    public abstract void Exit();

    public virtual void CheckStateChange() { }

    protected void CheckInput()
    {
        mouseVector = InputHandler.mouseVector;
    }

    protected Vector3 ConvertMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToViewportPoint(mouseVector);
        relativeMouseVector = (mousePosition - new Vector3(0.5f, 0.5f, 0.0f)).normalized;
        return relativeMouseVector;
    }

    protected IState GetPreviousState()
    {
        IState previousState = owner.HeadStateMachine.GetPreviousState();
        return previousState;
    }

    protected HeadState GetPreviousHeadState()
    {
        IState previousState = owner.HeadStateMachine.GetPreviousState();
        HeadState adjustedState = (HeadState)previousState;
        return adjustedState;
    }

}