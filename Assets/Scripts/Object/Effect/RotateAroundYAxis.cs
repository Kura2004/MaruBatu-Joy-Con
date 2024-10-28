using UnityEngine;

public class RotateAroundYAxis : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f; // 回転速度（度/秒）
    private Vector3 lastPosition; // 前フレームの位置を記録する変数

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        lastPosition = transform.position;
    }
}



