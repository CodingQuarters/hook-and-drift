using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class CurrentSystem : MonoBehaviour
{
    [Header("References")]
    public Material targetMaterial; // Reference to the material that will be affected by the current system
    public Vector2 targetPosition;// the targeted position
    public Vector2 currentValue; // what the shader is currently displaying 
    public GrappleTetherController playerScript; // Reference to the player script, which will be used to influence the player's position 
    [Header("Timing")]
    public float minWaitTime = 5f;
    public float maxWaitTime = 15f;
    public float moveSpeed = 3f;
    public float steps = 5f; // the step size is always the same, making the transition speed consistent regardless of the distance to the target position
    public Vector2 difference; // the distance between the current value and the target position, used to calculate the direction of movement
    public Vector2 normalDirection; // the difference normalized, which means it has a length of 1 and only represents the direction from the current value to the target position, used to move the current value towards the target position at a consistent speed
    public Vector2 previousValue; // the previous value of the current, used to calculate the difference and normal direction for the next update
    public Vector2 shift;
    public Vector2 velocity;
    void Start()
    {
        StartCoroutine(WaitAndChangeCurrent()); // Call the Current method to initialize the current system
    }
    private IEnumerator WaitAndChangeCurrent() //Coroutine to wait for a random time between minWaitTime and maxWaitTime, then call the Current method to change the current system. This will run indefinitely, changing the current at random intervals.
    {
        while (true)
        {
            targetPosition = Random.insideUnitCircle; // Generate a random value within a unit circle
            float waitTime = Random.Range(minWaitTime, maxWaitTime); // Randomize the wait time between changes
            yield return new WaitForSeconds(waitTime); // Wait for the randomized time
        }

    }
    void Update()
    {
        currentValue = targetMaterial.GetVector("_CurrentDirection"); // Get the current value from the material
        if (Vector2.Distance(currentValue, targetPosition) > 0.01f) // Check if the current value is not close enough to the target position
        {
            difference = (targetPosition - currentValue).normalized; // Calculate the distance between the current value and the target position
            velocity = difference / moveSpeed; // Calculate the velocity to move towards the target position based on the difference and move speed
            currentValue += velocity * Time.deltaTime; // Scale the normalized direction by the move speed to control how fast the current value moves towards the target position
            targetMaterial.SetVector("_CurrentDirection", currentValue); // Update the material with the new current value
            playerScript.pushVelocity = velocity; // Update the player script with the new shift value, allowing the player's position to be influenced by the current system

        }
        
    }

}