using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoSafeSpawner : MonoBehaviour
{
    /// <summary>
    /// 스폰되는 Object가 이전껀 안 나오게 셋팅한 스크립트
    /// Claude 이용한거라 당분간은 안 쓸 예정
    /// </summary>
    
    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] gameObjects;

    [SerializeField] private float minSpawnDelay;
    [SerializeField] private float maxSpawnDelay;

    [Space]
    
    [SerializeField] private int previousObjectsToExclude = 3;

    private Queue<int> _recentSpawnedIndices;
    
    private void Start()
    {
        _recentSpawnedIndices = new Queue<int>();
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnObject();
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        }
    }

    private void SpawnObject()
    {
        
        if ((Vector2)transform.position != Vector2.zero)
        {
            int randomIndex = GetRandomObjectIndex();
            Instantiate(gameObjects[randomIndex], transform.position, Quaternion.identity);
            
            _recentSpawnedIndices.Enqueue(randomIndex);
            while (_recentSpawnedIndices.Count > previousObjectsToExclude)
            {
                _recentSpawnedIndices.Dequeue();
            }
        }
    }

    private int GetRandomObjectIndex()
    {
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (!_recentSpawnedIndices.Contains(i))
            {
                availableIndices.Add(i);
            }
        }

        if (availableIndices.Count == 0)
        {
            availableIndices = new List<int>();
            for (int i = 0; i < gameObjects.Length; i++)
            {
                availableIndices.Add(i);
            }
            _recentSpawnedIndices.Clear();
        }

        return availableIndices[Random.Range(0, availableIndices.Count)];
    }

}
