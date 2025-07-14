using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverShadowEffect : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{
    [Header("��Text�Ӷ���")]
    public Text targetText; // �����Text�Ӷ���

    private Shadow textShadow; // Shadow�������

    void Start()
    {
        // �Զ���ȡShadow���
        if (targetText != null)
            textShadow = targetText.GetComponent<Shadow>();

        // Ĭ������Shadow
        if (textShadow != null)
            textShadow.enabled = false;
    }

    // ������ʱ��ʾShadow
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (textShadow != null)
            textShadow.enabled = true;
    }

    // ����뿪ʱ����Shadow
    public void OnPointerExit(PointerEventData eventData)
    {
        if (textShadow != null)
            textShadow.enabled = false;
    }
}