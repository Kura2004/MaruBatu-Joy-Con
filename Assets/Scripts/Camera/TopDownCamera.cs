using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [SerializeField] Transform target; // ターゲットのゲームオブジェクト
    [SerializeField] float YOffset = 10f; // カメラの高さ（Y座標のオフセット）
    [SerializeField] float ZOffset = -10f; // カメラのZ座標のオフセット

    void Start()
    {
        if (target != null)
        {
            // ターゲットの真上にカメラを配置
            Vector3 newPosition = target.position;
            newPosition.y += YOffset;
            transform.position = newPosition;

            // カメラの向きをターゲットに向ける
            transform.LookAt(target);

            newPosition.z += ZOffset;
            transform.position = newPosition;
        }

        // 配置が終わったら削除
        Destroy(this);
    }
}

