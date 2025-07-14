using UnityEngine;

public class Key : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Կ�ױ��ռ�!");
            GameManager.instance?.ObtainKey(); // ֱ�ӵ��� GameManager
            Destroy(gameObject);
        }
    }
}