using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float moveForce = 10f; 
    [SerializeField] private float maxSpeed = 5f; 
    [SerializeField] private float linearDrag = 2f; 

    private Rigidbody2D rb; 
    private Vector2 moveInput;
    //Above here is all for the movement/slide effect. The Rigidbody2D is used to apply physics to the player, and the moveInput is used to store the input from the player.
    private Vector2 origin;
    [SerializeField] private float MaxDistance = 5f; // The maximum distance for the raycast to check for gameobject (with colliders).
    private Vector2 direction;
    private bool clickedThisFrame;
    [SerializeField] private GameObject linePrefab; // Prefab for the line renderer
    [SerializeField] private float pullForce = 30f; // Force applied to the player when the raycast hits an object
    [SerializeField] private float clickCooldown = 2f; // Cooldown time in seconds between clicks
    [SerializeField] private float targetLength = 5f; // Target length for the line animation
    private float lastClickTime;
    public Vector2 RandomValue;
    public Vector2 pushVelocity; // This variable will be updated by the current system to influence the player's position based on the current direction and strength.
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = linearDrag;
        lastClickTime = -clickCooldown; // Allow immediate first click
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    void Update() 
    {
        Shader.SetGlobalVector("_PlayerPosition", transform.position); // Update the shader with the player's current position
        Vector3 movementDirection = transform.forward;
        Shader.SetGlobalVector("_MovementDirection", movementDirection); // Update the shader with the player's movement direction
        if (Mouse.current.leftButton.wasReleasedThisFrame && Time.time - lastClickTime >= clickCooldown)
        {
            clickedThisFrame = true;
            lastClickTime = Time.time;
        }
    }

    void FixedUpdate() // FixedUpdate is used for physics updates, which is where we want to handle the raycasting and movement logic.
    {
        //Debug.Log("Current push velocity: " + pushVelocity); // Log the current push velocity for debugging purposes
        rb.AddForce(pushVelocity*10); // Apply the push velocity from the current system to the player's position, allowing the current system to influence the player's movement
        origin = (Vector2)transform.position;
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Camera.main.nearClipPlane));
        direction = ((Vector2)mouseWorldPos - origin).normalized; // Direction towards the mouse
        
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, MaxDistance);
        Debug.DrawRay(origin, direction * MaxDistance, Color.red); // Visualize the raycast in the scene

        if (clickedThisFrame)
        {
            if (linePrefab != null)
            {
                GameObject spawnedLine = Instantiate(linePrefab, origin, Quaternion.identity);
                spawnedLine.transform.up = direction; // Align the prefab's vertical axis with the ray direction
                float lineLength = hit.collider != null ? Vector2.Distance(origin, hit.point) : targetLength; // Set line length to hit point or max distance
                spawnedLine.GetComponent<GrapplerLine>().Animate(lineLength); // Animate the line with the target length
            }

            if (hit.collider != null)
            {
                rb.AddForce(direction * pullForce, ForceMode2D.Impulse);
            }

            clickedThisFrame = false;
        }

        //This is for the movement/slide effect. It applies a force in the direction of the input, and then clamps the speed to maxSpeed. The linear drag will help to slow down the player when they stop giving input, creating a sliding effect.
        Vector2 moveDirection = moveInput;
        // Normalize ONLY if needed (keeps analog input working)
        if (moveDirection.magnitude > 1f)
            moveDirection.Normalize();

        // Apply force
        rb.AddForce(moveDirection * moveForce, ForceMode2D.Force);

        // Clamp total speed only when not pulling (to allow pulling to exceed maxSpeed)
        if (!clickedThisFrame && Time.time - lastClickTime >= clickCooldown)
        {
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);
        }
                // Removed speed clamping to allow pulling to propel the player far
        // Linear drag will still slow down the player over time

    }
}