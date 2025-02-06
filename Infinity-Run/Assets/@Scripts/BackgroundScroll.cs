using System;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{

    [Header("Settings")] 
    [Tooltip("How fast should the texture scroll?")]
    public float scrollSpeed;
    
    [Header("References")]
    public MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        meshRenderer.material.mainTextureOffset += Vector2.right * scrollSpeed * Time.deltaTime;
    }
}
