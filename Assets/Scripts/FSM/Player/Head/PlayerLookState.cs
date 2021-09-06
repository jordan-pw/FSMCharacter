using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerLookState : HeadState
{
    public PlayerLookState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        Debug.Log("Entering Look State");
    }

    public override void Execute()
    {
        Debug.Log("Executing Look State");

        //Check for input
        CheckInput();
        CheckStateChange();

        Rotate();
    }

    public override void Exit()
    {
        Debug.Log("Exiting Look State");
    }

    public override void CheckStateChange()
    {

    }

    private void Rotate()
    {
        Vector3 rotationTarget = new Vector3(ConvertMouse().x, 0f, ConvertMouse().y);
        float rotateSpeed = 100f * Time.deltaTime;

        Quaternion rotationAngle = Quaternion.LookRotation(rotationTarget);
        rotationAngle *= Quaternion.Euler(0, inputSpace.transform.rotation.eulerAngles.y, 0);
        owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, rotationAngle, rotateSpeed);

    }
}