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
    [SerializeField] private Transform planet2; // 세 번째 행성y
    [SerializeField] private float gravityStrength = 50f;
    [SerializeField] private LayerMask planetLayer;

    [Header("Character Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float normalJumpForce = 7f;
    [SerializeField] private Transform characterVisual;

    [Header("Planet Jump Settings")]
    [SerializeField] private float planetJumpDuration = 1f;
    [SerializeField] private AnimationCurve jumpCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private GameObject jumpEffectPrefab;
    [SerializeField] private AudioClip jumpSound;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 0.3f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckPoint;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    private Transform currentPlanet;
    private bool isJumpingBetweenPlanets = false;
    private AudioSource audioSource;
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

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && jumpSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
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

        // 행성 간 점프 중에는 입력 무시
        if (isJumpingBetweenPlanets) return;

        // 방향키 입력
        // moveInput = Input.GetAxisRaw("Horizontal");
        transform.RotateAround(
            currentPlanet.position,
            Vector3.forward,
            1 * moveSpeed * Time.deltaTime
        );

        // 점프 입력 처리
        //if (Input.GetButtonDown("Jump") && isGrounded)
        //{
        //    JumpToOtherPlanet();
        //}

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("Up Arrow key was pressed.");
            UpToPlanet();

        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("Down Arrow key was pressed.");
            DownToPlanet();
        }

        // 캐릭터 시각적 요소 좌우 반전
        if (moveInput != 0 && characterVisual != null)
        {
            Vector3 newScale = characterVisual.localScale;
            newScale.x = moveInput > 0 ? Mathf.Abs(newScale.x) : -Mathf.Abs(newScale.x);
            characterVisual.localScale = newScale;
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
        // 행성 간 점프 중에는 물리 처리 무시
        if (isJumpingBetweenPlanets) return;

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

        // 이동 처리
        MoveOnPlanet();
    }

    private void CheckGround(Vector2 gravityUp)
    {
        Vector2 checkDirection = -gravityUp;

        RaycastHit2D hit = Physics2D.Raycast(
            groundCheckPoint.position,
            checkDirection,
            groundCheckDistance,
            groundLayer
        );

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

    private void MoveOnPlanet()
    {
        Vector2 moveDirection = transform.right;
        Vector2 verticalVelocity = Vector2.Dot(rb.linearVelocity, transform.up) * (Vector2)transform.up;
        rb.linearVelocity = moveDirection * moveInput * moveSpeed + verticalVelocity;
    }

    private void JumpToOtherPlanet()
    {
        // 다른 행성으로 이동
        
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
            planet1.GetComponent<CircleCollider2D>().enabled = false;
            planet2.GetComponent<CircleCollider2D>().enabled = false;
        }


        // 코루틴으로 행성 간 이동 시작
        // StartCoroutine(SmoothPlanetJump(targetPlanet));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Planet"))
        {
            // 행성에 닿으면 현재 행성으로 설정
            currentPlanet = planet0;
            Debug.Log(currentPlanet.name);
            moveSpeed = collision.gameObject.GetComponent<PlanetInfo>()._rotationSpeed;
        }
        else if (collision.gameObject.CompareTag("Planet1"))
        {
            // 행성에 닿으면 현재 행성으로 설정
            currentPlanet = planet1;
            Debug.Log(currentPlanet.name);
            moveSpeed = collision.gameObject.GetComponent<PlanetInfo>()._rotationSpeed;
        }
        else if (collision.gameObject.CompareTag("Planet2"))
        {
            // 점프 중 땅에 닿으면 점프 종료
            currentPlanet = planet2;
            Debug.Log(currentPlanet.name);
            moveSpeed = collision.gameObject.GetComponent<PlanetInfo>()._rotationSpeed;
        }
    }

   

    //private IEnumerator SmoothPlanetJump(Transform targetPlanet)
    //{
    //    isJumpingBetweenPlanets = true;

    //    // 점프 사운드 재생
    //    if (audioSource != null && jumpSound != null)
    //    {
    //        audioSource.PlayOneShot(jumpSound);
    //    }

    //    // 이동 시작 시 물리 비활성화
    //    rb.simulated = false;

    //    // 시작 위치와 회전
    //    Vector3 startPosition = transform.position;
    //    Quaternion startRotation = transform.rotation;

    //    // 목표 위치 계산 (목표 행성 표면 약간 위)
    //    Vector3 planetDirection = (startPosition - targetPlanet.position).normalized;
    //    float planetRadius = GetPlanetRadius(targetPlanet);
    //    Vector3 targetPosition = targetPlanet.position + planetDirection * (planetRadius + 0.5f);

    //    // 목표 회전 계산
    //    float targetAngle = Mathf.Atan2(planetDirection.y, planetDirection.x) * Mathf.Rad2Deg - 90;
    //    Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);

    //    // 중간 점 계산 (두 행성 사이 적절한 위치)
    //    Vector3 midPoint = (startPosition + targetPosition) / 2f;

    //    // 두 행성 사이의 거리에 비례한 점프 높이
    //    float distance = Vector3.Distance(currentPlanet.position, targetPlanet.position);
    //    float jumpHeight = Mathf.Max(1f, distance * 0.2f);

    //    // 중간 점을 두 행성을 잇는 선에서 수직으로 이동
    //    Vector3 perpendicular = Vector3.Cross(
    //        (targetPlanet.position - currentPlanet.position).normalized,
    //        Vector3.forward
    //    );
    //    midPoint += perpendicular * jumpHeight;

    //    // 점프 시작 효과
    //    CreateJumpEffect(startPosition);

    //    float timeElapsed = 0;
    //    while (timeElapsed < planetJumpDuration)
    //    {
    //        float normalizedTime = timeElapsed / planetJumpDuration;
    //        float curveValue = jumpCurve.Evaluate(normalizedTime);

    //        // 베지어 곡선을 이용한 포물선 이동
    //        Vector3 m1 = Vector3.Lerp(startPosition, midPoint, curveValue);
    //        Vector3 m2 = Vector3.Lerp(midPoint, targetPosition, curveValue);
    //        transform.position = Vector3.Lerp(m1, m2, curveValue);

    //        // 회전 보간
    //        transform.rotation = Quaternion.Slerp(startRotation, targetRotation, curveValue);

    //        timeElapsed += Time.deltaTime;
    //        yield return null;
    //    }

    //    // 최종 위치와 회전 설정
    //    transform.position = targetPosition;
    //    transform.rotation = targetRotation;

    //    // 도착 효과
    //    CreateJumpEffect(targetPosition);

    //    // 물리 다시 활성화 및 현재 행성 업데이트
    //    rb.simulated = true;
    //    rb.linearVelocity = Vector2.zero;
    //    currentPlanet = targetPlanet;

    //    // 점프 종료
    //    isJumpingBetweenPlanets = false;

    //    Debug.Log("Jumped to: " + currentPlanet.name);
    //}

    private float GetPlanetRadius(Transform planet)
    {
        // 행성의 반지름 계산 (콜라이더 기반)
        CircleCollider2D collider = planet.GetComponent<CircleCollider2D>();
        if (collider != null)
        {
            return collider.radius * Mathf.Max(planet.lossyScale.x, planet.lossyScale.y);
        }

        // 콜라이더가 없으면 스프라이트 기반
        SpriteRenderer renderer = planet.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            return Mathf.Max(renderer.bounds.extents.x, renderer.bounds.extents.y);
        }

        // 기본값
        return 1f;
    }

    private void CreateJumpEffect(Vector3 position)
    {
        if (jumpEffectPrefab != null)
        {
            Instantiate(jumpEffectPrefab, position, Quaternion.identity);
        }
    }

    // 에디터에서 시각화
    private void OnDrawGizmos()
    {
        if (planet0 != null && planet1 != null)
        {
            // 두 행성 간의 연결선
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(planet0.position, planet1.position);
        }

        if (currentPlanet != null)
        {
            // 현재 행성과 캐릭터 사이의 연결선
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, currentPlanet.position);
        }
    }

}
