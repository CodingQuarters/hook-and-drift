using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    public GameManager gameManager; 
    public Transform portal;
    private Rigidbody2D rb;
    public float forceAmount = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Rocks"))
        {   
            gameManager.PlayerDies();
        }
        else
        {
            Debug.Log(collision.gameObject.tag);
        }
        

    }
    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Touching but not the right current");
        if (collision.gameObject.CompareTag("rightCurrent"))
        {
           Debug.Log("Still touching...");
        }
    }*/
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, portal.position) < 2.5f)
        {
            gameManager.YouWon();
        }
    }
    void FixedUpdate()
    {
        rb.WakeUp(); // forces Unity to calculate the rb every frame

    }
}
