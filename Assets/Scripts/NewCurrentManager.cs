using UnityEngine;
using System.Collections.Generic;
public class NewCurrentManager : MonoBehaviour
{
    public List<GameObject> Currents = new List<GameObject>();
    public Transform Player;
    private Rigidbody2D playerRb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = Player.GetComponent<Rigidbody2D>();

        ProcessObjectsByName();
    }

    // Update is called once per frame
    private void OnTriggerStay2D(Collider2D other)
    {
        // Check if the object entering the trigger is the player
        if (other.transform == Player)
        {
            // gameObject refers to the current this script is attached to
            if (gameObject.CompareTag("rightCurrent"))
            {
                MovePlayerRight();
            }
        }
    }
    void MovePlayerRight()
    {
        if (playerRb != null)
        {
            // Physics-based movement (Best practice)
            playerRb.linearVelocity = new Vector2(5f, playerRb.linearVelocity.y);
        }

    }
    void ProcessObjectsByName() 
    {
        foreach (GameObject obj in Currents) 
        {
            if (obj == null) continue;

            // CompareTag is highly optimized in Unity
            if (obj.CompareTag("rightCurrent")) 
            {
                Debug.Log("Found the right current! Restoring health...");
            }
            else if (obj.CompareTag("upCurrent")) 
            {
                Debug.Log("Found the upCurrent!");
            }
            else if (obj.CompareTag("downCurrent")) 
            {
                Debug.Log($"Collecting item: {obj.name}");
            }
        }
    }

}
