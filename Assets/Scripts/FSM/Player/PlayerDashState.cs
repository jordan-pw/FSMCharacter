using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerDashState : MovingState
{
    public PlayerDashState(PlayerController player) : base(player) { }

    private float dashSpeed;
    private float dashDuration;
    private Vector3 dashDirection;

    public override void Enter()
    {
        Debug.Log("Entering Dodge State");

        //Check for input
        CheckInput();
        CheckStateChange();

        // First, subtract stamina
        if ((playerStamina.stamina - playerStats.dashStaminaCost.BaseValue) >= 0)
        {
            playerStamina.stamina  -= playerStats.dashStaminaCost.BaseValue;
        }
        else
            owner.ChangeState(new PlayerMoveState(owner));

        // Change material
        owner.SetMaterial(owner.dashMaterial);
        // Set velocity
        velocity = Vector3.zero;
        // Set dash variables
        dashSpeed = playerStats.dashSpeed.BaseValue;
        dashDuration = playerStats.dashDuration.BaseValue;
        // Set direction, set y velocity to 0, normalize!
        dashDirection = GetPreviousMovingState().velocity;
        dashDirection.y = 0;
        dashDirection = dashDirection.normalized;
    }

    public override void Execute()
    {
        Debug.Log("Executing Dodge State");

        velocity.x = dashDirection.x * dashSpeed * Time.deltaTime;
        velocity.z = dashDirection.z * dashSpeed * Time.deltaTime;
        //velocity.y += gravity * Time.deltaTime;

        dashDuration -= Time.deltaTime;
        if (dashDuration >= 0)
        {
            characterController.Move(velocity);
        }
        else
            owner.ChangeState(new PlayerMoveState(owner));
    }

    public override void Exit()
    {
        Debug.Log("Exiting Dodge State");
    }

}