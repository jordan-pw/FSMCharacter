using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    const float Gravity = -9.81f;

    [HideInInspector]
    public bool toggleSprint;

    [HideInInspector]
    public CharacterController characterController;

    [SerializeField]
    public Material idleMaterial, jumpMaterial, moveMaterial, sprintMaterial;

    [SerializeField, Range(0f, 100f)]
    private float maxSpeed = 10f;

    [SerializeField, Range(0f, 100f)]
    private float sprintSpeed = 10f;

    [SerializeField, Range(0f, 100f)]
    private float maxAcceleration = 10f;

    [SerializeField, Range(0f, 100f)]
    private float maxAirAcceleration = 10f;

    [SerializeField, Range(0f, 10f)]
    private float gravityMultiplier = 2f;

    [SerializeField, Range(0, 5)]
    int maxAirJumps = 0;


    private StateMachine bodyStateMachine;
    private StateMachine headStateMachine;

    public void ChangeState(IState newState)
    {
        bodyStateMachine.ChangeState(newState);
    }

    public void SetMaterial(Material newMaterial)
    {
        Renderer playerMat = GetComponent<MeshRenderer>();
        playerMat.material = newMaterial;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public float GetSprintSpeed()
    {
        return sprintSpeed;
    }

    public float GetMaxAcceleration()
    {
        return maxAcceleration ;
    }

    public float GetMaxAirAcceleration()
    {
        return maxAirAcceleration;
    }

    public float GetGravity()
    {
        return Gravity;
    }

    public float GetGravityMultiplier()
    {
        return gravityMultiplier;
    }

    public StateMachine GetBodyStateMachine()
    {
        return bodyStateMachine;
    }

    private void Awake()
    {
        bodyStateMachine = new StateMachine();
        bodyStateMachine.Configure(new PlayerIdleState(this));
        characterController = GetComponent<CharacterController>();

        toggleSprint = false;
    }

    private void FixedUpdate()
    {
        // Calls the state machine update method, which calls the current state's execute method
        bodyStateMachine.Update();
    }

    private void Update()
    {

    }


}
