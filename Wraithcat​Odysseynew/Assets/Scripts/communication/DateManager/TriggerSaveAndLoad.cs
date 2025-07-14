using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TriggerSaveAndLoad : MonoBehaviour
{
    public GameObject cat;
    public List<GameObject> objectsToSave;
    public string targetSceneName;

    private bool canTrigger = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == cat)
        {
            // ���Ŀ�곡���Ƿ��Ѿ������ʹ�
            if (GameManager1.Instance.visitedScenes.ContainsKey(targetSceneName) &&
                !GameManager1.Instance.visitedScenes[targetSceneName])
            {
                // ������Ϸ״̬
                GameManager1.Instance.SaveGame(cat, objectsToSave);

                // ��ǳ���Ϊ�ѷ���
                GameManager1.Instance.MarkSceneVisited(targetSceneName);

                // ����Ŀ�곡��
                SceneManager.LoadScene(targetSceneName);
            }
        }
    }

    private void ResetTrigger()
    {
        canTrigger = true;
    }

    // �����屻���¼���ʱ���ô���״̬
    private void OnEnable()
    {
        canTrigger = true;
    }
}