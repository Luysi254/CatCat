using UnityEngine;
using UnityEngine.SceneManagement;

public class SanjiaoButtonController : MonoBehaviour
{

    public GameObject button1NPC;
    public GameObject button2CAT;
    public GameObject button3NPC;

    // ��¼�������
    private int clickCount = 0;

    // ��������
    public string beginningSceneName = "Beginning";

    void Awake()
    {
        // ���ó�ʼ״̬��button1NPC��ʾ����������
        button1NPC.SetActive(true);
        button2CAT.SetActive(false);
        button3NPC.SetActive(false);
    }
    public void OnSanjiaoClicked()
    {
        clickCount++;

        // ���ε����ѭ��
        if (clickCount > 3)
        {
            clickCount = 1;
        }

        switch (clickCount)
        {
            case 1:
                // ��һ�ε��������1��3����ʾ2
                button1NPC.SetActive(false);
                button3NPC.SetActive(false);
                button2CAT.SetActive(true);
                break;

            case 2:
                // �ڶ��ε��������2��1����ʾ3
                button2CAT.SetActive(false);
                button1NPC.SetActive(false);
                button3NPC.SetActive(true);
                break;

            case 3:
                // �����ε��
                SceneManager.LoadScene(beginningSceneName);
                break;
        }
    }
}