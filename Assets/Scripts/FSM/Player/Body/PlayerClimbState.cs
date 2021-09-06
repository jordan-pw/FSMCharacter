using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbState : MovingState
{
    private Vector3 movementVector;

    public PlayerClimbState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        Debug.Log("Entering Climb State");

        // Change material
        owner.SetMaterial(owner.climbMaterial);
        // Set velocity
        velocity = Vector3.zero;
    }

    public override void Execute()
    {
        Debug.Log("Executing Climb State");

        //Check for input
        CheckInput();
        CheckStateChange();

        Move();

        if (!owner.touchingLadder || (characterController.collisionFlags == CollisionFlags.None))
        {
            //owner.ChangeState(new PlayerMoveState(owner));
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Climb State");
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
    }

    private void Move()
    {
        // Variables for movement
        float speed;
        speed = playerStats.maxSpeed.BaseValue;

        float maxAcceleration = playerStats.maxAcceleration.BaseValue;

        // Desired velocity is the direction*speed
        float desiredVelocity;

        desiredVelocity = movementVector.z * speed;

        // Max speed on change on ground is fast and responsive
        // Max speed change in the air is slower and less responsive
        float maxSpeedChange = maxAcceleration * Time.deltaTime;

        // Move the x and z (horz and vert) velocity towards our desired velocity
        velocity.y =
            Mathf.MoveTowards(velocity.y, desiredVelocity, maxSpeedChange);

        Vector3 displacement = velocity * Time.deltaTime;

        // Move the character controller (note that Move does not include gravity)
        characterController.Move(displacement);
    }

    private void OnMovementPerformed()
    {
        movementVector = InputHandler.movementVector;
        savedVelocity = velocity;
    }

    private void OnMovementCanceled()
    {
        movementVector = InputHandler.movementVector;
    }

    private void OnJumpPerformed()
    {
        owner.ChangeState(new PlayerJumpState(owner));
    }

}
