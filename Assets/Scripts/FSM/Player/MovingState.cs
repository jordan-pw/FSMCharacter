using UnityEngine;
using UnityEngine.InputSystem;
public abstract class MovingState : IState
{
    // Initiate fields
    public InputAction movement;
    public InputAction jump;

    public float gravity;
    public float gravityMultiplier;
    public Vector3 velocity;

    protected bool isStateActive;

    protected PlayerController owner;

    private PlayerInputActions playerInputActions;
    private InputActionMap playerActionMap;

    public abstract void Enter();

    public abstract void Execute();

    public abstract void Exit();

    // Event happens when jump input is used
    public abstract void OnJump(InputAction.CallbackContext context);

    // Event happens when movement input are used
    public abstract void OnMovement(InputAction.CallbackContext context);

    public void InitiateInputSystem()
    {
        // Instantiate input system components
        playerInputActions = new PlayerInputActions();
        playerActionMap = playerInputActions.Player.Get();

        // Find actions and assign them to input actions
        movement = playerActionMap.FindAction("Movement");
        jump = playerActionMap.FindAction("Jump");

        // Create events
        movement.performed += OnMovement;
        jump.performed += OnJump;

        // Enable input actions
        movement.Enable();
        jump.Enable();
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