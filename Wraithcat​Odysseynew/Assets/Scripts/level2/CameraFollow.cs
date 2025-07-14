using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("����Ŀ��")]
    public Transform target; // ����Сè��Transform

    [Header("�������")]
    public float distance = 5.0f; // �����Ŀ��ľ���
    public float height = 2.0f;   // ����߶�ƫ��
    public float rotationSpeed = 3.0f; // �����ת�ٶ�

    [Header("�ӽ�����")]
    public float minVerticalAngle = -20f;
    public float maxVerticalAngle = 80f;

    private float currentX = 0f;
    private float currentY = 0f;

    void LateUpdate()
    {
        if (target == null) return;

        // ������������ת
        currentX += Input.GetAxis("Mouse X") * rotationSpeed;
        currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        currentY = Mathf.Clamp(currentY, minVerticalAngle, maxVerticalAngle);

        // �������λ��
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = target.position + rotation * dir + Vector3.up * height;

        // ʼ�տ���Ŀ��
        transform.LookAt(target.position + Vector3.up * height * 0.5f);
    }
}