using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TenmanGenerator : MonoBehaviour
{
    public GameObject groundPrefab;   // �������Ԥ����
    public Transform player;          // �����������
    public int initialTiles = 2;      // ��ʼ����5��
    public float tileWidth = 1664f;     // һ�������

    private List<GameObject> activeTiles = new List<GameObject>();
    private float nextSpawnX = 0f;

    void Start()
    {
        for (int i = 0; i < initialTiles; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        if (player.position.x + tileWidth * 2f > nextSpawnX)
        {
            SpawnTile();
        }

        // ���ٶ���tile����ֹ���٣��ȼ���Ƿ���Ч��
        if (activeTiles.Count > initialTiles + 2 && activeTiles[0] != null)
        {
            Destroy(activeTiles[0]);
            activeTiles.RemoveAt(0);
        }

    }

    void SpawnTile()
    {
        Vector3 spawnPos = new Vector3(nextSpawnX, 164f, 921f);
        GameObject tile = Instantiate(groundPrefab, spawnPos, Quaternion.identity);
        activeTiles.Add(tile);
        nextSpawnX += tileWidth;
    }
}
