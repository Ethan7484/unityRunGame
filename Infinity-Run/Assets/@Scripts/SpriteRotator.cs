using System;
using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 30f; // 초당 회전 각도
    [SerializeField] private bool clockwise = true; // 시계방향 회전 여부

    [Header("Advanced Settings")]
    [SerializeField] private RotationMode rotationMode = RotationMode.Continuous;
    [SerializeField] private float oscillationAngle = 45f; // 진동 각도 (Oscillate 모드)
    [SerializeField] private float oscillationSpeed = 2f; // 진동 속도
    [SerializeField] private AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private float currentAngle = 0f;
    private float oscillationTime = 0f;

    public enum RotationMode
    {
        Continuous, // 연속 회전
        Oscillate,  // 진동 회전
        Random,     // 불규칙 회전
        Interactive // 입력 기반 회전
    }

    private void Update()
    {
        switch (rotationMode)
        {
            case RotationMode.Continuous:
                RotateContinuously();
                break;

            case RotationMode.Oscillate:
                OscillateRotation();
                break;

            case RotationMode.Random:
                RandomRotation();
                break;

            case RotationMode.Interactive:
                InteractiveRotation();
                break;
        }
    }

    // 연속 회전 (기본)
    private void RotateContinuously()
    {
        float direction = clockwise ? -1f : 1f;
        float rotationAmount = rotationSpeed * direction * Time.deltaTime;

        transform.Rotate(0, 0, rotationAmount);

        // 또는 다음과 같이 직접 각도 계산
        // currentAngle += rotationAmount;
        // transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }

    // 진동 회전
    private void OscillateRotation()
    {
        oscillationTime += Time.deltaTime * oscillationSpeed;
        float curveValue = rotationCurve.Evaluate(Mathf.PingPong(oscillationTime, 1f));

        float targetAngle = Mathf.Lerp(-oscillationAngle, oscillationAngle, curveValue);
        transform.rotation = Quaternion.Euler(0, 0, targetAngle);
    }

    // 불규칙 회전
    private void RandomRotation()
    {
        float noise = Mathf.PerlinNoise(Time.time * 0.5f, 0) * 2f - 1f;
        float rotationAmount = noise * rotationSpeed * Time.deltaTime;

        transform.Rotate(0, 0, rotationAmount);
    }

    // 입력 기반 회전
    private void InteractiveRotation()
    {
        float horizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            float rotationAmount = horizontal * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, 0, rotationAmount);
        }
    }

    // 회전 방향 전환
    public void ToggleDirection()
    {
        clockwise = !clockwise;
    }

    // 회전 속도 변경
    public void SetRotationSpeed(float newSpeed)
    {
        rotationSpeed = newSpeed;
    }

    // 회전 모드 변경
    public void SetRotationMode(RotationMode newMode)
    {
        rotationMode = newMode;
    }
}
