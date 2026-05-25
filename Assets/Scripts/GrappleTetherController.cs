using Unity.VisualScripting;
using UnityEngine;

public class GrappleTetherController : MonoBehaviour
{
    [Header("Movement & Current")]
    public Vector2 currentDirection = Vector2.down;
    public float currentStrength = 5f;
    public float playerMoveSpeed = 8f;

    private Rigidbody2D rb ; 
    private DistanceJoint2D distanceJoint ; 
    private Vector2 movementInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        distanceJoint = GetComponent<DistanceJoint2D>();

        distanceJoint.enabled = false; // disable the distanceJOint so that the player can move freely
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
