using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class Spawner : MonoBehaviour
{
    [Header("Settings")]
    public float minSpawnDelay;
    public float maxSpawnDelay;
    
    [Header("References")] 
    public GameObject[] gameObjects;

    private void Start()
    {
        InvokeRepeating("Spawn", 0f, Random.Range(minSpawnDelay, maxSpawnDelay));
    }

    private void Spawn()
    {
        var randomObject = gameObjects[Random.Range(0, gameObjects.Length)];
        
        Instantiate(randomObject, transform.position, Quaternion.identity);
        

        
    }

    
}
