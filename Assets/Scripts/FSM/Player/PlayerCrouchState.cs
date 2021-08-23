using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerCrouchState : MovingState
{
    private Vector3 move;

    private float maxSpeedChange;

    public PlayerCrouchState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        // Instantiate input system components
        Debug.Log("Entering Crouch State");
        movement.canceled += OnMovementCanceled;
        toggleCrouch = owner.toggleCrouch;

        // Set velocity
        velocity = GetPreviousMovingState().velocity;
        // Change material
        owner.SetMaterial(owner.crouchMaterial);
        // State is active
        isStateActive = true;
        crouching = true;
    }

    public override void Execute()
    {
        Debug.Log("Executing Crouch State");

        // Variables for movement
        float speed = owner.GetCrouchSpeed();

        float maxAcceleration = owner.GetMaxAcceleration();

        // Desired velocity is the direction*speed
        Vector3 desiredVelocity =
            new Vector3(move.x * speed, -1f, move.z * speed);

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
        isStateActive = false;
        crouching = false;
    }

    // Event happens when jump input is used
    public override void OnJump(InputAction.CallbackContext context)
    {
        if (isStateActive)
        {
            // When jumping, change to jump state
            owner.toggleCrouch = false;
            owner.ChangeState(new PlayerJumpState(owner));
        }
    }

    // Event happens when movement input are used
    public override void OnMovement(InputAction.CallbackContext context)
    {
        if (isStateActive)
        {
            // Create a move vector with the horizontal and vertical input axis's
            move = new Vector3(
                context.ReadValue<Vector2>().x,
                0f,
                context.ReadValue<Vector2>().y
                );
        }
    }

    public override void OnSprint(InputAction.CallbackContext context)
    {
        if (isStateActive)
        {
            sprinting = true;
            owner.toggleCrouch = false;
            owner.ChangeState(new PlayerMoveState(owner));
        }
    }

    public override void OnSprintCanceled(InputAction.CallbackContext context)
    {
        if (isStateActive)
        {

            if (toggleSprint == true)
            {
                sprinting = true;
            }
            else
            {
                sprinting = false;
            }
        }
    }

    // Event happens when movement input are used
    public void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (isStateActive)
        {
            // This sets the x and y to 0, since when movement is canceled, there is no input
            move = new Vector3(
                context.ReadValue<Vector2>().x,
                0f,
                context.ReadValue<Vector2>().y
                );
        }
    }

    public override void OnSprintToggle(InputAction.CallbackContext context)
    {
        if (isStateActive)
        {
            owner.toggleSprint = !owner.toggleSprint;
            Debug.Log(owner.toggleSprint);
        }
    }

    public override void OnCrouch(InputAction.CallbackContext context)
    {
        if (isStateActive)
        {
            return;
        }
    }

    public override void OnCrouchCanceled(InputAction.CallbackContext context)
    {
        if (isStateActive)
        {
            owner.ChangeState(new PlayerMoveState(owner));
        }
    }

    public override void OnCrouchToggle(InputAction.CallbackContext context)
    {
        if (isStateActive)
        {
            toggleCrouch = !toggleCrouch;
            owner.toggleCrouch = toggleCrouch;

            Debug.Log(toggleCrouch);
            Debug.Log(owner.toggleCrouch);
            if (!toggleCrouch)
            {
                owner.ChangeState(new PlayerMoveState(owner));
            }
        }
    }

    public override void OnDodge(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }
}
