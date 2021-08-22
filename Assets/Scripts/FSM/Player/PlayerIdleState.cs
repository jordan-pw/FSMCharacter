using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerIdleState : MovingState
{
    public PlayerIdleState(PlayerController player)
    {
        owner = player;
        InitiateInputSystem();
    }

    public override void Enter()
    {
        // Instantiate input system components
        Debug.Log("Entering Idle State");
        isStateActive = true;
        owner.SetMaterial(owner.idleMaterial);

        velocity = Vector3.zero;

        gravity = owner.GetGravity();
        gravityMultiplier = owner.GetGravityMultiplier();
        gravity *= gravityMultiplier;
    }

    public override void Execute()
    {
        Debug.Log("Executing Idle State");

        // Even in idle, gravity must apply.
        // In the future, this will be replaced by a falling state
        owner.characterController.Move(new Vector3(0, gravity, 0) * Time.deltaTime);
    }

    public override void Exit()
    {
        Debug.Log("Exiting Idle State");
        isStateActive = false; ;
    }

    // Event happens when jump input is used
    public override void OnJump(InputAction.CallbackContext context)
    {
        if (isStateActive)
        {
            // When jumping, change to jump state
            owner.ChangeState(new PlayerJumpState(owner));
        }
    }

    // Event happens when movement input are used
    public override void OnMovement(InputAction.CallbackContext context)
    {
        if (isStateActive)
        {
            // When moving, change to jump state
            owner.ChangeState(new PlayerMoveState(owner));
        }
    }

}