using System;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputHandler
{
    private static PlayerInputActions playerInputActions = new PlayerInputActions();
    private static InputActionMap playerActionMap = playerInputActions.Player.Get();
    private static InputActionMap cameraActionMap = playerInputActions.Camera.Get();

    // Player Movement
    public static InputAction movement = playerActionMap.FindAction("Movement");
    public static InputAction jump = playerActionMap.FindAction("Jump");
    public static InputAction sprint = playerActionMap.FindAction("Sprint");
    public static InputAction sprintToggle = playerActionMap.FindAction("SprintToggle");
    public static InputAction crouch = playerActionMap.FindAction("Crouch");
    public static InputAction crouchToggle = playerActionMap.FindAction("CrouchToggle");
    public static InputAction dodge = playerActionMap.FindAction("Dodge");
    public static InputAction mousePosition = playerActionMap.FindAction("MousePosition");

    // Camera controls
    public static InputAction rotate = cameraActionMap.FindAction("Rotate");

    public static Vector3 movementVector;
    public static Vector2 mouseVector;
    public static float rotateAxis;

    // Movement values
    public static bool movementPerformed = false;
    public static bool movementCanceled = false;
    public static bool jumpPerformed = false;
    public static bool jumpCanceled = false;
    public static bool sprintPerformed = false;
    public static bool sprintCanceled = false;
    public static bool sprintTogglePerformed = false;
    public static bool sprintToggleCanceled = false;    
    public static bool crouchPerformed = false;
    public static bool crouchCanceled = false;
    public static bool crouchTogglePerformed = false;
    public static bool crouchToggleCanceled = false;
    public static bool dodgePerformed = false;
    public static bool dodgeCanceled = false;

    // Camera values
    public static bool rotatePerformed = false;
    public static bool rotateCanceled = false;


    public static void OnEnable()
    {
        // Enable input actions
        movement.Enable();
        jump.Enable();
        sprint.Enable();
        sprintToggle.Enable();
        crouch.Enable();
        crouchToggle.Enable();
        dodge.Enable();
        mousePosition.Enable();

        // Enable camera rotation
        rotate.Enable();

        // Subscribe
        movement.performed += OnMovement;
        movement.canceled += OnMovementCanceled;
        jump.performed += OnJump;
        jump.canceled += OnJumpCanceled;
        sprint.performed += OnSprint;
        sprint.canceled += OnSprintCanceled;
        sprintToggle.performed += OnSprintToggle;
        sprintToggle.canceled += OnSprintToggleCanceled;
        crouch.performed += OnCrouch;
        crouch.canceled += OnCrouchCanceled;
        crouchToggle.performed += OnCrouchToggle;
        crouchToggle.canceled += OnCrouchToggleCanceled;
        dodge.performed += OnDodge;
        dodge.canceled += OnDodgeCanceled;
        mousePosition.performed += mousePositionPerformed;

        rotate.performed += OnRotate;
        rotate.canceled += OnRotateCanceled;
    }

    public static void OnDisable()
    {
        // Disable input actions
        movement.Disable();
        jump.Disable();
        sprint.Disable();
        sprintToggle.Disable();
        crouch.Disable();
        crouchToggle.Disable();
        dodge.Disable();
        mousePosition.Disable();

        // Disable camera rotation
        rotate.Disable();

        movement.performed -= OnMovement;
        movement.canceled -= OnMovementCanceled;
        jump.performed -= OnJump;
        jump.canceled -= OnJumpCanceled;
        sprint.performed -= OnSprint;
        sprint.canceled -= OnSprintCanceled;
        sprintToggle.performed -= OnSprintToggle;
        sprintToggle.canceled -= OnSprintToggleCanceled;
        crouch.performed -= OnCrouch;
        crouch.canceled -= OnCrouchCanceled;
        crouchToggle.performed -= OnCrouchToggle;
        crouchToggle.canceled -= OnCrouchToggleCanceled;
        dodge.performed -= OnDodge;
        dodge.canceled -= OnDodgeCanceled;
        mousePosition.performed -= mousePositionPerformed;

        rotate.performed -= OnRotate;
        rotate.canceled -= OnRotateCanceled;
    }
    private static void OnMovement(InputAction.CallbackContext obj)
    {
        movementCanceled = false;
        movementPerformed = true;
        movementVector = new Vector3(
            obj.ReadValue<Vector2>().x,
            0f,
            obj.ReadValue<Vector2>().y
            );
    }

    private static void OnMovementCanceled(InputAction.CallbackContext obj)
    {
        movementPerformed = false;
        movementCanceled = true;

        movementVector = new Vector3(
            obj.ReadValue<Vector2>().x,
            0f,
            obj.ReadValue<Vector2>().y
            );

    }

    private static void OnJump(InputAction.CallbackContext obj)
    {
        jumpCanceled = false;
        jumpPerformed = true;
    }

    private static void OnJumpCanceled(InputAction.CallbackContext obj)
    {
        jumpPerformed = false;
        jumpCanceled = true;
    }

    private static void OnSprint(InputAction.CallbackContext obj)
    {
        sprintCanceled = false;
        sprintPerformed = true;
    }

    private static void OnSprintCanceled(InputAction.CallbackContext obj)
    {
        sprintPerformed = false;
        sprintCanceled = true;
    }

    private static void OnSprintToggle(InputAction.CallbackContext obj)
    {
        sprintToggleCanceled = false;
        sprintTogglePerformed = true;
    }

    private static void OnSprintToggleCanceled(InputAction.CallbackContext obj)
    {
        sprintTogglePerformed = false;
        sprintToggleCanceled = true;
    }

    private static void OnCrouch(InputAction.CallbackContext obj)
    {
        crouchCanceled = false;
        crouchPerformed = true;
    }

    private static void OnCrouchCanceled(InputAction.CallbackContext obj)
    {
        crouchPerformed = false;
        crouchCanceled = true;
    }

    private static void OnCrouchToggle(InputAction.CallbackContext obj)
    {
        crouchToggleCanceled = false;
        crouchTogglePerformed = true;
    }

    private static void OnCrouchToggleCanceled(InputAction.CallbackContext obj)
    {
        crouchTogglePerformed = false;
        crouchToggleCanceled = true;
    }

    private static void OnDodge(InputAction.CallbackContext obj)
    {
        dodgeCanceled = false;
        dodgePerformed = true;
    }

    private static void OnDodgeCanceled(InputAction.CallbackContext obj)
    {
        dodgePerformed = false;
        dodgeCanceled = true;
    }


    private static void mousePositionPerformed(InputAction.CallbackContext obj)
    {
        mouseVector = obj.ReadValue<Vector2>();
    }

    private static void OnRotate(InputAction.CallbackContext obj)
    {
        rotateCanceled = false;
        rotatePerformed = true;

        rotateAxis = obj.ReadValue<float>();
    }

    private static void OnRotateCanceled(InputAction.CallbackContext obj)
    {
        rotatePerformed = false;
        rotateCanceled = true;

        rotateAxis = 0f;

    }
}
