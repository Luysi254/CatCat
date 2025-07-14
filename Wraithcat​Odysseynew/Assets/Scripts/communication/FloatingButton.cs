using UnityEngine;

public class FloatingButton : MonoBehaviour
{
    public float floatSpeed = 1f;      // �����ٶ�
    public float floatAmount = 10f;   // ��������

    private RectTransform rectTransform;
    private Vector2 startPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
    }

    void Update()
    {
        // ʹ�����Һ�������ƽ�������¸���Ч��
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        rectTransform.anchoredPosition = startPosition + new Vector2(0, yOffset);
    }
}