using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCameraController : MonoBehaviour
{
    [Header("������")]
    public Transform cat; // ����Hierarchy�е�è����

    [Header("��������")]
    public float followSpeed = 5f;    // ������Ӧ�ٶ�
    public float heightOffset = 0f;   // ��ֱƫ�ƣ����ֽ�ͼY=0��
    public float distance = 0.66f;   // Z����루ƥ���ͼZ=0.66��

    void LateUpdate()
    {
        if (cat == null) return;

        // ˮƽ���棨��ͬ��X�ᣬ�̶�Y/Z��
        Vector3 targetPos = new Vector3(
            cat.position.x + 1.8f,    // ���ֽ�ͼXƫ��1.8
            heightOffset,            // �̶�Y��
            -distance                // �̶�Z�����
        );

        // ƽ���ƶ�
        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            followSpeed * Time.deltaTime
        );

    }
}
