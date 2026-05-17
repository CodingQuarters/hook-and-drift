using UnityEngine;
[System.Serializable]
public enum ChunkDifficulty
{
    Clear, 
    Light, 
    Dense, 
    Hazardous
}

[CreateAssetMenu(fileName = "Chunk", menuName = "Scriptable Objects/Chunk")]
public class Chunk : ScriptableObject
{
    public GameObject chunkPrefab; // The prefab for the chunk, which will be instantiated in the scene
    public ChunkDifficulty difficulty; // The difficulty level of the chunk, which can be used to
}
