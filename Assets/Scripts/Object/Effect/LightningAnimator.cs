using UnityEngine;
using DG.Tweening;

public class LightningAnimator : MonoBehaviour
{
    [SerializeField]
    private GameObject lightningRed;  // 雷のプレハブ

    [SerializeField]
    private GameObject lightningBlue;

    [SerializeField]
    private float duration = 0.5f;  // 雷の持続時間

    [SerializeField]
    private float yOffset = 1.0f;  // 雷のY座標オフセット

    [SerializeField]
    private Camera mainCamera;  // 使用するカメラ

    [SerializeField]
    private CameraShaker shaker;  // カメラシェイカー

    private void OnDisable()
    {
        shaker.StopAnimation();  // カメラシェイカーのアニメーションを停止
    }
    /// <summary>
    /// 雷のアニメーションを開始するメソッド
    /// </summary>
    public void StartLightningAnimationRed()
    {
        // カメラの揺れを開始
        shaker.enabled = true;
        shaker.StartAnimation();

        if (lightningRed != null && mainCamera != null)
        {
            // カメラの視野に基づいて画面の中央を計算
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenCenter);
            worldPosition.y += yOffset;  // Y座標オフセットを適用

            // プレハブを画面の中央に生成
            GameObject lightningInstance = Instantiate(lightningRed, worldPosition, Quaternion.identity);

            // DOTween を使用して雷の持続時間後に自動的に削除し、カメラのアニメーションも停止する処理
            DOVirtual.DelayedCall(duration, () =>
            {
                Destroy(lightningInstance);  // 雷を削除
            });
        }
        else
        {
            if (lightningRed == null)
            {
                Debug.LogWarning("雷のプレハブが設定されていません。");
            }
            if (mainCamera == null)
            {
                Debug.LogWarning("カメラが設定されていません。");
            }
        }
    }

    public void StartLightningAnimationBlue()
    {
        // カメラの揺れを開始
        shaker.enabled = true;
        shaker.StartAnimation();

        if (lightningBlue != null && mainCamera != null)
        {
            // カメラの視野に基づいて画面の中央を計算
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenCenter);
            worldPosition.y += yOffset;  // Y座標オフセットを適用

            // プレハブを画面の中央に生成
            GameObject lightningInstance = Instantiate(lightningBlue, worldPosition, Quaternion.identity);

            // DOTween を使用して雷の持続時間後に自動的に削除し、カメラのアニメーションも停止する処理
            DOVirtual.DelayedCall(duration, () =>
            {
                Destroy(lightningInstance);  // 雷を削除
            });
        }
        else
        {
            if (lightningRed == null)
            {
                Debug.LogWarning("雷のプレハブが設定されていません。");
            }
            if (mainCamera == null)
            {
                Debug.LogWarning("カメラが設定されていません。");
            }
        }
    }
}
