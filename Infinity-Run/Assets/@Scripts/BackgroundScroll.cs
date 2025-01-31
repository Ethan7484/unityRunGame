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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        meshRenderer.material.mainTextureOffset += new Vector2(scrollSpeed * Time.deltaTime, 0);
        
    }
}
