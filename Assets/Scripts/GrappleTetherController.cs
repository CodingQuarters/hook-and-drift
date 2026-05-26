using Unity.VisualScripting;
using UnityEngine;


public class GrappleTetherController : MonoBehaviour
{
    public Vector2 pushVelocity;

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    public void FixedUpdate()
    {
        rb.AddForce(pushVelocity*10, ForceMode2D.Force);

    }

}