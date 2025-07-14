using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Catt : MonoBehaviour
{
    // ׹��߶���ֵ
    public float fallDeathHeight = -15f;
    // �ƶ�����
    public float moveSpeed = 5f;
    public float rotationSpeed = 200f;
    public LayerMask groundLayer;

    // ����
    public Transform headPosition;
    public GameObject branchPrefab;
    public float branchYOffset = 0.5f;
    public float groundCheckDistance = 0.5f;
    public Transform footPosition;

    // ״̬
    private GameObject carriedBranch;
    private bool isBranchForm = false;
    private Rigidbody rb;
    private CapsuleCollider col;
    private bool isGrounded;

    private List<GameObject> highlightedBranches = new List<GameObject>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        if (rb != null) rb.useGravity = false;

        // ������ʼ��ת
        transform.rotation = Quaternion.Euler(-90f, 0, 0);
    }

    void Update()
    {
        CheckGroundStatus();
        if (!isBranchForm)
        {
            HandleMovement(); // �޸��ƶ��߼�
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (carriedBranch == null && highlightedBranches.Count > 0)
            {
                // ʰȡ��һ����������֦
                PickupBranch(highlightedBranches[0]);
            }
            else if (carriedBranch != null)
            {
                DropBranch();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ToggleForm();
        }
        CheckFallDeath();
    }
    void CheckFallDeath()
    {
        if (transform.position.y < fallDeathHeight)
        {
            ReloadScene();
        }
    }

    // �������������¼��ص�ǰ����
    void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

        // ����ֱ��ʹ�ó������ƣ����ȶ���
        // SceneManager.LoadScene("��ĳ�������");
    }
    void CheckGroundStatus()
    {
        // 1. ����׼���棨�ݵأ�- �޸�Ϊ��̬������
        float dynamicCheckDistance = groundCheckDistance + Mathf.Abs(transform.position.y);
        bool isOnGround = Physics.Raycast(
            transform.position + Vector3.up * 0.1f,
            Vector3.down,
            dynamicCheckDistance,  // ��̬���������
            groundLayer
        );

        // 2. ����ѷ��õ���������ȫ����ԭ���߼���
        bool isOnBridge = false;
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (var collider in nearbyColliders)
        {
            BranchInfo branchInfo = collider.GetComponent<BranchInfo>();
            if (branchInfo != null && branchInfo.isPlaced)
            {
                isOnBridge = true;
                break;
            }
        }

        // 3. �ϲ���������䣩
        isGrounded = isOnGround || isOnBridge;

        // 4. �������������䣩
        if (rb != null)
        {
            rb.useGravity = !isGrounded;
            if (isGrounded && rb.velocity.y < 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            }
        }

        // 5. ������ǿ�����أ����ڼ�⵽����ʱ��Ч��
        if (isOnGround)
        {
            RaycastHit groundHit;
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f,
                              Vector3.down,
                              out groundHit,
                              dynamicCheckDistance,
                              groundLayer))
            {
                float targetY = groundHit.point.y + col.height * 0.5f;
                transform.position = new Vector3(
                    transform.position.x,
                    targetY,
                    transform.position.z
                );
            }
        }
    }

    // �޸��ƶ��߼����ؼ��޸ģ�
    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 1. ��ȡ�����Է��򣨺���Y�ᣩ
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // 2. ��������ռ���ƶ�����
        Vector3 cameraRelativeDir = (cameraForward * vertical) + (cameraRight * horizontal);
        if (cameraRelativeDir == Vector3.zero) return;

        // 3. ���������ת��Ϊ���緽��
        float inputAngle = Mathf.Atan2(cameraRelativeDir.x, cameraRelativeDir.z) * Mathf.Rad2Deg;
        float snappedAngle = Mathf.Round(inputAngle / 90) * 90; // ���뵽0/90/180/270

        // 4. ����ʵ���ƶ����򣨻��ڶ����ĽǶȣ�
        Vector3 worldMoveDir = Quaternion.Euler(0, snappedAngle, 0) * Vector3.forward;

        // 5. Ӧ���ƶ�����ת
        transform.position += worldMoveDir * moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(-90f, snappedAngle, 0);
    }


    // �л���̬
    void ToggleForm()
    {
        isBranchForm = !isBranchForm;

        // ����/��ʾСèģ��
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.enabled = !isBranchForm;
        }

        // ��֦��̬ʱ������ײ����ƶ�
        col.enabled = !isBranchForm;
        rb.isKinematic = isBranchForm;
    }

    // ʰȡ��֦�߼�
    void PickupBranch(GameObject branch)
    {
        ResetAllHighlightedBranches();
        // ����Я������֦������ͷ��
        carriedBranch = Instantiate(branchPrefab, headPosition.position, Quaternion.identity);
        carriedBranch.transform.SetParent(headPosition);
        carriedBranch.transform.localRotation = Quaternion.Euler(0, 90, 90);
        carriedBranch.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        // �Ƴ������е�ԭ��֦
        Destroy(branch);

        Debug.Log("ʰȡ��֦�ɹ���");
    }
    void ResetAllHighlightedBranches()
    {
        foreach (var branch in highlightedBranches)
        {
            if (branch != null)
            {
                BranchData data = branch.GetComponent<BranchInfo>().data;
                branch.transform.position = data.originalPosition;
                branch.GetComponent<Renderer>().material.color = data.originalColor;
            }
        }
        highlightedBranches.Clear();
    }

    // ������֦�߼�
    void DropBranch()
    {
        if (carriedBranch != null)
        {
            // ��鵱ǰλ���Ƿ���Grass������
            Collider[] areaColliders = Physics.OverlapSphere(footPosition.position, 0.5f);
            bool isInGrassArea = false;

            foreach (var collider in areaColliders)
            {
                if (collider.CompareTag("Grass") && collider.gameObject.name.Contains("GrassArea"))
                {
                    isInGrassArea = true;
                    break;
                }
            }

            if (isInGrassArea)
            {
                Debug.Log("�˴����ɴ������");
                // �������������UI��ʾ�����磺
                // ShowNotification("�ݵ������ܴ������");
                return; // ֱ�ӷ��أ���ִ�к�����߼�
            }

            // ʹ����ʰȡʱ��ͬ����ת
            Quaternion bridgeRotation = carriedBranch.transform.rotation;

            Vector3 bridgePosition = footPosition.position;
            bridgePosition.y -= 0.2f; // ΢���߶�

            // ��ѡ����ӵ�����ȷ���������ڵ�����
            RaycastHit hit;
            if (Physics.Raycast(bridgePosition + Vector3.up * 0.5f, Vector3.down, out hit, 1f, groundLayer))
            {
                bridgePosition.y = hit.point.y;
            }

            // ��������
            GameObject bridge = Instantiate(
                carriedBranch,
                bridgePosition,
                bridgeRotation  // ʹ��һ�µ���ת
            );

            Collider bridgeCollider = bridge.GetComponent<Collider>();
            if (bridgeCollider != null)
            {
                bridgeCollider.isTrigger = false; // �ؼ��޸ģ��ر�Trigger
            }

            // ����ԭ�б���߼�
            bridge.tag = "Untagged";
            BranchInfo bridgeInfo = bridge.GetComponent<BranchInfo>() ?? bridge.AddComponent<BranchInfo>();
            bridgeInfo.isPlaced = true;

            // �����ŵĸ�����ʹ�С
            bridge.transform.SetParent(null);
            bridge.transform.localScale = new Vector3(1f, 1f, 1.2f);

            // �Ƴ�Я������֦
            Destroy(carriedBranch);
            carriedBranch = null;

            Debug.Log("������֦�������ţ�");
        }
    }

    // ����Ƿ񿿽���֦��������ʾ
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Branch") && carriedBranch == null)
        {
            // ȷ�� BranchInfo �������
            BranchInfo branchInfo = other.GetComponent<BranchInfo>();
            if (branchInfo == null || !branchInfo.isPlaced)
            {
                if (branchInfo == null) branchInfo = other.gameObject.AddComponent<BranchInfo>();

                // ��¼ԭʼ״̬
                branchInfo.data = new BranchData
                {
                    originalPosition = other.transform.position,
                    originalColor = other.GetComponent<Renderer>().material.color
                };

                // ��������
                other.transform.Translate(Vector3.up * branchYOffset);
                other.GetComponent<Renderer>().material.color = Color.yellow;

                highlightedBranches.Add(other.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Branch") && highlightedBranches.Contains(other.gameObject))
        {
            // ��λ
            BranchData data = other.GetComponent<BranchInfo>().data;
            other.transform.position = data.originalPosition;
            other.GetComponent<Renderer>().material.color = data.originalColor;

            highlightedBranches.Remove(other.gameObject);
        }
    }

}