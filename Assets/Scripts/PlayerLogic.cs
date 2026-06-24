using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLogic : MonoBehaviour
{
    public GameManager gameManager; 
    public GameObject portal;
    private Rigidbody2D rb;
    public float forceAmount = 10f;
    public Vector2 newSpawn;
    private Animator myAnimator;
    public bool stayLocked;
    public Vector2 direction;
    public Vector3 mousePos;
    public GrappleTetherController grappleTetherController;
    public Rigidbody2D rigidbody2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2D.GetComponent<Rigidbody2D>();
        stayLocked = false;
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myAnimator.SetBool("StopAnim", true);
    }
 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Rocks"))
        {   
            gameManager.PlayerDies();
        }
        if (collision.gameObject.CompareTag("Portal"))
        {
            portal = collision.gameObject;
            newSpawn = new Vector2(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y);
            StartCoroutine(TriggerMyAnimation());
        }

    }
    // Update is called once per frame

    void FixedUpdate()
    {
        if (stayLocked)
        {
            transform.position = newSpawn;
        }
    }
    IEnumerator TriggerMyAnimation()
    {
        gameObject.layer = 7;
        stayLocked = true;
        portal.SetActive(false);
        myAnimator.SetTrigger("PlayAnim");
        myAnimator.SetBool("StopAnim", false);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        myAnimator.SetBool("StopAnim", true);
        yield return new WaitForEndOfFrame();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        direction = (mousePos-transform.position).normalized;
        stayLocked = false;
        rb.AddForce(direction*10, ForceMode2D.Impulse);
        Debug.Log(direction*10);
        gameObject.layer = 0;        
        yield return new WaitForSeconds(1);
        portal.SetActive(true);
    }
}
