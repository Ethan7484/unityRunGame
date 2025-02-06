using System;
using Unity.VisualScripting;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    
    void Update()
    {
        if (transform.position.x < -15)
        {
            Destroy(gameObject);
        }
        
    }


}
