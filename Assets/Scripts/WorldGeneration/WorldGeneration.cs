using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    // Gameplay
    private float chunkSpawnZ;

    private Queue<Chunk> activeChunks = new Queue<Chunk>();
    private List<Chunk> chunkPool = new List<Chunk>();

    // Configurable fields
    [SerializeField] private int firstChunkSpawnPosition = 5;
    [SerializeField] private int chunksOnScreen = 5;
    [SerializeField] private float despawnDistance = 5.0f;
    [SerializeField] private List<GameObject> chunkPrefab;
    [SerializeField] private Transform cameraTransform;

    #region TEMPORARY
    private void Awake()
    {
        ResetWorld();
    }
    #endregion
    private void Start()
    {
        if (chunkPrefab.Count == 0)
        {
            Debug.LogError("No chunk prefabs. Please assign.");
            return;
        }

        if (!cameraTransform)
        {
            cameraTransform = Camera.main.transform;
            Debug.Log("We assigned the default camera.");
        }
    }
    private void Update()
    {
        ScanPosition();
    }
    private void ScanPosition()
    {
        float cameraZ = cameraTransform.position.z;
        Chunk lastChunk = activeChunks.Peek();

        if (cameraZ >= lastChunk.transform.position.z + lastChunk.chunkLength + despawnDistance) // If we are far enough
        {
            SpawnNewChunk();
            DeleteLastChunk();
        }
    }
    private void SpawnNewChunk()
    {
        // Get a random index for which prefab to spawn
        int randomIndex = Random.Range(0, chunkPrefab.Count);

        // Does it already exists within our pool
        Chunk chunk = chunkPool.Find(x => !x.gameObject.activeSelf && x.name == (chunkPrefab[randomIndex].name + "(Clone)"));

        // Create a chunk, if we're not able to find one to reuse
        if (!chunk)
        {
            GameObject go = Instantiate(chunkPrefab[randomIndex], transform);
            chunk = go.GetComponent<Chunk>();
        }

        // Place the object and show it
        chunk.transform.position = new Vector3(0, 0, chunkSpawnZ);
        chunkSpawnZ += chunk.chunkLength;

        // Store the value to reuse in our pool
        activeChunks.Enqueue(chunk);
        chunk.ShowChunk();
    }
    private void DeleteLastChunk()
    {
        Chunk chunk = activeChunks.Dequeue();
        chunk.HideChunk();
        chunkPool.Add(chunk);
    }
    public void ResetWorld()
    {
        // Reset the ChunkSpawnZ
        chunkSpawnZ = firstChunkSpawnPosition;
        for (int i = activeChunks.Count; i != 0; i--)
        {
            DeleteLastChunk();
        }
        for (int i = 0; i < chunksOnScreen; i++)
        {
            SpawnNewChunk();
        }
    }
}
