using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject groundPrefab;   // �������Ԥ����
    public Transform player;          // �����������
    public int initialTiles = 0;      // ��ʼ����5��
    public float tileWidth = 1664f;     // һ�������

    private List<GameObject> activeTiles = new List<GameObject>();
    private float nextSpawnX = 1075f;
 

    void Start()
    {
        for (int i = 0; i < initialTiles; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        // ��ǰ������뿪ʼ���ɣ���ֹ���ǳ���
        if (player.position.x + tileWidth * 2f > nextSpawnX)
        {
            SpawnTile();
        }

        // ���ٶ���tile����ֹ����
        if (activeTiles.Count > initialTiles + 2)
        {
            Destroy(activeTiles[0]);
            activeTiles.RemoveAt(0);
        }
       

    }

    void SpawnTile()
    {
        Vector3 spawnPos = new Vector3(nextSpawnX, -539f, 741f);
        GameObject tile = Instantiate(groundPrefab, spawnPos, Quaternion.identity);
        activeTiles.Add(tile);
        
        nextSpawnX += tileWidth;
    }

}