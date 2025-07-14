using UnityEngine;

public class EyeOpening : MonoBehaviour
{
    // ���������Ļ
    public RectTransform leftMask;  // ����Ļ
    public RectTransform rightMask; // �Ҳ��Ļ
    public float openSpeed = 500f;  // ���ٶ�

    private float targetX;    // Ŀ��λ��
    private bool isOpening = false; // �Ƿ���������

    void Start()
    {

        
        targetX = Screen.width / 2f;  // ������Ļһ����
    }

    void Update()
    {
        if (!isOpening) return; // û�����Ͳ�ִ��

        // ���Һ�Ļ�ֱ��ƶ���Ŀ��λ��
        leftMask.anchoredPosition = Vector2.MoveTowards(leftMask.anchoredPosition, new Vector2(-targetX, 0), openSpeed * Time.deltaTime);
        rightMask.anchoredPosition = Vector2.MoveTowards(rightMask.anchoredPosition, new Vector2(targetX, 0), openSpeed * Time.deltaTime);

        // �ж��Ƿ��������
        if (Mathf.Abs(leftMask.anchoredPosition.x + targetX) < 1f)
        {
            leftMask.gameObject.SetActive(false);
            rightMask.gameObject.SetActive(false);
            isOpening = false;
        }
    }

    // �������������ʼ����
    public void StartOpening()
    {
        isOpening = true;
    }
}