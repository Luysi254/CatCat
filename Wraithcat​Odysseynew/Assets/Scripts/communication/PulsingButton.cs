using UnityEngine;
using UnityEngine.UI; // ע��ʹ��UnityEngine.UI�����ռ�

public class PulsingButton : MonoBehaviour
{
    public float pulseSpeed = 1f;
    public float minScale = 0.95f;
    public float maxScale = 1.05f;

    private RectTransform rectTransform;
    private Vector3 originalScale;

    void Start()
    {
        // ��ȡ��ť��RectTransform
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }

    void Update()
    {
        // ʹ�����Һ���ʵ��ƽ������
        float scale = Mathf.Lerp(minScale, maxScale,
            (Mathf.Sin(Time.time * pulseSpeed) + 1) / 2);

        rectTransform.localScale = originalScale * scale;
    }
}