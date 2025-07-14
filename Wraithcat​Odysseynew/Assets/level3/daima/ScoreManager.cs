using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text pineNutText;  // ����UI�ı�����
    private int pineNutsCollected = 0;
    private int totalPineNuts = 5;

    void Start()
    {
        // ��ʼ���ı�
        UpdateScoreText();
    }

    public void CollectPineNut()
    {
        pineNutsCollected++;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        pineNutText.text = $"����: {pineNutsCollected}/{totalPineNuts}";
    }
}

