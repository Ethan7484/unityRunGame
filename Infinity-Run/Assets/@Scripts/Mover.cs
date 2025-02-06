using System;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [Header("Settings")] 
    public float moveSpeed = 1f;

    private void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }
}
