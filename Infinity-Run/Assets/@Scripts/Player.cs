using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Settings")]
    public float jumpForce;
    
    [Header("References")]
    public Rigidbody2D playerRb;
    public Animator playerAnimator;

    private bool isGrounded = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerRb.AddForceY(jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            playerAnimator.SetInteger("state", 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Platform")
        {
            if (!isGrounded)
            {
                // 점프 중에만 착지 모션이 출력되게 작업
                playerAnimator.SetInteger("state", 2);
            }   

            isGrounded = true;
        }

        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player Hit");
            
        }
        else if (other.gameObject.CompareTag("Food"))
        {
            Debug.Log("Food!!!!!");
            
            Destroy(other.gameObject);
            
        }
        else if (other.gameObject.CompareTag("Golden"))
        {
            Debug.Log("Golden!!!!!");
            
            Destroy(other.gameObject);
            
        }
    }
}
