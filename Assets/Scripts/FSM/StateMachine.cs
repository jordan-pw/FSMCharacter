using UnityEngine;
public class StateMachine
{
    private IState currentState;
    private IState previousState;

    public void Awake()
    {
        currentState = null;
        previousState = null;
    }

    // Sets the initial state
    public void Configure(IState initialState)
    {
        ChangeState(initialState);
    }

    public void Update()
    {
        if (currentState != null) currentState.Execute();
    }

    public void ChangeState(IState newState)
    {
        // Sets the current state as the previous state
        previousState = currentState;

        // Check if the states are different (better to avoid doing this when calling the method)
        if (currentState == newState) return;

        // Trigger the exit method
        if (currentState != null)
            currentState.Exit();

        // Set the new state and begin
        currentState = newState;
        currentState.Enter();
    }

    public void RevertToPreviousState()
    {
        if (previousState != null) ChangeState(previousState);
    }

    public IState GetPreviousState()
    {
        return previousState;
    }
}