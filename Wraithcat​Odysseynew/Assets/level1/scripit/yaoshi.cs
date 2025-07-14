using UnityEngine;
using System.Collections;

public class yaoshi : MonoBehaviour
{
    public enum KeyType { General, PipePuzzle }
    public KeyType keyType = KeyType.PipePuzzle;

    [Header("UI����")]
    [Tooltip("�л���KeyGetMask������ӳ�ʱ��")]
    public float transitionDelay = 1f;

    private GameObject keyGetMaskCanvas;
    private bool collected = false;

    void Awake()
    {
        // ����Ԥ�����ʼλ�ã����ݽ�ͼ�е���ֵ��
        transform.localPosition = new Vector3(0.95f, -0.23f, 0f);
        transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        transform.localScale = new Vector3(0.00943f, 0.00943f, 0.00943f);

        // ȷ������������ڣ�ƥ���ͼ�е�Rigidbody��Collider��
        if (GetComponent<Rigidbody>() == null)
        {
            var rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true; // ��ֹ����Ӱ��
        }

        if (GetComponent<Collider>() == null)
        {
            var collider = gameObject.AddComponent<BoxCollider>();
            collider.size = Vector3.one * 0.3f;
        }
    }

    void Start()
    {
        // �Զ����һ��壨ƥ���ͼ�е�"KeyGetMask"����
        keyGetMaskCanvas = GameObject.Find("KeyGetMask");

        // ���ò��ҷ��������������Canvas�������У�
        if (keyGetMaskCanvas == null)
        {
            Transform canvas = GameObject.Find("Canvas")?.transform;
            if (canvas != null) keyGetMaskCanvas = canvas.Find("KeyGetMask")?.gameObject;
        }

        if (keyGetMaskCanvas != null)
        {
            keyGetMaskCanvas.SetActive(false); // ��ʼ����
        }
        else
        {
            Debug.LogError("�Զ�����ʧ�ܣ���ȷ�ϣ�1.��������Ϊ'KeyGetMask' 2.��������ڳ�����");
        }
    }

    void OnMouseDown()
    {
        if (!collected) StartCoroutine(PickupKey());
    }

    IEnumerator PickupKey()
    {
        collected = true;

        // ������ʧ����
        Vector3 originalScale = transform.localScale;
        float timer = 0;
        while (timer < transitionDelay)
        {
            timer += Time.deltaTime;
            transform.localScale = originalScale * (1 - timer / transitionDelay);
            yield return null;
        }

        // ��ȫ�����
        if (keyGetMaskCanvas != null)
        {
            keyGetMaskCanvas.SetActive(true);
            PlayerPrefs.SetInt("PipePuzzleCompleted", 1);
        }
        else
        {
            Debug.LogWarning("���弤��ʧ�ܣ�������ִ������");
        }

        Destroy(gameObject);
    }

    // ���Կ��ӻ�����ѡ��
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
}