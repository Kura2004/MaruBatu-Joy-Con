using UnityEngine;

public class RotateAroundYAxis : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f; // 回転速度（度/秒）
    private Vector3 lastPosition; // 前フレームの位置を記録する変数

    private void Update()
    {
        if (!TimeControllerToggle.isTimeStopped)
        {
            // Y軸を中心に回転
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // 現在の位置を次フレームのために記録
            lastPosition = transform.position;
        }
        else
        {
            // 時間停止中は位置を前フレームの位置にリセット
            transform.position = lastPosition;
        }
    }
}



