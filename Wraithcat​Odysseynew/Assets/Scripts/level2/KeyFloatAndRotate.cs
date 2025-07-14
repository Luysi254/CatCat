using UnityEngine;

public class KeyFloatAndRotate : MonoBehaviour
{
    // Ư������
    public float floatHeight = 0.5f;    // ���¸����߶�
    public float floatSpeed = 1f;       // �����ٶ�
    private Vector3 startPos;          // ��ʼλ��

    public GameObject keyGetMask; // Կ��������ʾ
    public float maskDelay = 1.0f;    // ������ʾ�ӳ�ʱ�䣨�룩

    // ��ת����
    public float rotateSpeed = 90f;    // ��ת�ٶȣ���/�룩

    void Start()
    {
        startPos = transform.position; // ��¼��ʼλ��
        if (keyGetMask != null)
        {
            keyGetMask.SetActive(false); // ȷ�����ֳ�ʼ����
        }
    }

    void Update()
    {
        // ����Ư��Ч����ʹ�����Һ�����
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        // ������Y����ת����������������ת��
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }

    // ����Կ��
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // ȷ��СèTag��"Player"
        {
            // ����Կ��
            gameObject.SetActive(false);

            // �ӳ���ʾ������ʾ
            if (keyGetMask != null)
            {
                Invoke("ShowMask", maskDelay);
            }
        }
    }

    void ShowMask()
    {
        keyGetMask.SetActive(true);
    }
}