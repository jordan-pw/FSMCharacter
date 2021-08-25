using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerDodgeState : MovingState
{

    public PlayerDodgeState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        Debug.Log("Entering Dodge State");

        //Check for input
        CheckInput();
        CheckStateChange();

        // Change material
        owner.SetMaterial(owner.idleMaterial);
        // Set velocity
        velocity = Vector3.zero;
    }

    public override void Execute()
    {
        Debug.Log("Executing Dodge State");

        // Even in idle, gravity must apply.
        // In the future, this may be replaced by a falling state
        owner.characterController.Move(new Vector3(0, gravity, 0) * Time.deltaTime);
    }

    public override void Exit()
    {
        Debug.Log("Exiting Dodge State");
    }

    public override void CheckStateChange()
    {
        throw new System.NotImplementedException();
    }

}