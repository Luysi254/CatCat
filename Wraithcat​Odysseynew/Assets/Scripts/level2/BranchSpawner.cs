using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GrassSettings
{
    public GameObject grassObject;
    public int branchCount = 5; // ���Ե�������ÿ��ݵص���֦����
}

public class BranchSpawner : MonoBehaviour
{
    public GameObject branchPrefab;
    public GrassSettings[] grassSettings; // ��Inspector������ÿ��ݵ�
    private bool hasSpawned = false;

    void Start()
    {
        if (!hasSpawned)
        {
            SpawnBranches();
            hasSpawned = true;
        }
    }

    void SpawnBranches()
    {
        if (grassSettings == null || grassSettings.Length == 0)
        {
            Debug.LogError("δ���òݵ����ã�");
            return;
        }

        foreach (GrassSettings setting in grassSettings)
        {
            if (setting.grassObject != null)
            {
                SpawnBranchesOnGrass(setting.grassObject, setting.branchCount);
            }
            else
            {
                Debug.LogWarning("����δ����Ĳݵض���");
            }
        }
    }

    void SpawnBranchesOnGrass(GameObject grass, int branchCount)
    {
        BoxCollider collider = grass.GetComponent<BoxCollider>();
        if (collider == null)
        {
            Debug.LogError($"�ݵ� {grass.name} û�� BoxCollider �����");
            return;
        }

        Vector3 size = collider.size;
        Vector3 center = collider.center;
        Vector3 halfExtents = new Vector3(
            size.x * grass.transform.localScale.x / 2,
            size.y * grass.transform.localScale.y / 2,
            size.z * grass.transform.localScale.z / 2
        );

        float grassTopY = grass.transform.position.y + center.y + halfExtents.y;

        for (int j = 0; j < branchCount; j++)
        {
            Vector3 randomPos = new Vector3(
                grass.transform.position.x + center.x + Random.Range(-halfExtents.x, halfExtents.x),
                grassTopY,
                grass.transform.position.z + center.z + Random.Range(-halfExtents.z, halfExtents.z)
            );

            Debug.Log($"�ڲݵ� {grass.name} ���ɵ� {j + 1}/{branchCount} ����֦");
            Instantiate(branchPrefab, randomPos, Quaternion.identity).tag = "Branch";
        }
    }
}