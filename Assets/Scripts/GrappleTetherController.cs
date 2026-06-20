using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class GrappleTetherController : MonoBehaviour
{
    public Vector2 pushVelocity;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float castDistance = 16f; // distance allowed for max
    private Rigidbody2D rb;
    private DistanceJoint2D distanceJoint;
    private LineRenderer lineRenderer;
    void Start()
    {
        distanceJoint = gameObject.GetComponent<DistanceJoint2D>();
        distanceJoint.enableCollision = true;
        rb = GetComponent<Rigidbody2D>();
        distanceJoint.connectedAnchor = new Vector2(5,50);
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }
    public void Update()
    {


        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            
            Camera cam = Camera.main;
            Vector3 worldPoint = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Mathf.Abs(cam.transform.position.z)));
            Ray ray = cam.ScreenPointToRay(mousePos); // this is for the ray 

            RaycastHit2D hit2D = Physics2D.Raycast(worldPoint, Vector2.zero, castDistance, targetLayer); 
            Debug.Log(hit2D.collider);
            lineRenderer.SetPosition(0, worldPoint);
            
            if (hit2D.collider != null )
            {
                if (hit2D.collider.transform.CompareTag("Rocks"))
                {   


                    Debug.Log($"Hit 2d object : {hit2D.collider.name}");
                    distanceJoint.connectedAnchor = hit2D.collider.transform.position;
                    distanceJoint.autoConfigureDistance= true;
                    lineRenderer.enabled = true;
                    distanceJoint.enabled = true;
                    lineRenderer.SetPosition(1,hit2D.point); // if the line hits a collider, end the line at that point
                }
            }
            else
            {
                Vector3 endPoint = worldPoint + (Vector3)(Vector2.zero);
                lineRenderer.SetPosition(1, endPoint);
            }

        }   
        if (distanceJoint.enabled)
        {
            lineRenderer.SetPosition(0, transform.position);
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            distanceJoint.enabled = false;
            lineRenderer.enabled = false;
        }

    }
    public void FixedUpdate()
    {
        rb.AddForce(pushVelocity*10, ForceMode2D.Force);

    }

}