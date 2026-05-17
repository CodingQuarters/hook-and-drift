using UnityEngine;

public class RockVariator : MonoBehaviour
{
    public Sprite[] rockSprites; // Array to hold the different rock sprites that can be randomly assigned to the game object 
    
    //called automatically when the chunk is spawned
    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component attached to the game object
        if (rockSprites.Length > 0) // Check if there are any sprites in the array
        {
            int randomIndex = Random.Range(0, rockSprites.Length); // Generate a random index to select a sprite from the array
            spriteRenderer.sprite = rockSprites[randomIndex]; // Assign the randomly selected sprite to the SpriteRenderer component
        }
        transform.localScale *= Random.Range(0.8f, 1.2f); // Randomly scale the rock to add visual variety, making it slightly smaller or larger than its original size
        transform.Rotate(0,0,Random.Range(0f, 360f)); // Randomly rotate the rock to add visual variety, giving it a random orientation 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
