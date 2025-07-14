using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    public float typingSpeed = 0.1f; 
    private TMP_Text tmpText;
    private string fullText;
    private Coroutine typingCoroutine; // ���ڴ洢Э������

    void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
        fullText = tmpText.text;
        tmpText.text = ""; // ��ճ�ʼ�ı�

        // ֻ�г�ʼ����Ķ����������ʼ����
        if (gameObject.activeInHierarchy)
        {
            StartTyping();
        }
    }

    // �����󱻼���ʱ����
    void OnEnable()
    {
        // ����ı����ݴ�������δ��ʼ����
        if (fullText != null && typingCoroutine == null)
        {
            StartTyping();
        }
    }

    // �����󱻽���ʱ����
    void OnDisable()
    {
        // ֹͣ���ڽ��еĴ���Ч��
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
    }

    // ��ʼ����Ч��
    void StartTyping()
    {
        // ȷ����ֹͣ���е�Э��
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            tmpText.text = fullText.Substring(0, i);
            yield return new WaitForSeconds(typingSpeed);
        }
        typingCoroutine = null; // ������ɺ��������
    }

    // ��ѡ��������÷���
    public void ResetTypewriter()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        tmpText.text = "";

        if (gameObject.activeInHierarchy)
        {
            StartTyping();
        }
    }
}