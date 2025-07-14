// NPCFloating.cs
using UnityEngine;

public class NPCFloating : MonoBehaviour
{
    public float floatHeight = 0.5f;  // �����߶�
    public float floatSpeed = 1f;     // �����ٶ�

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // ʹ�����Һ����������¸���Ч��
        // Using sine function to create floating effect
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}