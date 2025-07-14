using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    public float scrollSpeedX = 0f; // X��������ٶ� (�ں���У�ͨ�������·"�ƶ�"����)
    public float scrollSpeedY = -0.5f; // Y��������ٶ� (��Plane�ϣ�Y�������Ӧģ��Z�ᣬҲ����"��ǰ"����)
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float offsetX = Time.time * scrollSpeedX;
        float offsetY = Time.time * scrollSpeedY;
        rend.material.mainTextureOffset = new Vector2(offsetX, offsetY);
    }
}
