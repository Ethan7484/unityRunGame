using System;
using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 30f; // �ʴ� ȸ�� ����
    [SerializeField] private bool clockwise = true; // �ð���� ȸ�� ����

    [Header("Advanced Settings")]
    [SerializeField] private RotationMode rotationMode = RotationMode.Continuous;
    [SerializeField] private float oscillationAngle = 45f; // ���� ���� (Oscillate ���)
    [SerializeField] private float oscillationSpeed = 2f; // ���� �ӵ�
    [SerializeField] private AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private float currentAngle = 0f;
    private float oscillationTime = 0f;

    public enum RotationMode
    {
        Continuous, // ���� ȸ��
        Oscillate,  // ���� ȸ��
        Random,     // �ұ�Ģ ȸ��
        Interactive // �Է� ��� ȸ��
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

    // ���� ȸ�� (�⺻)
    private void RotateContinuously()
    {
        float direction = clockwise ? -1f : 1f;
        float rotationAmount = rotationSpeed * direction * Time.deltaTime;

        transform.Rotate(0, 0, rotationAmount);

        // �Ǵ� ������ ���� ���� ���� ���
        // currentAngle += rotationAmount;
        // transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }

    // ���� ȸ��
    private void OscillateRotation()
    {
        oscillationTime += Time.deltaTime * oscillationSpeed;
        float curveValue = rotationCurve.Evaluate(Mathf.PingPong(oscillationTime, 1f));

        float targetAngle = Mathf.Lerp(-oscillationAngle, oscillationAngle, curveValue);
        transform.rotation = Quaternion.Euler(0, 0, targetAngle);
    }

    // �ұ�Ģ ȸ��
    private void RandomRotation()
    {
        float noise = Mathf.PerlinNoise(Time.time * 0.5f, 0) * 2f - 1f;
        float rotationAmount = noise * rotationSpeed * Time.deltaTime;

        transform.Rotate(0, 0, rotationAmount);
    }

    // �Է� ��� ȸ��
    private void InteractiveRotation()
    {
        float horizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            float rotationAmount = horizontal * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, 0, rotationAmount);
        }
    }

    // ȸ�� ���� ��ȯ
    public void ToggleDirection()
    {
        clockwise = !clockwise;
    }

    // ȸ�� �ӵ� ����
    public void SetRotationSpeed(float newSpeed)
    {
        rotationSpeed = newSpeed;
    }

    // ȸ�� ��� ����
    public void SetRotationMode(RotationMode newMode)
    {
        rotationMode = newMode;
    }
}
