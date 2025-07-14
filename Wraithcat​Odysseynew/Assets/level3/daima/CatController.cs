using UnityEngine;

public class CatController : MonoBehaviour
{
    [Header("�ƶ�����")]
    public float moveSpeed = 5f; // �ƶ��ٶ�
    public float rotationSpeed = 10f; // ��ת�ٶ�

    [Header("������������")]
    public float floatHeight = 0.2f;      // �����߶ȣ��ף�
    public float floatSpeed = 1.5f;       // �����ٶȣ�ÿ����������
    public float floatSmoothness = 5f;    // ����ƽ����
    public bool enableFloating = true;    // �Ƿ����ø�������

    private Rigidbody rb; // �������
    private Quaternion baseRotation; // �洢������תƫ��
    private Vector3 originalPosition;     // ԭʼλ�ã����ڸ������㣩
    private float floatOffset;            // ����ƫ�������������Ҽ��㣩
    private bool isFloating = false;      // ��ǰ�Ƿ����ڸ���

    [Header("�������")]
    public bool isTrapped = false;              // �Ƿ񱻽���
    public float trapDuration = 3f;              // ��������ʱ��
    private float trapTimer;                      // ������ʱ��
    public int escapePresses = 5;                 // ������Ҫ�����Ĵ���
    private int currentPressCount;                // ��ǰ��������

    void Start()
    {
        // ���û�����תƫ�ƣ���X����ת-90�ȣ�
        baseRotation = Quaternion.Euler(-90, 0, 0);

        // Ӧ�ó�ʼ��ת
        transform.rotation = baseRotation;

        // ��ȡ�������
        rb = GetComponent<Rigidbody>();

        // ������ת�Է�ֹ����Ӱ����ת
        rb.freezeRotation = true;

        // ��¼��ʼλ����Ϊ������׼
        originalPosition = transform.position;
    }

    void Update()
    {
        // ��ȡWASD����
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // �����ƶ�����
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical);

        // ����Ƿ�Ӧ�ø�����û���ƶ�������δ��������
        isFloating = enableFloating && moveDirection == Vector3.zero && !isTrapped;

        // �ƶ���ɫ
        if (moveDirection != Vector3.zero)
        {
            // �ƶ�λ��
            Vector3 moveVelocity = moveDirection * moveSpeed;
            rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);

            // ֱ�Ӽ���Ŀ����ת������X��-90�ȣ�������Y�ᣩ
            Quaternion targetRotation = Quaternion.Euler(-90,
                Quaternion.LookRotation(moveDirection).eulerAngles.y,
                0);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
        else
        {
            // û������ʱֹͣ�ƶ�
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }

        // ������������������ѻ���
        if (isTrapped)
        {
            HandleTrap();
            return; // ���������ƶ��߼�
        }

        // Ӧ�ø�������
        if (isFloating)
        {
            ApplyFloatingAnimation();
        }
        else
        {
            // ���ø�����׼λ��
            originalPosition = transform.position;
        }
    }

    // Ӧ�ø�������
    private void ApplyFloatingAnimation()
    {
        // ���¸���ƫ����
        floatOffset += Time.deltaTime * floatSpeed;

        // �������Ҳ�����ֵ
        float verticalOffset = Mathf.Sin(floatOffset) * floatHeight;

        // ����Ŀ��λ��
        Vector3 targetPosition = originalPosition + Vector3.up * verticalOffset;

        // ƽ���ƶ���Ŀ��λ��
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            floatSmoothness * Time.deltaTime
        );
    }

    public void GetTrapped()
    {
        if (isTrapped) return; // �Ѿ����ڽ���״̬���ٴ���

        isTrapped = true;
        trapTimer = trapDuration;
        currentPressCount = 0;

        // ����ֹͣ�ƶ�
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Debug.Log("��ұ�������");
    }

    private void HandleTrap()
    {
        // �ݼ���ʱ��
        trapTimer -= Time.deltaTime;

        // ������Ѱ������ո����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentPressCount++;

            // ��������㹻����������
            if (currentPressCount >= escapePresses)
            {
                EscapeTrap();
            }
        }

        // ��ʱ���������Զ�����
        if (trapTimer <= 0)
        {
            EscapeTrap();
        }
    }

    private void EscapeTrap()
    {
        isTrapped = false;
        Debug.Log("��������˽�����");
    }

    // ���Կ��ӻ�
    void OnDrawGizmosSelected()
    {
        if (isFloating)
        {
            // ���Ƹ����켣
            Gizmos.color = Color.green;
            Vector3 startPos = originalPosition - Vector3.up * floatHeight;
            Vector3 endPos = originalPosition + Vector3.up * floatHeight;
            Gizmos.DrawLine(startPos, endPos);

            // ���Ƶ�ǰ����λ��
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.1f);
        }
    }
}