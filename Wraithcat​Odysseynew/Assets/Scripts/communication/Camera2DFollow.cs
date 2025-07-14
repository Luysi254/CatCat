using UnityEngine;

public class Camera2DFollow : MonoBehaviour
{
    public Transform target;    // èè
    public float smoothSpeed = 0.1f;
    public Vector3 offset = new Vector3(0, 0, -10);  // �����������-Z

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        desiredPosition.z = offset.z;  // �̶�Z��
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }
}