using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [Header("Settings")] public float jumpForce;

    [Header("References")] public Rigidbody2D playerRb;
    public Animator playerAnimator;
    public BoxCollider2D playerCollider;
    
    private bool _isGrounded = true;
    private bool _isDoubleJump = false;

    [SerializeField] private bool isInvincible;
    
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        //{
        //    Jump();
        //}
        //else if (Input.GetKeyDown(KeyCode.Space) && _isDoubleJump)
        //{
        //    DoubleJump();
        //}

    }

    public void Jump()
    {
        if (_isGrounded)
        {
            playerRb.AddForceY(jumpForce, ForceMode2D.Impulse);
            _isDoubleJump = true;
            _isGrounded = false;
            playerAnimator.SetInteger("state", 1);
        }
        else if(_isDoubleJump)
        {
            DoubleJump();
        }
    }

    public void DoubleJump()
    {
        playerRb.AddForceY(jumpForce * .8f, ForceMode2D.Impulse);
        _isDoubleJump = false;
        playerAnimator.SetInteger("state", 1);        
    }

    public void KillPlayer()
    {
        // 콜라이더, 애니메이터 체크 해제
        playerCollider.enabled = false;
        playerAnimator.enabled = false;
        
        // 캐릭터 공중으로 점프시킴
        playerRb.AddForceY(jumpForce * 1.5f, ForceMode2D.Impulse);
    }
    
    private void Hit()
    {
        GameManager.instance.hp -= 1;
    }

    private void Heal()
    {
        GameManager.instance.hp = Mathf.Min(3, GameManager.instance.hp + 1);
    }

    private void StartInvincible()
    {
        isInvincible = true;
        Invoke("StopInvincible", 5f);
    }

    private void StopInvincible()
    {
        isInvincible = false;
    }

private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Platform")
        {
            if (!_isGrounded)
            {
                // 점프 중에만 착지 모션이 출력되게 작업
                playerAnimator.SetInteger("state", 2);
            }   

            _isGrounded = true;
            _isDoubleJump = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (!isInvincible)
            {
                Hit();
                Destroy(other.gameObject);
            }
            
            Debug.Log("Player Hit!!! HP: " + GameManager.instance.hp);
        }
        else if (other.gameObject.CompareTag("Food"))
        {
            Destroy(other.gameObject);
            Heal();
            Debug.Log("Food!!!!!  HP: " + GameManager.instance.hp);
        }
        else if (other.gameObject.CompareTag("Golden"))
        {
            Destroy(other.gameObject);
            StartInvincible();
            Debug.Log("Golden!!!!!");
        }
    }

}
