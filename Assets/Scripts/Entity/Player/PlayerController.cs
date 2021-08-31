using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static readonly float gravity = -9.81f;

    public bool allowAirDash;

    public Material idleMaterial, jumpMaterial, moveMaterial,
    sprintMaterial, crouchMaterial, dashMaterial;

    [HideInInspector]
    public bool toggleCrouch;

    [HideInInspector]
    public bool toggleSprint;

    [HideInInspector]
    public CharacterController characterController;

    [SerializeField, Range(0f, 10f)]
    private float _gravityMultiplier = 2f;

    [SerializeField]
    private float _slopeForce, _slopeForceRayLength;

    private int _jumpsLeft;

    private bool _falling = false;

    private PlayerStats playerStats;
    private PlayerStamina playerStamina;
    private PlayerHealth playerHealth;
    private PlayerEnergy playerEnergy;

    private StateMachine _bodyStateMachine;
    private StateMachine _headStateMachine;

    public float GravityMultiplier
    {
        get { return _gravityMultiplier; }
    }

    public int JumpsLeft
    {
        get { return _jumpsLeft; }
        set { _jumpsLeft = value;  }
    }

    public StateMachine BodyStateMachine
    {
        get { return _bodyStateMachine; }
    }

    public StateMachine HeadStateMachine
    {
        get { return _headStateMachine; }
    }

    public PlayerStats PlayerStats
    {
        get { return playerStats; }
    }

    public PlayerStamina PlayerStamina
    {
        get { return playerStamina; }
    }

    public void ChangeState(IState newState)
    {
        _bodyStateMachine.ChangeState(newState);
    }

    public void SetMaterial(Material newMaterial)
    {
        Renderer playerMat = GetComponent<MeshRenderer>();
        playerMat.material = newMaterial;
    }

    public bool OnSlope()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, characterController.height / 2 * _slopeForceRayLength))
            if (hit.normal != Vector3.up)
                return true;
        return false;

    }

    private void Awake()
    {
        _bodyStateMachine = new StateMachine();
        _bodyStateMachine.Configure(new PlayerIdleState(this));
        characterController = GetComponent<CharacterController>();
        playerStats = GetComponent<PlayerStats>();
        playerStamina = GetComponent<PlayerStamina>();
        playerHealth = GetComponent<PlayerHealth>();
        playerEnergy = GetComponent<PlayerEnergy>();

        toggleSprint = false;
        _jumpsLeft = PlayerStats.maxJumps.BaseValue;
    }

    private void OnEnable()
    {
        InputHandler.OnEnable();
    }

    private void OnDisable()
    {
        InputHandler.OnDisable();
    }

    private void FixedUpdate()
    {
        // Calls the state machine update method, which calls the current state's execute method
        _bodyStateMachine.Update();
    }

    private void Update()
    {
        JumpReset();
    }

    private void JumpReset()
    {
        // If grounded, reset jumps and set falling to false
        if (characterController.isGrounded)
        {
            JumpsLeft = playerStats.maxJumps.BaseValue;
            _falling = false;
        }

        // If not grounded, you're falling
        if (!characterController.isGrounded)
        {
            // Reduce jumps left by one, so you lose a jump when you fall
            if (!_falling)
            {
                JumpsLeft = playerStats.maxJumps.BaseValue;
            }
            // Set falling to true so you don't keep losing jumps
            _falling = true;
        }
    }
}
