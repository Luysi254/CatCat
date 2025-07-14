using UnityEngine;
using System.Collections.Generic;

public class SceneLoader : MonoBehaviour
{
    public GameObject cat;
    public List<GameObject> objectsToLoad;
    public GameObject endUI;
    public GameObject kaishiPanel;

    private int scenesCompleted = 0;

    void Start()
    {
        if (kaishiPanel != null)
        {
            kaishiPanel.SetActive(false);
        }

        // ������Ϸ״̬
        if (GameManager1.Instance != null)
        {
            GameManager1.Instance.LoadGame(cat, objectsToLoad);

            // ����Ƿ����г����������
            CheckAllScenesCompleted();

            if (GameManager1.Instance.showEndUI)
            {
                endUI.SetActive(true);
            }
        }
    }

    private void CheckAllScenesCompleted()
    {
        scenesCompleted = 0;
        if (GameManager1.Instance.visitedScenes["Level1"]) scenesCompleted++;
        if (GameManager1.Instance.visitedScenes["Level2"]) scenesCompleted++;
        if (GameManager1.Instance.visitedScenes["Level3"]) scenesCompleted++;

        if (scenesCompleted >= 3)
        {
            GameManager1.Instance.showEndUI = true;
            endUI.SetActive(true);
        }
    }
}