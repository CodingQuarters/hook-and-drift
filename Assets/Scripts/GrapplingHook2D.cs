using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public enum grappleState
{
    idle, 
    launching,
    connected,
    retracting
}
public class GrapplingHook2D : MonoBehaviour
{
    public grappleState currentState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case grappleState.idle:
                HandleIdle();
                break;
            case grappleState.launching:
                HandleLaunching();
                break;
            case grappleState.connected:
                HandleConnected();
                break;
            case grappleState.retracting:
                HandleRetracting();
                break;
        }
    }
    public void HandleIdle()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            SetState(grappleState.launching);
        }
    }
    public void HandleLaunching()
    {
        // logic
    }
    public void HandleConnected()
    {
        // logic
    }
    public void HandleRetracting()
    {
        // logic
    }
    public void SetState(grappleState newState)
    {
        currentState = newState;
        Debug.Log("Game state changed to: " + currentState.ToString());

    }
}
