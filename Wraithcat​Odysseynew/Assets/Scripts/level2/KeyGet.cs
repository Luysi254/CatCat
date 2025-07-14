using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGet : MonoBehaviour
{
    // Ư������
    public float floatHeight = 0.5f;    // ���¸����߶�
    public float floatSpeed = 1f;       // �����ٶ�
    private Vector3 startPos;          // ��ʼλ��

    // ��ת����
    public float rotateSpeed = 90f;    // ��ת�ٶȣ���/�룩

    void Start()
    {
        startPos = transform.position; // ��¼��ʼλ��

    }

    void Update()
    {
        // ����Ư��Ч����ʹ�����Һ�����
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        // ������Y����ת����������������ת��
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }
}
