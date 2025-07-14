using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;       // �ƶ��ٶ�
    public float moveUpSpeed = 3f;     // �����ƶ��ٶ�
    public float floatHeight = 0.5f;   // �����߶ȣ������ܷ�Χ��
    public float floatSpeed = 1f;      // �����ٶȣ�Ƶ�ʣ�

    private float originalY;           // ������׼λ��
    private float floatTimer = 0f;     // ������ʱ��
    private bool isFloating = false;   // �Ƿ����ڸ���

   
    void Start()
    {
        originalY = transform.position.y; // ��ʼ����׼λ��
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D �� ���Ҽ�
        float moveY = Input.GetAxisRaw("Vertical");   // W/S �� ���¼�

        // ��������룬ֹͣ�������ƶ�
        if (moveX != 0f || moveY != 0f)
        {
            isFloating = false;
            floatTimer = 0f; // ���ü�ʱ��
            Vector3 move = new Vector3(moveX * moveSpeed, moveY * moveUpSpeed, 0f);
            transform.position += move * Time.deltaTime;
        }
        else
        {
            // ������ʱ����ʼ����
            if (!isFloating)
            {
                originalY = transform.position.y; // ���»�׼λ��
                isFloating = true;
            }
            FloatEffect();
        }

        // ����Y�᷶Χ��-316��-148��
        float clampedY = Mathf.Clamp(transform.position.y, -316f, -148f);
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);
    }

    void FloatEffect()
    {
        // ʹ�����Ҳ� + �������ø��������
        floatTimer += Time.deltaTime * floatSpeed;
        float newY = originalY + Mathf.Sin(floatTimer) * floatHeight * 0.5f; // �߶ȼ��룬����ͻ��
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}