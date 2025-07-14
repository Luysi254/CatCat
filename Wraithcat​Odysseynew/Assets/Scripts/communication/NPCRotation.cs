// NPCRotation.cs
using UnityEngine;

public class NPCRotation : MonoBehaviour
{
    public Transform player;          // ��ҽ�ɫ
    public float detectionDistance = 3f; // ��֪����
    private bool hasRotated = false;  // �Ƿ�����ת

    void Update()
    {
        if (!hasRotated && Vector3.Distance(transform.position, player.position) < detectionDistance)
        {
            // ��Z����ת-90��
            // Rotate -90 degrees around Z axis
            transform.Rotate(0, 0, -90);
            hasRotated = true;

            // �����Ի�
            // Trigger dialogue
            // DialogueManager.Instance.StartDialogue();
        }
    }
}