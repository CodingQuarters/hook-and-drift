using UnityEngine;

public class CurrentSystem : MonoBehaviour
{
    public Material targetMaterial; // Reference to the material that will be affected by the current system
    public Vector2 randomValue; 
    void Start()
    {
        Debug.Log("CurrentSystem initialized");   
    }
    void Current()
    {
        randomValue = Random.insideUnitCircle; // Generate a random value within a unit circle
        targetMaterial.SetVector("_CurrentDirection", randomValue);
    }

}