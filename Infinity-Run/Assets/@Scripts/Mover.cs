using System;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [Header("Settings")] 
    public float moveSpeed = 1f;

    public virtual void Update()
    {
        transform.position += Vector3.left * GameManager.instance.CalculateGameSpeed() * moveSpeed * Time.deltaTime;
    }
}
