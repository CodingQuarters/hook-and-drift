using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class LineDrawer : MonoBehaviour
{
    public GameObject player;
    public Vector2 playerPosition;
    public Vector3 worldMousePos;
    public RaycastHit2D raycastHit2D;
    public Vector2 direction;
    [SerializeField] private LayerMask targetLayer;
    public Vector2 grapplePoint;
    [SerializeField] private DistanceJoint2D distanceJoint2D;
    public float maxDistance = 15f;
    private LineRenderer lineRenderer;
    public bool canGrapple; // this is a bool to check so that it doesn't grapple when you try to launch

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        distanceJoint2D = player.GetComponent<DistanceJoint2D>();
        distanceJoint2D.enabled = false;
        //distanceJoint2D.enableCollision = true;
        canGrapple = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (distanceJoint2D.enabled)
        {
            lineRenderer.SetPosition(0, playerPosition);
        }
        Vector2 screenPos = Mouse.current.position.ReadValue();
        playerPosition = player.transform.position;
        worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane + 1f));
        direction = (new Vector2(worldMousePos.x, worldMousePos.y) - playerPosition).normalized;
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            raycastHit2D = Physics2D.Raycast(playerPosition, direction, maxDistance, targetLayer);
            Debug.DrawRay(playerPosition, direction * 15, Color.red);
            if (raycastHit2D.collider != null)
            {
                if (raycastHit2D.collider.transform.CompareTag("Rocks") && canGrapple)
                {

                    grapplePoint = raycastHit2D.point;
                    distanceJoint2D.connectedAnchor = grapplePoint;
                    distanceJoint2D.autoConfigureDistance = true;
                    distanceJoint2D.enabled = true;
                    Debug.Log(distanceJoint2D);
                    lineRenderer.enabled = true;
                    lineRenderer.SetPosition(0, playerPosition);
                    lineRenderer.SetPosition(1, grapplePoint);
                    
                }
            }
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            distanceJoint2D.enabled = false;
            lineRenderer.enabled = false;
        }
    }
}
