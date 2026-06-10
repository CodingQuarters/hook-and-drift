using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    public GameManager gameManager; 
    public Transform portal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("What's up??");
        gameManager.PlayerDies();

    }
    
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, portal.position) < 5f)
        {
            Debug.Log("You WIN YOU WIN YOU WIN");
            gameManager.YouWon();
        }
    }
}
