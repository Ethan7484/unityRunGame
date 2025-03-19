using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlanetWalker : MonoBehaviour
{
    [Header("Planet Settings")]
    [SerializeField] private Transform planet0; // 첫 번째 행성
    [SerializeField] private Transform planet1; // 두 번째 행성
    [SerializeField] private Transform planet2; // 세 번째 행성
    [SerializeField] private float gravityStrength = 100f;

    [Header("Character Settings")]
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private Transform characterVisual;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 0.3f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckPoint;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Transform currentPlanet;
    [SerializeField] private SpriteRenderer _sr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (groundCheckPoint == null)
        {
            GameObject checkPoint = new GameObject("GroundCheck");
            checkPoint.transform.parent = transform;
            checkPoint.transform.localPosition = new Vector3(0, -0.5f, 0);
            groundCheckPoint = checkPoint.transform;
        }

        // 초기 행성 설정 - 가장 가까운 행성으로 시작
        SetInitialPlanet();
    }

    private void SetInitialPlanet()
    {
        if (planet0 == null || planet1 == null)
        {
            Debug.LogError("Both planets must be assigned!");
            return;
        }

        // 가장 가까운 행성 선택
        float dist0 = Vector2.Distance(transform.position, planet0.position);
        float dist1 = Vector2.Distance(transform.position, planet1.position);

        currentPlanet = (dist0 <= dist1) ? planet0 : planet1;
        Debug.Log("Initial planet: " + currentPlanet.name);
    }

    private void Update()
    {
        transform.RotateAround(
            currentPlanet.position,
            Vector3.forward,
            1 * moveSpeed * Time.deltaTime
        );

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            UpToPlanet();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            DownToPlanet();
        }

    }

    private void DownToPlanet()
    {
        if (currentPlanet == planet2)
        {
            planet2.GetComponent<CircleCollider2D>().enabled = false;
        }
        else if (currentPlanet == planet1)
        {
            planet1.GetComponent<CircleCollider2D>().enabled = false;
        }
        else if (currentPlanet == planet0)
        {
            return;
        }
    }

    private void UpToPlanet()
    {
        if (currentPlanet == planet0)
        {
            planet1.GetComponent<CircleCollider2D>().enabled = true;
        }
        else if (currentPlanet == planet1)
        {
            planet2.GetComponent<CircleCollider2D>().enabled = true;
        }
        else if (currentPlanet == planet2)
        {
            return;
        }
    }

    private void FixedUpdate()
    {
        Debug.Log(currentPlanet.name);

        if (currentPlanet == null)
        {
            SetInitialPlanet();
            if (currentPlanet == null) return;
        }

        // 행성 중심에서 캐릭터 방향으로의 벡터
        Vector2 gravityUp = (transform.position - currentPlanet.position).normalized;

        // 지면 체크
        CheckGround(gravityUp);

        // 캐릭터 회전
        AlignWithPlanet(gravityUp);

        // 중력 적용
        ApplyGravity(gravityUp);

    }

    private void CheckGround(Vector2 gravityUp)
    {
        Vector2 checkDirection = -gravityUp;

        RaycastHit2D hit = Physics2D.Raycast(groundCheckPoint.position, checkDirection, groundCheckDistance, groundLayer);

        isGrounded = hit.collider != null;
        Debug.DrawRay(groundCheckPoint.position, checkDirection * groundCheckDistance, isGrounded ? Color.green : Color.red);
    }

    private void AlignWithPlanet(Vector2 gravityUp)
    {
        float angle = Mathf.Atan2(gravityUp.y, gravityUp.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0, 0, angle);

    }

    private void ApplyGravity(Vector2 gravityUp)
    {
        rb.AddForce(-gravityUp * gravityStrength);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Planet"))
        {
            currentPlanet = planet0;
            moveSpeed = collision.gameObject.GetComponent<PlanetInfo>()._rotationSpeed;
        }
        else if (collision.gameObject.CompareTag("Planet1"))
        {
            currentPlanet = planet1;
            moveSpeed = collision.gameObject.GetComponent<PlanetInfo>()._rotationSpeed;
        }
        else if (collision.gameObject.CompareTag("Planet2"))
        {
            currentPlanet = planet2;
            moveSpeed = collision.gameObject.GetComponent<PlanetInfo>()._rotationSpeed;
        }
    }

    
}
