using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class CurrentSystem : MonoBehaviour
{
    [Header("References")]
    public Material targetMaterial; // Reference to the material that will be affected by the current system
    public Vector2 activeDirection; // The current direction of the current system, which will be updated in the shader
    public Vector2 newDirection; // The new direction that the current system will transition to
    [Header("Timing")]
    public float minWaitTime = 5f;
    public float maxWaitTime = 15f;
    public float transitionDuration = 3f; // Duration for the current transition effect
    void Start()
    {
        activeDirection = Random.insideUnitCircle.normalized; // Initialize the current system at the start of the game
        targetMaterial.SetVector("_CurrentDirection", activeDirection); // Set the initial current direction in the shader
        StartCoroutine(WaitAndChangeCurrent()); // Call the Current method to initialize the current system
    }
    private IEnumerator WaitAndChangeCurrent() //Coroutine to wait for a random time between minWaitTime and maxWaitTime, then call the Current method to change the current system. This will run indefinitely, changing the current at random intervals.
    {
        while (true)
        {
            float waitTime = Random.Range(minWaitTime, maxWaitTime); // Randomize the wait time between changes
            yield return new WaitForSeconds(waitTime); // Wait for the randomized time
            Vector2 newDirection = Random.insideUnitCircle.normalized; // Generate a new random value for the current direction
            yield return StartCoroutine(SmoothTransition(activeDirection, newDirection)); // Transition to the new current direction smoothly 
            activeDirection = newDirection; // Update the active direction to the new direction after the transition is complete
        }
    }
    private IEnumerator SmoothTransition(Vector2 newDirection, Vector2 activeDirection) // Coroutine to smoothly transition the current direction in the shader from the old value to the new value over the specified transition duration.
    {
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration; // Calculate the interpolation factor
            Vector2 currentDirection = Vector2.Lerp(activeDirection, newDirection, t); // Interpolate between the old and new directions
            targetMaterial.SetVector("_CurrentDirection", currentDirection); // Update the shader with the current interpolated direction
            elapsedTime += Time.deltaTime; // Increment elapsed time
            yield return null; // Wait for the next frame
        }
        targetMaterial.SetVector("_CurrentDirection", newDirection); // Ensure the final direction is set after the transition
    }
}