using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class IntroManager : MonoBehaviour
{
    public TextMeshProUGUI storyText;    // �м�����
    public GameObject continueButton;    // ������ť
    public GameObject EyeCanvas;
    public EyeOpening eyeOpeningScript;  // EyeOpening�ű�

    public string fullText = "��о��Լ��������ƣ�����������Լ���˭��ֻ�뼫�������۾�....";  // ��ʾ����������
    public float typingSpeed = 0.1f;      // �����ٶ�

    void Start()
    {
        storyText.text = "";  // ��ʼʱ���
        continueButton.SetActive(false); // ��ť����
        StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        foreach (char letter in fullText.ToCharArray())
        {
            storyText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // ������ɺ���ʾ��ť
        continueButton.SetActive(true);
    }

    public void OnContinueClicked()
    {
        // ��ť���ʱ����EyeOpening
        eyeOpeningScript.StartOpening();
        gameObject.SetActive(false); // ��������Intro����
        EyeCanvas.SetActive(false);
    }
}