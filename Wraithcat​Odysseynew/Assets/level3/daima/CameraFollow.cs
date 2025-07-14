using UnityEngine;

[RequireComponent(typeof(Camera))]
public class camerafollow : MonoBehaviour
{
    [Header("������������")]
    [Tooltip("Ҫ�����Ŀ�����ͨ������ҽ�ɫ��")]
    public Transform target;

    [Tooltip("����ƽ���ȣ�ֵԽ�����Խ����")]
    [Range(1f, 20f)] public float smoothSpeed = 5f;

    [Tooltip("����������Ŀ���ƫ����")]
    public Vector3 offset = new Vector3(0f, 3f, -5f);

    [Header("��������")]
    [Tooltip("�Ƿ����X���ƶ�")] public bool followX = true;
    [Tooltip("�Ƿ����Y���ƶ�")] public bool followY = true;
    [Tooltip("�Ƿ����Z���ƶ�")] public bool followZ = true;

    [Header("��ת����")]
    [Tooltip("�Ƿ��������ת")] public bool rotateWithTarget = true;

    [Tooltip("��תƽ����")]
    [Range(1f, 20f)] public float rotationSmoothness = 10f;

    [Header("��ײ����")]
    [Tooltip("�������ײ����")]
    public LayerMask collisionMask;

    [Tooltip("���⴩ǽ�ľ���")]
    [Range(0.1f, 1f)] public float wallClipDistance = 0.5f;

    [Header("�߼��������")]
    [Tooltip("���ö�̬�������")] public bool enableDistanceControl = false;

    [Tooltip("��С�������")]
    [Min(1f)] public float minDistance = 3f;

    [Tooltip("���������")]
    [Min(5f)] public float maxDistance = 10f;

    [Tooltip("����������")]
    [Range(0.1f, 2f)] public float zoomSensitivity = 1f;

    [Header("�����˳��ӽ�")]
    [Tooltip("���õ����˳��ӽǿ���")] public bool enableThirdPersonView = false;

    [Tooltip("�ӽ���ת�ٶ�")]
    [Range(0.1f, 5f)] public float rotationSpeed = 1f;

    // ˽�б���
    private Vector3 desiredPosition;    // �����������λ��
    private Vector3 adjustedPosition;   // �������λ�ã����⴩ǽ��
    private float currentDistance;      // ��ǰ�������
    private float currentYaw;           // ��ǰƫ���ǣ����ڵ����˳��ӽǣ�
    private Camera cam;                 // ��������
    private Vector3 lastTargetPosition; // Ŀ����һ֡��λ��

    void Start()
    {
        // ��ȡ��������
        cam = GetComponent<Camera>();

        // �Զ�������Ҷ���
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
                Debug.Log("�Զ��ҵ����Ŀ��: " + target.name);
            }
            else
            {
                Debug.LogWarning("�����δ�ҵ���Ҷ������ֶ�ָ��Ŀ��");
            }
        }

        // ��ʼ������
        currentDistance = Mathf.Abs(offset.z);

        // ���ó�ʼλ��
        if (target != null)
        {
            UpdateDesiredPosition();
            transform.position = desiredPosition;
            transform.LookAt(target.position);
            lastTargetPosition = target.position;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // ��������λ��
        UpdateDesiredPosition();

        // ���⴩ǽ
        adjustedPosition = AdjustForWallCollision(desiredPosition);

        // ƽ���ƶ������
        transform.position = Vector3.Lerp(
            transform.position,
            adjustedPosition,
            smoothSpeed * Time.deltaTime
        );

        // ƽ����ת�����
        if (rotateWithTarget)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSmoothness * Time.deltaTime
            );
        }

        // ����Ŀ��λ��������һ֡
        lastTargetPosition = target.position;
    }

    void Update()
    {
        if (target == null) return;

        // ����������
        if (enableDistanceControl)
        {
            HandleDistanceControl();
        }

        // ��������˳��ӽǿ���
        if (enableThirdPersonView)
        {
            HandleThirdPersonView();
        }
    }

    // ��������λ��
    private void UpdateDesiredPosition()
    {
        // �������ƫ��
        Vector3 baseOffset = new Vector3(
            followX ? offset.x : 0f,
            followY ? offset.y : 0f,
            followZ ? offset.z : 0f
        );

        // Ӧ�þ������
        if (enableDistanceControl)
        {
            baseOffset.z = -currentDistance;
        }

        // Ӧ�õ����˳��ӽ���ת
        if (enableThirdPersonView)
        {
            Quaternion rotation = Quaternion.Euler(0f, currentYaw, 0f);
            desiredPosition = target.position + rotation * baseOffset;
        }
        else
        {
            // ��׼����
            desiredPosition = target.position +
                              target.right * baseOffset.x +
                              target.up * baseOffset.y +
                              target.forward * baseOffset.z;
        }
    }

    // ����������
    private void HandleDistanceControl()
    {
        // �����ֿ��ƾ���
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            currentDistance = Mathf.Clamp(
                currentDistance - scroll * zoomSensitivity,
                minDistance,
                maxDistance
            );
        }
    }

    // ��������˳��ӽǿ���
    private void HandleThirdPersonView()
    {
        // ����Ҽ���ת�ӽ�
        if (Input.GetMouseButton(1))
        {
            currentYaw += Input.GetAxis("Mouse X") * rotationSpeed;
        }
    }

    // ���⴩ǽ�ķ���
    private Vector3 AdjustForWallCollision(Vector3 targetPosition)
    {
        // ����ҵ�������ķ���
        Vector3 direction = targetPosition - target.position;
        float distance = direction.magnitude;
        direction.Normalize();

        // ʹ������Ͷ������ײ
        RaycastHit hit;
        if (Physics.SphereCast(target.position, 0.2f, direction, out hit, distance, collisionMask))
        {
            // �������ǽ�ڣ����������λ�ñ��⴩ǽ
            return target.position + direction * (hit.distance - wallClipDistance);
        }

        return targetPosition;
    }

    // ��ʾ������Ϣ
    void OnDrawGizmosSelected()
    {
        if (target == null) return;

        // ��������λ��
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(desiredPosition, 0.3f);

        // ����ʵ��λ��
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.2f);

        // ���Ʊ��⴩ǽ����
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(adjustedPosition, 0.15f);

        // �����������Ŀ�����
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(target.position, transform.position);

        // ������ײ�������
        Gizmos.color = new Color(1, 0.5f, 0, 0.5f);
        Vector3 direction = desiredPosition - target.position;
        Gizmos.DrawLine(target.position, target.position + direction.normalized * direction.magnitude);
        Gizmos.DrawWireSphere(target.position, 0.2f);
    }

    // ����������������Ŀ��
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        if (target != null)
        {
            lastTargetPosition = target.position;
        }
    }

    // �������������������λ��
    public void ResetCameraPosition()
    {
        if (target != null)
        {
            UpdateDesiredPosition();
            transform.position = desiredPosition;
            transform.LookAt(target.position);
        }
    }

    // �����������л�����ģʽ
    public void ToggleFollowMode(bool followX, bool followY, bool followZ)
    {
        this.followX = followX;
        this.followY = followY;
        this.followZ = followZ;
    }

    // ��������������ƫ����
    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
        if (enableDistanceControl)
        {
            currentDistance = Mathf.Abs(offset.z);
        }
    }
}