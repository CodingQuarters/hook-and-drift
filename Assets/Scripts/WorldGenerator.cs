using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public int seed = 12345; // The seed for the random number generator, which will be used to ensure that the same world is generated each time with the same seed
    public List<GameObject> chunkTemplates; // A list of chunk templates that can be used to generate the world, allowing for a variety of different chunks to be included in the world generation process
    public float chunkSize = 20f; // The size of each chunk, which will be used to determine how far apart to place the chunks when generating the world
    public Transform playerTransform; // The current position of the player, which will be used to determine which chunks to spawn around the player
    public Dictionary<Vector2Int, GameObject> spawnedChunks = new Dictionary<Vector2Int, GameObject>(); // A dictionary to keep track of the spawned chunks, using the chunk index as the key and the spawned chunk GameObject as the value, allowing for easy access and management of the spawned chunks in the world
    // Update is called once per frame

    void Update() // a chunk will be 20 x 20 units, which means that from x = 0 to x = 20 will be chunk 0, from x = 20 to x = 40 will be chunk 1, and so on. The same applies for the y-axis. This way, we can easily determine which chunk the player is currently in based on their position, and spawn the appropriate chunks around them.
    {
        if (playerTransform == null) return;
        int currentChunkX = Mathf.FloorToInt(playerTransform.position.x / chunkSize); // Calculate the current chunk index on the x-axis based on the player's position and the chunk size
        int currentChunkY = Mathf.FloorToInt(playerTransform.position.y / chunkSize); // Calculate the current chunk index on the y-axis based on the player's position and the chunk size
        Vector2Int currentChunkKey = new Vector2Int(currentChunkX, currentChunkY); // check our cordinate key
        if (!spawnedChunks.ContainsKey(currentChunkKey)) 
       {
            GameObject newChunk = SpawnChunk(currentChunkX, currentChunkY); // If the chunk has not been spawned, call the SpawnChunk method to generate and spawn the chunk at the current index
            spawnedChunks.Add(currentChunkKey, newChunk);

        }
    }
    GameObject SpawnChunk(int chunkX, int chunkY)
    {
        Vector3 worldPosition = new Vector3(chunkX * chunkSize, chunkY * chunkSize, 0); // we need to calculate the world position for unity to place it, ex: if chunkX = 2 and chunkSize is 10, then worldX is 20
        Debug.Log(worldPosition);
        GameObject randomTemplate = chunkTemplates[0]; // just grabbing the first one for now
        
        GameObject newChunkObject = Instantiate(randomTemplate, worldPosition, Quaternion.identity); // spawn logic
        return newChunkObject; // returns to the main loop
    }
}
