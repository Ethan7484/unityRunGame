using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class Spawner : MonoBehaviour
{
    [Header("Settings")]
    public float firstSpawnTime = 0f;
    public float minSpawnDelay;
    public float maxSpawnDelay;
    
    [Header("References")] 
    public GameObject[] gameObjects;

    private void OnEnable()
    {
        InvokeRepeating("Spawn", firstSpawnTime, Random.Range(minSpawnDelay, maxSpawnDelay));
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Spawn()
    {
        var randomObject = gameObjects[Random.Range(0, gameObjects.Length)];
        
        // Building Spawner 게임 오브젝트 하위에 생성되도록 수정
        Instantiate(randomObject, transform.position, Quaternion.identity, transform);
        
    }

    
}
